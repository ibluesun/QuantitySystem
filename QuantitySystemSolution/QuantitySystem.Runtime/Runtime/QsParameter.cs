using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace Qs.Runtime
{

    /// <summary>
    /// Because I want to support passing function as a delegate to the function.
    /// </summary>
    public class QsParameter
    {
        public object Value { get; private set; }

        /// <summary>
        /// Original value before evaluation.
        /// </summary>
        public string RawValue { get; private set; }

        public static QsParameter MakeParameter(object value, string rawValue)
        {
            QsParameter qp = new QsParameter();

            qp.Value = value;

            qp.RawValue = rawValue;

            return qp;
        }


        public AnyQuantity<double> Quantity
        {
            get
            {
                return Value as AnyQuantity<double>;
            }
        }


        public bool IsKnown
        {
            get
            {
                return !(Value is string);
            }
        }

        /// <summary>
        /// Unknown is something that was sent to the function as a parameter. with text I don't know it
        /// 
        /// </summary>
        public string Unknown
        {
            get
            {
                return Value as string;
            }
        }


        public string GetTrueFunctionName(int paramCount)
        {
            return QsFunction.FormFunctionScopeName(RawValue, paramCount);
        }

    }
}
