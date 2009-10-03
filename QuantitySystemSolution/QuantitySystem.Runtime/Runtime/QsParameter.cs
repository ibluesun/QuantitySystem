using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Qs.RuntimeTypes;

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


        public string Namespace
        {
            get
            {
                string[] rv = RawValue.Split(':');
                if (rv.Length == 2)
                    return rv[0];
                else
                    return "";

            }
        }

        public string NamespaceValue
        {
            get
            {
                string[] rv = RawValue.Split(':');
                if (rv.Length == 2)
                    return rv[1];
                else
                    return rv[0];
            }
        }

        public static QsParameter MakeParameter(object value, string rawValue)
        {
            QsParameter qp = new QsParameter();

            qp.Value = value;

            qp.RawValue = rawValue;

            return qp;
        }


        public QsValue Quantity
        {
            get
            {
                return Value as QsValue;
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

        /// <summary>
        /// if we consider the <see cref="RawValue"/> as a function name.
        /// then this function will get the actual function name which include parameters count.
        /// This function is only used in making expressions.
        /// </summary>
        /// <param name="paramCount"></param>
        /// <returns></returns>
        public string GetTrueFunctionName(int paramCount)
        {
            return QsFunction.FormFunctionScopeName(RawValue, paramCount);
        }

    }
}
