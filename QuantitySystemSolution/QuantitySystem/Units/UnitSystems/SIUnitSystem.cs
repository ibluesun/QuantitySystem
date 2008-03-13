using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Quantities.BaseQuantities;

using QuantitySystem.Units;
using QuantitySystem.Units.SIUnits;
using QuantitySystem.Units.SIUnits.BaseUnits;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.UnitSystems
{
    public static class SIUnitSystem
    {
        #region Helper Methods
        private static AnyQuantity MakeQuantity(ISIUnit unit, SIPrefix siPrefix, double value)
        {
            
            //assign its prefix
            unit.Prefix = siPrefix;

            //create the corresponding quantity
            AnyQuantity qty = unit.CreateThisUnitQuantity();
            
            //assign the unit to the created quantity
            qty.Unit = unit;

            //assign the value to the quantity
            qty.Value = value;

            return qty;

        }
        #endregion

        #region Positive

        public static AnyQuantity Yotta<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Yotta, value);
        }

        public static AnyQuantity Zetta<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Zetta, value);
        }

        public static AnyQuantity Exa<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Exa, value);
        }

        public static AnyQuantity Peta<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Peta, value);
        }

        public static AnyQuantity Tera<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Tera, value);
        }

        public static AnyQuantity Giga<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Giga, value);
        }

        public static AnyQuantity Mega<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Mega, value);
        }

        public static AnyQuantity Kilo<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Kilo, value);
        }

        public static AnyQuantity Hecto<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Hecto, value);
        }

        public static AnyQuantity Deka<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Deka, value);
        }

        #endregion

        public static AnyQuantity Default<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Default, value);
        }

        #region Negative

        public static AnyQuantity Deci<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Deci, value);
        }

        public static AnyQuantity Centi<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Centi, value);
        }

        public static AnyQuantity Milli<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Milli, value);
        }

        public static AnyQuantity Micro<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Micro, value);
        }

        public static AnyQuantity Nano<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Nano, value);
        }

        public static AnyQuantity Pico<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Pico, value);
        }

        public static AnyQuantity Femto<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Femto, value);
        }

        public static AnyQuantity Atto<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Atto, value);
        }

        public static AnyQuantity Zepto<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Zepto, value);
        }

        public static AnyQuantity Yocto<TUnit>(double value) where TUnit : ISIUnit, new()
        {
            return MakeQuantity(new TUnit(), SIPrefix.Yocto, value);
        }


        #endregion


        #region Quantities of derived units

        /// <summary>
        /// search for the unit of the quantity
        /// return null if unit wasn't found.
        /// </summary>
        /// <param name="quantityType"></param>
        /// <returns></returns>
        private static ISIUnit UnitOf(Type quantityType)
        {
            #region base Quanities
            if (quantityType == typeof(Mass)) return new Gram();
            if (quantityType == typeof(Length)) return new Metre();
            if (quantityType == typeof(Time)) return new Second();
            if (quantityType == typeof(Temperature)) return new Kelvin();
            if (quantityType == typeof(LuminousIntensity)) return new Candela();
            if (quantityType == typeof(AmountOfSubstance)) return new Mole();
            if (quantityType == typeof(ElectricalCurrent)) return new Ampere();
            #endregion

            #region inherited Quantities
            if (quantityType == typeof(Energy)) return new Joule();
            if (quantityType == typeof(Force)) return new Newton();
            if (quantityType == typeof(Pressure)) return new Pascal();
            if (quantityType == typeof(Power)) return new Watt();
            if (quantityType == typeof(Angle)) return new Radian();
            #endregion


            return null;

        }

        /// <summary>
        /// Return the SI Unit of strongly typed quantity.
        /// </summary>
        /// <typeparam name="TQuantity"></typeparam>
        /// <returns></returns>
        public static IUnit UnitOf<TQuantity>() where TQuantity : AnyQuantity, new()
        {

            //try direct mapping
            IUnit unit =  UnitOf(typeof(TQuantity));


            if (unit != null)
                return unit;
            else
            {
                //if failed you should generate it
                //try first the child quantities in the quantity instance if its base is dervied quantity
                // and DerivedQuantity itself.

                QuantityDimension dimension = QuantityDimension.DimensionFrom(typeof(TQuantity));
                
                //AnyQuantity quantity = QuantityDimension.QuantityFrom(dimension);

                //if(quantity.GetType().BaseType == typeof(DerivedQuantity))
                //{
                    
                //    return new DerivedSIUnit(((DerivedQuantity)quantity).GetInternalQuantities());
                //}
                //else
                {
                    return new DerivedSIUnit(dimension);
                }
            }

        }

        /// <summary>
        /// returns SI unit of quantity instance.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static ISIUnit UnitOf(AnyQuantity quantity)
        {

            //try direct mapping first.

            ISIUnit unit = UnitOf(quantity.GetType());
            if (unit != null)
            {
                return unit;
            }
            else
            {
                //if (quantity.GetType().BaseType == typeof(DerivedQuantity))
                //{
                //    return new DerivedSIUnit(((DerivedQuantity)quantity).GetInternalQuantities());
                //}
                //else
                {
                    return new DerivedSIUnit(quantity.Dimension);
                }
            }
        }

        public static AnyQuantity GetUnitizedQuantityOf<TQuantity>(double value) where TQuantity : AnyQuantity, new()
        {
            IUnit unit = UnitOf<TQuantity>();
            
            
            AnyQuantity aq = unit.CreateThisUnitQuantity();
            
            
            aq.Value = value;


            return aq;
        }
        


        #endregion
    }
}
