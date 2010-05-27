using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units
{
    public abstract partial class MetricUnit : Unit
    {
        #region Helper Methods
        private static AnyQuantity<double> MakeQuantity(MetricUnit unit, MetricPrefix siPrefix, double value)
        {

            //assign its prefix
            unit.UnitPrefix = siPrefix;

            //create the corresponding quantity
            AnyQuantity<double> qty = unit.GetThisUnitQuantity<double>();

            //assign the unit to the created quantity
            qty.Unit = unit;

            //assign the value to the quantity
            qty.Value = value;

            return qty;

        }
        #endregion

        #region Positive

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Yotta"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Yotta<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Yotta, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Zetta"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Zetta<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Zetta, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Exa"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Exa<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Exa, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Peta"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Peta<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Peta, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tera"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Tera<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Tera, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Giga<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Giga, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Mega<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Mega, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Kilo<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Kilo, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hecto"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Hecto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Hecto, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Deka"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Deka<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Deka, value);
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> None<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.None, value);
        }

        #region Negative

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Deci"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Deci<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Deci, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Centi"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Centi<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Centi, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Milli"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Milli<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Milli, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Micro<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Micro, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nano"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Nano<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Nano, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Pico<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Pico, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Femto"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Femto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Femto, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Atto"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Atto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Atto, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Zepto"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Zepto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Zepto, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Yocto"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> Yocto<TUnit>(double value) where TUnit : MetricUnit, new()
        {
            return MakeQuantity(new TUnit(), MetricPrefix.Yocto, value);
        }


        #endregion


        #region Quantities of derived units


        /// <summary>
        /// Return the SI Unit of strongly typed quantity.
        /// </summary>
        /// <typeparam name="TQuantity"></typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static MetricUnit UnitOf<TQuantity>() where TQuantity : BaseQuantity, new()
        {

            //try direct mapping
            MetricUnit unit = Activator.CreateInstance(Unit.GetDefaultSIUnitTypeOf(typeof(TQuantity))) as MetricUnit;


            if (unit != null)
            {
                return unit;
            }
            else
            {
                //if failed you should generate it
                //try first the child quantities in the quantity instance if its base is dervied quantity
                // and DerivedQuantity itself.

                QuantityDimension dimension = QuantityDimension.DimensionFrom(typeof(TQuantity));

                //return a derived unit.
                //return new DerivedSIUnit(dimension);
                throw new NotImplementedException();
            }

        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity<double> GetUnitizedQuantityOf<TQuantity>(double value) where TQuantity : BaseQuantity, new()
        {
            MetricUnit unit = UnitOf<TQuantity>();


            AnyQuantity<double> aq = unit.GetThisUnitQuantity<double>();


            aq.Value = value;


            return aq;
        }



        #endregion

    }
}
