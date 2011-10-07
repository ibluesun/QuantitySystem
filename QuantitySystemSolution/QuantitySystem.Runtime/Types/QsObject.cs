using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ParticleLexer;
using ParticleLexer.StandardTokens;
using ParticleLexer.QsTokens;
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

        public Object ThisObject
        {
            get
            {
                return _NativeObject;
            }
        }

        /// <summary>
        /// Native Type.
        /// </summary>
        public Type InstanceType
        {
            get
            {
                return _NativeObject.GetType();
            }
        }
        
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
                    var mi = typeof(Root).GetMethod("NativeToQsConvert", System.Reflection.BindingFlags.IgnoreCase| System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
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

               var mi = typeof(Root).GetMethod("NativeToQsConvert", System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
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

        public object GetProperty(string propertyName)
        {
            var pi = InstanceType.GetProperty(propertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);

            return Root.NativeToQsConvert(pi.GetValue(_NativeObject, null));
        }


        public override string ToString()
        {
            return "Native: " + _NativeObject.ToString();
        }


        #region QsValue Implementation
        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        public override QsValue AddOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue DivideOperation(QsValue value)
        {
            throw new NotImplementedException();
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

        public override QsValue GetIndexedItem(int[] indices)
        {            
            var pi = InstanceType.GetProperty("Item", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);

            var r = from m in indices select (object)m;

            return Root.NativeToQsConvert(pi.GetValue(_NativeObject, r.ToArray()));   
        }
        #endregion
    }
}
