using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using QuantitySystem;

namespace QsRoot
{
    public static class Quantity
    {
        /// <summary>
        /// Dimension of scalar quantities
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QsValue Dimension(QsParameter value)
        {
            
            QsScalar s = value.QsNativeValue as QsScalar;
            if (s != null)
            {
                return new QsText( s.Unit.UnitDimension.ToString());
            }


            return new QsText("Works on scalar quantities");
        }

        public static QsValue Type(QsParameter value)
        {
            QsScalar s = value.QsNativeValue as QsScalar;
            if (s != null)
            {
                string qt = s.Unit.QuantityType.Name;
                return new QsText(qt.Substring(0, qt.Length - 2));
            }


            return new QsText("Works on scalar quantities");
        }


        public static QsValue FromDimension(QsParameter dimension)
        {
            string ss = dimension.ParameterRawText;
            if (dimension.QsNativeValue is QsText) ss = ((QsText)dimension.QsNativeValue).Text;
            var q = QuantityDimension.Parse(ss);
            
            string qt = QuantityDimension.GetQuantityTypeFrom(q).Name;
            return new QsText(qt.Substring(0, qt.Length - 2));
        }
    }
}
