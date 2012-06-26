using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;

namespace Qs.Types
{
    public class QsReference : QsValue
    {
        readonly string  _varname;
        public QsReference(string varname)
        {
            _varname= varname;
        }

        public string VariableName
        {
            get
            {
                return _varname;
            }
        }

        public QsValue ContentValue
        {
            get
            {
                if (_varname.Contains(":"))
                {
                    int lc = _varname.LastIndexOf(':');

                    var ns = _varname.Substring(0, lc);
                    var nm = _varname.Substring(lc + 1);

                    var cns = QsNamespace.GetNamespace(QsEvaluator.CurrentEvaluator.Scope, ns);
                    return (QsValue)cns.GetValue(nm);
                }
                else
                    return (QsValue)QsEvaluator.CurrentEvaluator.GetVariable(_varname);
            }
            set
            {
                if (_varname.Contains(":"))
                {
                    int lc = _varname.LastIndexOf(':');

                    var ns = _varname.Substring(0, lc);
                    var nm = _varname.Substring(lc + 1);

                    var cns = QsNamespace.GetNamespace(QsEvaluator.CurrentEvaluator.Scope, ns);
                    cns.SetValue(nm, value);
                }
                else
                {
                    QsEvaluator.CurrentEvaluator.SetVariable(_varname, value);
                }
            }
        }

        public override string ToShortString()
        {
            return VariableName + ": " + ContentValue.ToShortString();
        }

        public override string ToString()
        {
            return VariableName + ": " + ContentValue.ToString();
        }


        public override QsValue Identity
        {
            get { return ContentValue.Identity; }
        }

        public override QsValue AddOperation(QsValue value)
        {
            return ContentValue.AddOperation(value);
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            return ContentValue.SubtractOperation(value);
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            return ContentValue.MultiplyOperation(value);
        }

        public override QsValue DivideOperation(QsValue value)
        {
            return ContentValue.DivideOperation(value);
        }

        public override QsValue PowerOperation(QsValue value)
        {
            return ContentValue.PowerOperation(value);
        }

        public override QsValue ModuloOperation(QsValue value)
        {
            return ContentValue.ModuloOperation(value);
        }

        public override bool LessThan(QsValue value)
        {
            return ContentValue.LessThan(value);
        }

        public override bool GreaterThan(QsValue value)
        {
            return ContentValue.GreaterThan(value);
        }

        public override bool LessThanOrEqual(QsValue value)
        {
            return ContentValue.LessThanOrEqual(value);
        }

        public override bool GreaterThanOrEqual(QsValue value)
        {
            return ContentValue.GreaterThanOrEqual(value);
        }

        public override bool Equality(QsValue value)
        {
            return ContentValue.Equality(value);
        }

        public override bool Inequality(QsValue value)
        {
            return ContentValue.Inequality(value);
        }

        public override QsValue DotProductOperation(QsValue value)
        {
            return ContentValue.DotProductOperation(value);
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            return ContentValue.CrossProductOperation(value);
        }

        public override QsValue TensorProductOperation(QsValue value)
        {
            return ContentValue.TensorProductOperation(value);
        }

        public override QsValue NormOperation()
        {
            return ContentValue.NormOperation();
        }

        public override QsValue AbsOperation()
        {
            return ContentValue.AbsOperation();
        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            return ContentValue.RightShiftOperation(times);
        }

        public override QsValue LeftShiftOperation(QsValue times)
        {
            return ContentValue.LeftShiftOperation(times);
        }

        public override QsValue GetIndexedItem(QsParameter[] indices)
        {
            return ContentValue.GetIndexedItem(indices);
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            ContentValue.SetIndexedItem(indices, value);
        }

        /// <summary>
        /// when executing object->method or property
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override QsValue Execute(ParticleLexer.Token expression)
        {
            return ContentValue.Execute(expression);
        }

    }
}
