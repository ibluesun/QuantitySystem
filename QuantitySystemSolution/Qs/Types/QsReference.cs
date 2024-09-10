using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Qs.Runtime;

namespace Qs.Types
{
    public class QsReference : QsValue
    {
        readonly string  _ReferencedExpressionText;

        readonly Expression _ReferencedExpression;

        public QsReference(string expression, Expression referencedExpression)
        {
            _ReferencedExpressionText= expression;
            _ReferencedExpression = referencedExpression;
        }

        public string ReferencedExpressionText => _ReferencedExpressionText;


        internal object Execute()
        {

            // Construct Lambda function which return one object.
            Expression<Func<object>> cq = Expression.Lambda<Func<object>>(this._ReferencedExpression);

            // compile the function
            Func<object> aqf = cq.Compile();

            // execute the function
            object result = aqf();

            // return the result
            return result;
        }


        public QsValue ContentValue
        {
            get
            {
                return (QsValue)Execute();
            }

            /*
            get
            {
                if (_ReferencedExpressionText.Contains(":"))
                {
                    int lc = _ReferencedExpressionText.LastIndexOf(':');

                    var ns = _ReferencedExpressionText.Substring(0, lc);
                    var nm = _ReferencedExpressionText.Substring(lc + 1);

                    var cns = QsNamespace.GetNamespace(QsEvaluator.CurrentEvaluator.Scope, ns);
                    return (QsValue)cns.GetValue(nm);
                }
                else
                {
                    return (QsValue)QsEvaluator.CurrentEvaluator.GetVariable(_ReferencedExpressionText);
                }
            }
            set
            {
                if (_ReferencedExpressionText.Contains(":"))
                {
                    int lc = _ReferencedExpressionText.LastIndexOf(':');

                    var ns = _ReferencedExpressionText.Substring(0, lc);
                    var nm = _ReferencedExpressionText.Substring(lc + 1);

                    var cns = QsNamespace.GetNamespace(QsEvaluator.CurrentEvaluator.Scope, ns);
                    cns.SetValue(nm, value);
                }
                else
                {
                    QsEvaluator.CurrentEvaluator.StoreHeapVariable(_ReferencedExpressionText, value);
                }
            }
            */
        }

        public override string ToShortString()
        {
            return $"*({ReferencedExpressionText}): {ContentValue.ToShortString()}";
        }

        public override string ToString()
        {
            return $"*({ReferencedExpressionText}): {ContentValue}";
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
            string operation = expression.TokenValue;

            if (operation.Equals("Expression", StringComparison.OrdinalIgnoreCase))
                return new QsText(_ReferencedExpression.ToString());

            throw new QsException("Not implemented or Unknow method for the QsReference type");

        }

    }
}
