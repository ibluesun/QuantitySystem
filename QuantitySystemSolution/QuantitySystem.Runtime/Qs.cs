using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using QuantitySystem;
using Qs.RuntimeTypes;
using System.Diagnostics;

namespace Qs
{
    public static class Qs
    {
        public static ScriptRuntime CreateRuntime()
        {
            string[] QsNames = { "QuantitySystem", "Qs" };
            string[] QsExtensions = { ".Qs" };
            string QsType = typeof(Runtime.QsContext).FullName + ", " + typeof(Runtime.QsContext).Assembly.FullName;

            LanguageSetup QsSetup = new LanguageSetup(QsType, "Quantity System Calculator", QsNames, QsExtensions);

            ScriptRuntimeSetup srs = new ScriptRuntimeSetup();

            srs.LanguageSetups.Add(QsSetup);

            ScriptRuntime sr = new ScriptRuntime(srs);

            return sr;


        }

        #region Extensions and helper methods

        /// <summary>
        /// Force to return integer value from <see cref="AnyQuantity<double>"/>
        /// used mainly for calculated indexes for sequences.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int IntegerFromQuantity(AnyQuantity<double> val)
        {
            Debug.Assert(val is QuantitySystem.Quantities.DimensionlessQuantities.DimensionlessQuantity<double>);

            return (int)val.Value;
        }

        public static int IntegerFromQsValue(QsValue val)
        {
            return IntegerFromQuantity(((QsScalar)val).Quantity);
        }

        /// <summary>
        /// Quantitize the double value into DimensionlessQuantity
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this int i)
        {
            Unit un = Unit.DiscoverUnit(QuantityDimension.Dimensionless);
            return un.GetThisUnitQuantity<double>((double)i);
        }

        /// <summary>
        /// Quantitize the double value into DimensionlessQuantity
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this float d)
        {
            Unit un = Unit.DiscoverUnit(QuantityDimension.Dimensionless);
            return un.GetThisUnitQuantity<double>((double)d);
        }

        /// <summary>
        /// Quantitize the double value into DimensionlessQuantity
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this double d)
        {
            Unit un = Unit.DiscoverUnit(QuantityDimension.Dimensionless);
            return un.GetThisUnitQuantity<double>(d);
        }

        /// <summary>
        /// Quantitize the double value into AnyQuantity with the help of selected unit.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this double d, string unit)
        {
            Unit un = Unit.Parse(unit);

            return un.GetThisUnitQuantity<double>(d);
        }

        public static AnyQuantity<double> ToQuantity(this int d, string unit)
        {
            Unit un = Unit.Parse(unit);

            return un.GetThisUnitQuantity<double>(d);
        }

        /// <summary>
        /// Quantitize the string into quantity based on its input of units.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this string s)
        {
            return Unit.ParseQuantity(s);
        }
        public static QsScalar ToScalar(this AnyQuantity<double> qty)
        {
            return new QsScalar { Quantity = qty };
        }

        public static QsValue ToScalarValue(this AnyQuantity<double> qty)
        {
            return new QsScalar { Quantity = qty };
        }

        public static QsValue ToScalarValue(this string s)
        {
            return QsValue.ParseScalar(s);
        }

        public static QsValue ToScalarValue(this int i)
        {
            return new QsScalar { Quantity = ToQuantity(i) };
        }


        public static QsScalar ToScalar(this string s)
        {
            return new QsScalar { Quantity = s.ToQuantity() };
        }


        /// <summary>
        /// The function try to convert the passed array into an array of scalars.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static QsScalar[] ToScalars<T>(this T[] data) 
        {
            QsScalar[] ss = new QsScalar[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                double d = Convert.ToDouble(data[i]);

                ss[i] = d.ToQuantity().ToScalar();
            }
            return ss;

        }

        public static QsVector ToQsVector<T>(this T[] data)
        {
            var scs = ToScalars(data);
            QsVector vector = new QsVector(scs);
            return vector;
        }

        
        #endregion
    }
}
