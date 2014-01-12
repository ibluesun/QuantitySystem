using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassiveFlow;
using System.Diagnostics.Contracts;
using ParticleLexer;
using QsRoot;
using System.Linq.Expressions;
using ParticleLexer.StandardTokens;
using Qs.Runtime;

namespace Qs.Types
{

    /// <summary>
    /// Tuple Element
    /// </summary>
    public struct QsTupleValue
    {
        public int Id;
        public string Name;


        /// <summary>
        /// Holds original source value that wasn't converted into Native Qs Value yet
        /// </summary>
        private  Object _LazyValue;
        public Object LazyValue
        {
            get { return _LazyValue; }
        }

        public QsValue Value;



        /// <summary>
        /// Set the tuple value to a value that will be evaluated when value accessed first time
        /// </summary>
        /// <param name="value"></param>
        public void SetLazyValue(Object value)
        {
            _LazyValue = value;
        }

        public bool IsLazyValue
        {
            get
            {
                return _LazyValue != null;
            }
        }

        public QsTupleValue(QsValue value)
        {
            Id = 0;
            Name = string.Empty;
            Value = value;

            _LazyValue = null;
        }

        public QsTupleValue(string name, QsValue value)
        {
            Id = 0;
            Name = name;
            Value = value;

            _LazyValue = null;
        }

        public QsTupleValue(int id, string name, QsValue value)
        {
            Id = id;
            Name = name;
            Value = value;

            _LazyValue = null;
        }

        public QsTupleValue(int id, QsValue value)
        {
            Id = id;
            Name = string.Empty;
            Value = value;

            _LazyValue = null;
        }
    }

    /// <summary>
    /// Quantity System Tuple that stores any type of qsvalue
    /// </summary>
    public class QsFlowingTuple : QsValue
    {
        private Flow ThisFlow = new Flow();

        QsTupleValue[] InitialValues;

        public QsFlowingTuple()
        {
        }


        public QsFlowingTuple(params QsTupleValue[] values)
        {
            // get the maximum id defined in the array 
            // [to be the ground that will be increased whenever we find element without id]

            int sid = values.Max(s => s.Id);  

            foreach (var v in values)
            {
                if (v.Id == 0) sid += 10;

                var st = ThisFlow.Add(v.Name, v.Id == 0 ? sid : v.Id);

                if (v.IsLazyValue)
                    st.Value = v.LazyValue;
                else
                    st.Value = v.Value;
            }

            InitialValues = values;

        }

        public void AddTupleValue(QsValue value)
        {
            int sid = this.Count > 0 ? ThisFlow.FlowSteps.Max(s => s.Id) : 0;
            sid += 10;
            var v = ThisFlow.Add("Step " + sid, sid);
            if (value == null) v.Value = new QsText("n/a");
            else v.Value = value;
        }

