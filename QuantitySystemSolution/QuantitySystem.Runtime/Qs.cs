using System;
using System.Diagnostics;
using Qs.Types;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using SymbolicAlgebra;
using Qs.Numerics;
using System.Reflection;

namespace Qs
{
    public static class Qs
    {
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
            return IntegerFromQuantity(((QsScalar)val).NumericalQuantity);
        }

        /// <summary>
        /// Quantitize the integer value into DimensionlessQuantity
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this int i)
        {
            Unit un = Unit.DiscoverUnit(QuantityDimension.Dimensionless);
            return un.GetThisUnitQuantity<double>((double)i);
        }

        /// <summary>
        /// Quantitize the floating value into DimensionlessQuantity
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

        /// <summary>
        /// Quantitize the complex number into Complex Quantity
        /// </summary>
        /// <param name="complexValue"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static AnyQuantity<Complex> ToQuantity(this Complex complexValue, string unit="1")
        {
            Unit un = Unit.Parse(unit);

            return un.GetThisUnitQuantity<Complex>(complexValue);
        }


        /// <summary>
        /// Wrap Complex Quantity into Scalar.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static QsScalar ToScalar(this AnyQuantity<Complex> qty)
        {
            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = qty };
        }


        /// <summary>
        /// Quantitize the quaternion number into quantity
        /// </summary>
        /// <param name="quaternionValue"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static AnyQuantity<Quaternion> ToQuantity(this Quaternion quaternionValue, string unit = "1")
        {
            Unit un = Unit.Parse(unit);

            return un.GetThisUnitQuantity<Quaternion>(quaternionValue);
        }

        /// <summary>
        /// Return a scalar object from quaternion quantity.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static QsScalar ToScalar(this AnyQuantity<Quaternion> qty)
        {
            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = qty };
        }

        /// <summary>
        /// Wrap double quantity storage into qs scalar object.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static QsScalar ToScalar(this AnyQuantity<double> qty)
        {
            return new QsScalar { NumericalQuantity = qty };
        }

        /// <summary>
        /// Returns Quantity of the symbolic variable based on the unit
        /// </summary>
        /// <param name="sv"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static AnyQuantity<SymbolicVariable> ToQuantity(this SymbolicVariable sv, string unit="1")
        {
            Unit sunit = Unit.Parse(unit);
            AnyQuantity<SymbolicVariable> SymbolicQuantity = sunit.GetThisUnitQuantity<SymbolicVariable>(sv);

            return SymbolicQuantity;
        }

        /// <summary>
        /// Returns a quantity from function
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static AnyQuantity<QsFunction> ToQuantity(this QsFunction fn, string unit = "1")
        {
            Unit sunit = Unit.Parse(unit);

            AnyQuantity<QsFunction> FunctionQuantity = sunit.GetThisUnitQuantity<QsFunction>(fn);

            return FunctionQuantity;
        }

        /// <summary>
        /// Wrap AnyQuantity of Symbolic Variable object into qs scalar object.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static QsScalar ToScalar(this AnyQuantity<SymbolicVariable> qty)
        {
            QsScalar symscalar = new QsScalar(ScalarTypes.SymbolicQuantity)
            {
                SymbolicQuantity = qty
            };
            return symscalar;
        }

        /// <summary>
        /// Returns a scalar object from function quantity.
        /// </summary>
        /// <param name="functionQuantity"></param>
        /// <returns></returns>
        public static QsScalar ToScalar(this AnyQuantity<QsFunction> functionQuantity)
        {
            QsScalar fnScalar = new QsScalar(ScalarTypes.FunctionQuantity)
            {
                FunctionQuantity = functionQuantity
            };
            return fnScalar;
        }


        public static QsValue ToScalarValue(this AnyQuantity<double> qty)
        {
            return new QsScalar { NumericalQuantity = qty };
        }

        /// <summary>
        /// Returns Complex quantity from Double Quantity
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static AnyQuantity<Complex> ToComplex(this AnyQuantity<double> qty)
        {
            AnyQuantity<Complex> converted = qty.Unit.GetThisUnitQuantity<Complex>(qty.Value);
            return converted;
        }


        /// <summary>
        /// Returns Quaternion quantity from Double Quantity.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static AnyQuantity<Quaternion> ToQuaternion(this AnyQuantity<double> qty)
        {
            AnyQuantity<Quaternion> converted = qty.Unit.GetThisUnitQuantity<Quaternion>(qty.Value);
            return converted;
        }

        public static AnyQuantity<Rational> ToRational(this AnyQuantity<double> qty)
        {

            AnyQuantity<Rational> converted = qty.Unit.GetThisUnitQuantity<Rational>(new Rational((float)qty.Value, 1));
            return converted;
        }


        /// <summary>
        /// Returns Quaternion Quantity from Complex Quantity.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static AnyQuantity<Quaternion> ToQuaternion(this AnyQuantity<Complex> qty)
        {
            AnyQuantity<Quaternion> converted = qty.Unit.GetThisUnitQuantity<Quaternion>(qty.Value);
            return converted;
        }



        /// <summary>
        /// Quantitize the complex number into Complex Quantity
        /// </summary>
        /// <param name="rationalValue"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static AnyQuantity<Rational> ToQuantity(this Rational rationalValue, string unit = "1")
        {
            Unit un = Unit.Parse(unit);

            return un.GetThisUnitQuantity<Rational>(rationalValue);
        }


        /// <summary>
        /// Wrap Complex Quantity into Scalar.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static QsScalar ToScalar(this AnyQuantity<Rational> qty)
        {
            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = qty };
        }



        
        public static QsValue ToScalarValue(this string s)
        {
            return QsValue.ParseScalar(s);
        }

        public static QsValue ToScalarValue(this int i)
        {
            return new QsScalar { NumericalQuantity = ToQuantity(i) };
        }


        /// <summary>
        /// Parse the string into quantity and wrap it into QsScalar object.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static QsScalar ToScalar(this string s)
        {
            return new QsScalar { NumericalQuantity = s.ToQuantity() };
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
