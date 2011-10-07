using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PassiveFlow;
using System.Diagnostics.Contracts;

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
    }

    /// <summary>
    /// Quantity System Tuple that stores any type of qsvalue
    /// </summary>
    public class QsFlowingTuple : QsValue
    {
        private Flow ThisFlow = new Flow();

        QsTupleValue[] InitialValues;

        public QsFlowingTuple(params QsTupleValue[] values)
        {
            int sid = 0;
            foreach (var v in values)
            {
                if (v.Id == 0) sid += 10;

                var st = ThisFlow.Add(v.Name, v.Id == 0 ? sid : v.Id);
                st.Value = v.Value;
            }

            InitialValues = values;

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

        public override QsValue GetIndexedItem(int[] indices)
        {
            return (QsValue)ThisFlow.FlowSteps[indices[0]].Value;
        }


        /// <summary>
        /// ! Operator
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override QsValue ExclamationOperator(string key)
        {
            return (QsValue)ThisFlow[key].Value;
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