        private QsValue ValueToQsValue(object value)
        {
            if (value is QsValue) return (QsValue)value;
            else return Root.NativeToQsConvert(value);
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FlowingTuple (");
            foreach (Step s in ThisFlow)
            {
                if (!string.IsNullOrEmpty(s.Name))
                    sb.Append(s.Name + "!");

                if (s.Value != null) sb.Append(ValueToQsValue(s.Value).ToShortString());
                else sb.Append("nil");
                sb.Append(", ");
            }
            string lll = sb.ToString().TrimEnd(',', ' ') + ")";
            return lll;
        }


        public override string ToShortString()
        {
            return string.Format("QsTuple[{0} Elements]", Count);
        }

        /// <summary>
        /// Returns number of elements in tuple
        /// </summary>
        public int Count
        {
            get
            {
                return ThisFlow.Count();
            }
        }

        public override QsValue Execute(Token expression)
        {
            if (expression.TokenValue.ToUpperInvariant() == "COUNT") return Count.ToScalarValue();

            if (expression.TokenClassType == typeof(ParenthesisCallToken))
            {
                var arg = expression[1].TrimTokens(1, 1).TokenValue;
                if (expression[0].TokenValue.Equals("GetName", StringComparison.OrdinalIgnoreCase))
                {
                    // one parameter only 
                    int ordinal = (int)((QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(arg)).NumericalQuantity.Value;
                    return new QsText(this.ThisFlow.FlowSteps[ordinal].Name);
                }

                if (expression[0].TokenValue.Equals("GetValue", StringComparison.OrdinalIgnoreCase))
                {
                    // one parameter only 
                    int ordinal = (int)((QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(arg)).NumericalQuantity.Value;
                    return (QsValue)this.ThisFlow.FlowSteps[ordinal].Value;
                }

                if (expression[0].TokenValue.Equals("GetId", StringComparison.OrdinalIgnoreCase))
                {
                    int ordinal = (int)((QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(arg)).NumericalQuantity.Value;
                    return this.ThisFlow.FlowSteps[ordinal].Id.ToScalarValue();
                }

                if (expression[0].TokenValue.Equals("GetOrdinal", StringComparison.OrdinalIgnoreCase))
                {
                    // gets the index of element whether it was name or id
                    int id;

                    var param = QsEvaluator.CurrentEvaluator.SilentEvaluate(arg);

                    if (param is QsScalar)
                    {
                        id = (int)((QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(arg)).NumericalQuantity.Value;
                        for (int ix = 0; ix < ThisFlow.FlowSteps.Length; ix++)
                        {
                            if (ThisFlow.FlowSteps[ix].Id == id) return ix.ToScalarValue();
                        }

                        return QsScalar.NegativeOne;
                    }
                    else
                    {
                        var argtext = ((QsText)param).Text;


                        for (int ix = 0; ix < ThisFlow.FlowSteps.Length; ix++)
                        {
                            if (ThisFlow.FlowSteps[ix].Name.Equals(argtext, StringComparison.OrdinalIgnoreCase)) 
                                return ix.ToScalarValue();
                        }

                        return QsScalar.NegativeOne;
                    }
                }
            }

            return base.Execute(expression);
        }

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

        public override QsValue RightShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;

            int itimes = Qs.IntegerFromQsValue((QsScalar)times);

            int c = InitialValues.Count();

            if (itimes > c) itimes = itimes % c;


            QsTupleValue[] NewValues = new QsTupleValue[c];

            int ix = 0;
            for (int i = c - itimes; i < c; i++)
            {
                NewValues[ix] = InitialValues[i];
                ++ix;
                
            }

            for (int i = 0; i < (c - itimes); i++)
            {
                NewValues[ix] = InitialValues[i];
                ++ix;
                
            }

            return new QsFlowingTuple(NewValues);
        }

        public override QsValue LeftShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;

            int itimes = Qs.IntegerFromQsValue((QsScalar)times);

            int c = InitialValues.Count();

            if (itimes > c) itimes = itimes % c;


            QsTupleValue[] NewValues = new QsTupleValue[c];

            int ix = 0;

            for (int i = itimes; i < c; i++)
            {
                NewValues[ix] = InitialValues[i];
                ix++;
            }

            for (int i = 0; i < itimes; i++)
            {
                NewValues[ix] = InitialValues[i];
                ix++;
            }

            return new QsFlowingTuple(NewValues);
            
        }

        public override QsValue GetIndexedItem(QsParameter[] indices)
        {
            var t = indices[0].QsNativeValue as QsText;

            // if the passed indexer is text then return the Flow Object itself pointed to this step name .. for all the functionality of Passive Flow engine
            if (t != null)
            {
                Flow FlowStateMachine = ThisFlow[t.Text];
                return QsObject.CreateNativeObject(FlowStateMachine);
            }
            else
            {
                // or get the integer number from this parameter and get the value associated of this step index

                int ix = (int)((QsScalar)indices[0].QsNativeValue).NumericalQuantity.Value;
                if (ix < 0) ix = ThisFlow.FlowSteps.Length + ix;

                return ValueToQsValue(ThisFlow.FlowSteps[ix].Value);
            }
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return Flow Object hosted in QsObject
        /// </summary>
        /// <param name="stepName"></param>
        /// <returns></returns>
        public QsValue this[string stepName]
        {
            get
            {
                Flow FlowStateMachine = ThisFlow[stepName];
                return QsObject.CreateNativeObject(FlowStateMachine);
            }
        }


        /// <summary>
        /// ! Operator
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override QsValue ExclamationOperator(QsValue value)
        {
            string vt = ((QsText)value).Text;
            return ValueToQsValue(ThisFlow[vt].Value);
        }

        public override QsValue ColonOperator(QsValue value)
        {
            int p = (int)((QsScalar)value).NumericalQuantity.Value;
            return ValueToQsValue(ThisFlow[p].Value);
        }
        
        /// <summary>
        /// Return Tuple from C# enum.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static QsFlowingTuple FromCSharpEnum(Type enumType)
        {
            Contract.Requires(enumType == typeof(Enum));
            var names = Enum.GetNames(enumType);

            QsTupleValue[] fefe = new QsTupleValue[names.Length];
            var values = Enum.GetValues(enumType);


            for (int ix = 0; ix < names.Length; ix++)
            {
                fefe[ix].Name = (string) names[ix];
                fefe[ix].Id = (int)values.GetValue(ix);
                fefe[ix].Value = ((int)values.GetValue(ix)).ToQuantity().ToScalarValue();
            }
            
            return new QsFlowingTuple(fefe);
        }

        public static QsFlowingTuple FromStruct(ValueType value)
        {
            var StructType = value.GetType();

            var props = StructType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance );

            var all = from o in props
                      where o.IsSpecialName == false && o.GetIndexParameters().Length == 0
                      select o;

            QsTupleValue[] fefe = new QsTupleValue[all.Count()];

            int id = 10;   // begin the properties from id == 10  like Basic auto numbering in old days
            for (int ix = 0; ix < all.Count(); ix++)
            {

                var StructProperty = all.ElementAt(ix);

                fefe[ix].Name = StructProperty.Name;
                fefe[ix].Id = id;

                if (StructProperty.PropertyType.IsValueType)
                    fefe[ix].SetLazyValue(StructProperty.GetValue(value, null));
                else
                    fefe[ix].Value = Root.NativeToQsConvert(StructProperty.GetValue(value, null));
                
                id += 10;
            }

            return new QsFlowingTuple(fefe);
        }

        public ValueType FromTuple(Type structType)
        {
            var q = Expression.New(structType);

            var props = structType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            var all = from o in props
                      where o.IsSpecialName == false && o.CanWrite == true && o.GetIndexParameters().Length == 0
                      select o;

            var svt = Activator.CreateInstance(structType);
            // fill the available names from this tuple and 
            foreach (var step in ThisFlow.FlowSteps)
            {
                var prop = all.FirstOrDefault(p => p.Name.Equals(step.Name, StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                {
                    // property exist

                    prop.SetValue(svt, Root.QsToNativeConvert(prop.PropertyType, step.Value), null);

                }
            }

            return (ValueType)svt;
        }
    }
}
