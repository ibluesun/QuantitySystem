﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ParticleLexer;
using ParticleLexer.StandardTokens;
using QsRoot;


namespace Qs.Types
{

    /// <summary>
    /// Holds QsObject either native (from C# class) or custom (from qs itself)
    /// </summary>
    public class QsObject : QsValue
    {
        /// <summary>
        /// The only way to create the QsObject.
        /// </summary>
        /// <param name="nativeObject"></param>
        /// <returns></returns>
        public static QsObject CreateNativeObject(object nativeObject)
        {
            return new QsObject(nativeObject);
        }

        private readonly object _NativeObject;
        private QsObject(object nativeObject)
        {
            _NativeObject = nativeObject;
        }

        public Object ThisObject => _NativeObject;
        

        /// <summary>
        /// Native Type.
        /// </summary>
        public Type InstanceType => _NativeObject.GetType();
        
        
        public override  QsValue Execute(Token expression)
        {
            Expression ResultExpression = null;
            if (expression.TokenClassType == typeof(ParenthesisCallToken))
            {
                // function call
                string MethodName = expression[0].TokenValue;
                string[] args = expression[1].TrimTokens(1, 1).TokenValue.Split(',');
                int argn = args.Length;
                
                if (args.Length == 1 && args[0] == string.Empty) argn=0;

                var d_method = InstanceType.GetMethods().First(
                    c => c.GetParameters().Length == argn
                        && c.Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase)
                        && c.IsStatic == false    // important .. calling a function from object instance is for sure not static
                      );

                var d_method_params = d_method.GetParameters();

                var argsToken = expression[1];

                List<Expression> Arguments = new List<Expression>();

                int ix = 0;
                foreach (var tk in argsToken)
                {
                    if (tk.TokenClassType == typeof(ParameterToken))
                    {
                        var qv = new global::Qs.Runtime.QsVar(
                            global::Qs.Runtime.QsEvaluator.CurrentEvaluator
                            , tk);

                        // evaluate the expresion
                        var expr = qv.ResultExpression;  // the return value is QsValue

                        // we have to check the corresponding parameter type to make convertion if needed.
                        Type targetType = d_method_params[ix].ParameterType;

                        Arguments.Add(Root.QsToNativeConvert(targetType, expr));
                        ix++;
                    }
                }

                // parameters has been prepared.
                ResultExpression = Expression.Call(
                    Expression.Constant(ThisObject)
                    , d_method
                    , Arguments.ToArray()
                    );

                if (d_method.ReturnType == typeof(void))
                {
                    var cq = Expression.Lambda<Action>(ResultExpression);

                    var aqf = cq.Compile();

                    aqf();

                    return null;
                }
                else
                {
                    // determine the return type to conver it into suitable QsValue
                    var mi = typeof(Root).GetMethod("NativeToQsConvert", System.Reflection.BindingFlags.IgnoreCase| System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    ResultExpression = Expression.Call(mi, Expression.Convert(ResultExpression, typeof(object)));

                    Expression<Func<QsValue>> cq = Expression.Lambda<Func<QsValue>>(ResultExpression);

                    Func<QsValue> aqf = cq.Compile();

                    QsValue result = aqf();

                    return result;
                }

            }
            else
            {
                // property access.
               ResultExpression =  Expression.Property(Expression.Constant(_NativeObject), expression.TokenValue);

               var mi = typeof(Root).GetMethod("NativeToQsConvert", System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
               ResultExpression = Expression.Call(mi, Expression.Convert(ResultExpression, typeof(object)));


               // Construct Lambda function which return one object.
               Expression<Func<QsValue>> cq = Expression.Lambda<Func<QsValue>>(ResultExpression);

               // compile the function
               Func<QsValue> aqf = cq.Compile();

               // execute the function
               QsValue result = aqf();

               // return the result
               return result;

            }

        }

        public void SetProperty(string propertyName, object value)
        {
            var pi = InstanceType.GetProperty(propertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);

            var vc = Root.QsToNativeConvert(pi.PropertyType, value);

            pi.SetValue(_NativeObject, vc, null);
        }

        public PropertyInfo GetPropertyInfo(string propertyName)
        {
            return InstanceType.GetProperty(propertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);
        }


        /// <summary>
        /// the function will realize between property that is an array and needs to be accessed as an array
        /// and a property that is an object with indexer and needs to be treated that way
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public QsValue GetIndexedProperty(string propertyName, params object[] indices)
        {
            var pi = GetPropertyInfo(propertyName);

            if (pi.PropertyType.IsArray)
            {
                Array array = (Array)pi.GetValue(_NativeObject, null);
                return QsRoot.Root.NativeToQsConvert(array.GetValue((int)indices[0]));
            }
            else if (pi.GetIndexParameters().Length > 0)
            {
                return GetProperty(propertyName, indices);
            }
            else
            {
                throw new QsException("The property " + propertyName + " is not array nor object to be indexed");
            }
        }

        /// <summary>
        /// Gets the property value from the underlieng object in this QsObject.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="indices">indices in case of indexed property</param>
        /// <returns></returns>
        public QsValue GetProperty(string propertyName, params object[] indices )
        {
            var pi = GetPropertyInfo(propertyName);

            return Root.NativeToQsConvert(pi.GetValue(_NativeObject, indices));
        }

        public QsValue GetProperty(string propertyName)
        {
            return GetProperty(propertyName, null);
        }

        public override string ToString()
        {
            return "Native: " + _NativeObject.ToString();
        }

        public override string ToShortString()
        {
            return InstanceType.Name;
        }


        #region QsValue Implementation
        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        /*
        
        /// <summary>
        /// Get the operation method dynamically between the current object and the target object.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DynamicMethod GetOperationMethod(string operation, QsValue value)
        {
            var o = value as QsObject;
            if (o != null)
            {
                var p1 = this.ThisObject;
                var p2 = o.ThisObject;
                var p1Type = this.InstanceType;
                var p2Type = o.InstanceType;

                // creating the dynamic that will hold the operation
                DynamicMethod method = new DynamicMethod(operation, p1Type,
                    new Type[] { p1Type, p2Type });

                //get generator to construct the function.
                ILGenerator gen = method.GetILGenerator();
                gen.Emit(OpCodes.Ldarg_0);  //load the first value
                gen.Emit(OpCodes.Ldarg_1);  //load the second value

                string op = string.Empty;
                if (operation.Equals(Operator.Plus, StringComparison.OrdinalIgnoreCase)) op = "op_Addition";
                if (operation.Equals(Operator.Minus, StringComparison.OrdinalIgnoreCase)) op = "op_Subtraction";
                if (operation.Equals(Operator.Divide, StringComparison.OrdinalIgnoreCase)) op = "op_Division";
                if (operation.Equals(Operator.Multiply, StringComparison.OrdinalIgnoreCase)) op = "op_Multiply";
                if (string.IsNullOrEmpty(op)) throw new QsException("+ - / and * operations are allowed");

                MethodInfo info = p1Type.GetMethod
                    (
                    op,
                    new Type[] { p1Type, p2Type }, 
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.
                gen.Emit(OpCodes.Ret);

                return method;
            }

            return null;
        }
        */

        public override QsValue AddOperation(QsValue value)
        {
            /*
            DynamicMethod dm = GetOperationMethod(Operator.Plus, value);

            if (dm != null)
            {
                var result = dm.Invoke(null, new object[] { this.ThisObject, ((QsObject)value).ThisObject });
                return QsObject.CreateNativeObject(result);

            }
            */

            var o = value as QsObject;
            if (o != null)
            {
                var expr = Expression.Add(Expression.Constant(_NativeObject), Expression.Constant(o._NativeObject));

                // Construct Lambda function which return one object.
                Expression<Func<object>> cq = Expression.Lambda<Func<object>>(expr);

                // compile the function
                Func<object> aqf = cq.Compile();

                // execute the function
                object result = aqf();

                // return the result
                return new QsObject(result);
            }
            else
            {
                throw new NotImplementedException("Operation between " + this.ToString() + " and " + value.ToString() + " is not supported");
            }
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            /*
            DynamicMethod dm = GetOperationMethod(Operator.Minus, value);

            if (dm != null)
            {
                var result = dm.Invoke(null, new object[] { this.ThisObject, ((QsObject)value).ThisObject });
                return QsObject.CreateNativeObject(result);

            }
            */

            var o = value as QsObject;
            if (o != null)
            {
                var expr = Expression.Subtract(Expression.Constant(_NativeObject), Expression.Constant(o._NativeObject));

                // Construct Lambda function which return one object.
                Expression<Func<object>> cq = Expression.Lambda<Func<object>>(expr);

                // compile the function
                Func<object> aqf = cq.Compile();

                // execute the function
                object result = aqf();

                // return the result
                return new QsObject(result);

            }
            else
            {
                throw new NotImplementedException("Operation between " + this.ToString() + " and " + value.ToString() + " is not supported");
            }
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            /*
            DynamicMethod dm = GetOperationMethod(Operator.Multiply, value);

            if (dm != null)
            {
                var result = dm.Invoke(null, new object[] { this.ThisObject, ((QsObject)value).ThisObject });
                return QsObject.CreateNativeObject(result);

            }
            */

            var o = value as QsObject;
            if (o != null)
            {
                var expr = Expression.Multiply(Expression.Constant(_NativeObject), Expression.Constant(o._NativeObject));

                // Construct Lambda function which return one object.
                Expression<Func<object>> cq = Expression.Lambda<Func<object>>(expr);

                // compile the function
                Func<object> aqf = cq.Compile();

                // execute the function
                object result = aqf();

                // return the result
                return new QsObject(result);

            }
            else
            {
                throw new NotImplementedException("Operation between " + this.ToString() + " and " + value.ToString() + " is not supported");
            }
        }

        public override QsValue DivideOperation(QsValue value)
        {
            /*
            DynamicMethod dm = GetOperationMethod(Operator.Divide, value);

            if (dm != null)
            {
                var result = dm.Invoke(null, new object[] { this.ThisObject, ((QsObject)value).ThisObject });
                return QsObject.CreateNativeObject(result);

            }
            */
            var o = value as QsObject;
            if (o != null)
            {
                var expr = Expression.Divide(Expression.Constant(_NativeObject), Expression.Constant(o._NativeObject));

                // Construct Lambda function which return one object.
                Expression<Func<object>> cq = Expression.Lambda<Func<object>>(expr);

                // compile the function
                Func<object> aqf = cq.Compile();

                // execute the function
                object result = aqf();

                // return the result
                return new QsObject(result);

            }
            else
            {
                throw new NotImplementedException("Operation between " + this.ToString() + " and " + value.ToString() + " is not supported");
            }
        }

        
        public override QsValue PowerOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue ModuloOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool LessThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool LessThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool Equality(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool Inequality(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue DotProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue TensorProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue NormOperation()
        {
            throw new NotImplementedException();
        }

        public override QsValue AbsOperation()
        {
            throw new NotImplementedException();
        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue LeftShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue GetIndexedItem(QsParameter[] indices)
        {   
            
            
            var pi = InstanceType.GetProperty("Item"
                , System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public 
                );

            

            var r = Root.QsParametersToNativeValues(pi.GetGetMethod(), indices);

            return Root.NativeToQsConvert(pi.GetValue(_NativeObject, r));   
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            var pi = InstanceType.GetProperty("Item"
                , System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public
                );
            var gs = pi.GetSetMethod();

            var r = Root.QsParametersToNativeValues(gs, indices);

            var nativeValue = Root.QsToNativeConvert(pi.PropertyType, value);

            pi.SetValue(_NativeObject, nativeValue, r);
        }
        #endregion


        public override QsValue ExclamationOperator(QsValue key)
        {
            if (key.ToString().Equals("TypeName", StringComparison.OrdinalIgnoreCase))
            {
                return new QsText(InstanceType.Name);
            }

            if (key.ToString().Equals("Properties", StringComparison.OrdinalIgnoreCase))
            {
                QsFlowingTuple f = new QsFlowingTuple();
                var mms = InstanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                QsTupleValue[] fefe = new QsTupleValue[mms.Length];
            
                for (int ix = 0; ix < mms.Length; ix++)
                {
                    fefe[ix].Name = (string) mms[ix].Name;
                    
                    fefe[ix].Value = new QsText(mms[ix].PropertyType.Name);
                }
                if (fefe.Length == 0) return new QsFlowingTuple();
                return new QsFlowingTuple(fefe);
            }

            if (key.ToString().Equals("Methods", StringComparison.OrdinalIgnoreCase))
            {
                QsFlowingTuple f = new QsFlowingTuple();
                var mms = from m in InstanceType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                          where m.IsSpecialName == false
                          select m;

                QsTupleValue[] fefe = new QsTupleValue[mms.Count()];

                for (int ix = 0; ix < mms.Count(); ix++)
                {
                    fefe[ix].Name = (string)mms.ElementAt(ix).Name;

                    fefe[ix].Value = new QsText(mms.ElementAt(ix).ReturnType.Name);
                }
                if (fefe.Length == 0) return new QsFlowingTuple();
                return new QsFlowingTuple(fefe);
            }

            if (key.ToString().Equals("Type", StringComparison.OrdinalIgnoreCase))
            {
                return new QsObject(InstanceType);
            }
            return new QsText("Unknown Command");
        }
    }
}
