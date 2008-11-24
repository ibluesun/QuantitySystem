using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Quantities.BaseQuantities;

using QuantitySystem.Units;


using QuantitySystem.Quantities.DimensionlessQuantities;

using QuantitySystem.Units.SI;

namespace QuantitySystem.Units
{
    public static class SISystem
    {
        #region Helper Methods
        private static AnyQuantity<double> MakeQuantity(SIUnit unit, SIPrefix siPrefix, double value)
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

        public static AnyQuantity<double> Yotta<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Yotta, value);
        }

        public static AnyQuantity<double> Zetta<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Zetta, value);
        }

        public static AnyQuantity<double> Exa<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Exa, value);
        }

        public static AnyQuantity<double> Peta<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Peta, value);
        }

        public static AnyQuantity<double> Tera<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Tera, value);
        }

        public static AnyQuantity<double> Giga<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Giga, value);
        }

        public static AnyQuantity<double> Mega<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Mega, value);
        }

        public static AnyQuantity<double> Kilo<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Kilo, value);
        }

        public static AnyQuantity<double> Hecto<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Hecto, value);
        }

        public static AnyQuantity<double> Deka<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Deka, value);
        }

        #endregion

        public static AnyQuantity<double> None<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.None, value);
        }

        #region Negative

        public static AnyQuantity<double> Deci<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Deci, value);
        }

        public static AnyQuantity<double> Centi<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Centi, value);
        }

        public static AnyQuantity<double> Milli<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Milli, value);
        }

        public static AnyQuantity<double> Micro<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Micro, value);
        }

        public static AnyQuantity<double> Nano<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Nano, value);
        }

        public static AnyQuantity<double> Pico<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Pico, value);
        }

        public static AnyQuantity<double> Femto<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Femto, value);
        }

        public static AnyQuantity<double> Atto<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Atto, value);
        }

        public static AnyQuantity<double> Zepto<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Zepto, value);
        }

        public static AnyQuantity<double> Yocto<TUnit>(double value) where TUnit : SIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Yocto, value);
        }


        #endregion


        #region Quantities of derived units


        /// <summary>
        /// Return the SI Unit of strongly typed quantity.
        /// </summary>
        /// <typeparam name="TQuantity"></typeparam>
        /// <returns></returns>
        public static SIUnit UnitOf<TQuantity>() where TQuantity : BaseQuantity, new()
        {

            //try direct mapping
            SIUnit unit = Activator.CreateInstance(Unit.GetSIUnitTypeOf(typeof(TQuantity))) as SIUnit;


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

        ///// <summary>
        ///// returns SI unit of quantity instance.
        ///// </summary>
        ///// <param name="quantity"></param>
        ///// <returns></returns>
        //public static SIUnit UnitOf<T>(AnyQuantity<T> quantity)
        //{

        //    //try direct mapping first.

        //    SIUnit unit = UnitOf(quantity.GetType());
        //    if (unit != null)
        //    {
        //        return unit;
        //    }
        //    else
        //    {
        //        //if (quantity.GetType().BaseType == typeof(DerivedQuantity))
        //        //{
        //        //    return new DerivedSIUnit(((DerivedQuantity)quantity).GetInternalQuantities());
        //        //}
        //        //else
        //        {
        //            return new DerivedSIUnit(quantity.Dimension);
        //        }
        //    }
        //}

        public static AnyQuantity<double> GetUnitizedQuantityOf<TQuantity>(double value) where TQuantity : BaseQuantity, new()
        {
            SIUnit unit = UnitOf<TQuantity>();


            AnyQuantity<double> aq = unit.GetThisUnitQuantity<double>();
            
            
            aq.Value = value;


            return aq;
        }
        


        #endregion
    }
}
