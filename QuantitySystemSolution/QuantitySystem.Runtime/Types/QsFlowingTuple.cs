using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassiveFlow;
using System.Diagnostics.Contracts;
using ParticleLexer;

namespace Qs.Types
{

    public struct QsTupleValue
    {
        public int Id;
        public string Name;
        public QsValue Value;

        public QsTupleValue(QsValue value)
        {
            Id = 0;
            Name = string.Empty;
            Value = value;
        }

        public QsTupleValue(string name, QsValue value)
        {
            Id = 0;
            Name = name;
            Value = value;
        }

        public QsTupleValue(int id, string name, QsValue value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public QsTupleValue(int id, QsValue value)
        {
            Id = id;
            Name = string.Empty;
            Value = value;
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FlowingTuple (");
            foreach (Step s in ThisFlow)
            {
                sb.Append(((QsValue)s.Value).ToShortString());
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
            if (expression.TokenValue.ToUpper() == "COUNT") return ThisFlow.Count().ToScalarValue();
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
            if (t!=null)
            {
                Flow FlowStateMachine = ThisFlow[t.Text];
                return QsObject.CreateNativeObject(FlowStateMachine);
            }
            else
            {
                int ix = (int)((QsScalar)indices[0].QsNativeValue).NumericalQuantity.Value;
                return (QsValue)ThisFlow.FlowSteps[ix].Value;
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
            return (QsValue)ThisFlow[vt].Value;
        }

        public override QsValue ColonOperator(QsValue value)
        {
            int p = (int)((QsScalar)value).NumericalQuantity.Value;
            return (QsValue)ThisFlow[p].Value;
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
    }
}
