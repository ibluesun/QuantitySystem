using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using System.Globalization;

namespace QsRoot
{
    public static class Quantity
    {
        /// <summary>
        /// Returns the dimension of value quantity.
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

        /// <summary>
        /// Returns the name of the quantity associated with this value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QsValue FromValue(QsParameter value)
        {
            QsScalar s = value.QsNativeValue as QsScalar;
            if (s != null)
            {
                string qt = s.Unit.QuantityType.Name;
                return new QsText(qt.Substring(0, qt.Length - 2));
            }


            return new QsText("Works on scalar quantities");
        }

        /// <summary>
        /// Resturns the name of the quantity associated with this dimension
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static QsValue FromDimension(QsParameter dimension)
        {
            string ss = dimension.ParameterRawText;
            if (dimension.QsNativeValue is QsText) ss = ((QsText)dimension.QsNativeValue).Text;
            var q = QuantityDimension.Parse(ss);
            
            string qt = QuantityDimension.GetQuantityTypeFrom(q).Name;
            return new QsText(qt.Substring(0, qt.Length - 2));
        }

        /// <summary>
        /// Returns a value from the dimension of this quantity.
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QsValue FromDimension(QsParameter dimension, QsParameter value)
        {
            string ss = dimension.ParameterRawText;
            if (dimension.QsNativeValue is QsText) ss = ((QsText)dimension.QsNativeValue).Text;
            var q = QuantityDimension.Parse(ss);

            var unit = QuantitySystem.Units.Unit.DiscoverUnit(q);
            var qval = unit.GetThisUnitQuantity<double>(double.Parse(value.ParameterRawText,  CultureInfo.InvariantCulture));

            var qs = new QsScalar(ScalarTypes.NumericalQuantity) { NumericalQuantity = qval };

            return qs;
        }


        public static QsValue FromName(QsParameter name, QsParameter value)
        {
            string ss = name.ParameterRawText;
            if (name.QsNativeValue is QsText) ss = ((QsText)name.QsNativeValue).Text;

            var qval = AnyQuantity<double>.Parse(ss);
            qval.Unit = Unit.DiscoverUnit(qval);
            qval.Value = double.Parse(value.ParameterRawText, CultureInfo.InvariantCulture);

            var qs = new QsScalar(ScalarTypes.NumericalQuantity) { NumericalQuantity = qval };

            return qs;
        }

        public static QsValue Parse(string value)
        {
            return QsScalar.ParseScalar(value);
        }

        public static QsValue InvertedDimension(QsParameter value)
        {

            QsScalar s = value.QsNativeValue as QsScalar;
            if (s != null)
            {
                var InvertedDimension = s.Unit.UnitDimension.Invert();

                return new QsText(InvertedDimension.ToString());
            }


            return new QsText("Works on scalar quantities");

        }

        public static QsValue Name(QsParameter value)
        {

            QsScalar s = value.QsNativeValue as QsScalar;
            if (s != null)
            {
                return new QsText(s.Unit.QuantityType.Name.TrimEnd('`','1').ToString());
            }

            return new QsText("Works on scalar quantities");
        }


        public static QsValue InvertedQuantityName(QsParameter value)
        {
            QsScalar s = value.QsNativeValue as QsScalar;
            if (s != null)
            {
                var InvertedDimension = s.Unit.UnitDimension.Invert();

                var qp = QsParameter.MakeParameter(null, InvertedDimension.ToString());
                return Quantity.FromDimension(qp);
            }


            return new QsText("Works on scalar quantities");

        }
    }
}
