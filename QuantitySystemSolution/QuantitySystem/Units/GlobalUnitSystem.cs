using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units
{
    /// <summary>
    /// for shared units that exist in any unit system.
    /// </summary>
    public static class GlobalUnitSystem
    {
        public static AnyQuantity Default<TUnit>(double value) where TUnit : IUnit, new()
        {
            TUnit unit = new TUnit();

            //create the corresponding quantity
            AnyQuantity qty = unit.CreateThisUnitQuantity();

            //assign the unit to the created quantity
            qty.Unit = unit;

            //assign the value to the quantity
            qty.Value = value;

            return qty;

        }


        /// <summary>
        /// search for the unit of the quantity
        /// return null if unit wasn't found.
        /// </summary>
        /// <param name="quantityType"></param>
        /// <returns></returns>
        private static IUnit UnitOf(Type quantityType)
        {
            #region base Quanities
            if (quantityType == typeof(Time)) return new Second();
            #endregion

            #region inherited Quantities

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
            IUnit unit = UnitOf(typeof(TQuantity));


            if (unit != null)
                return unit;
            else
            {
                throw new UnitNotFoundException("Quantity dosen't have shared unit.");
            }

        }

        /// <summary>
        /// Suggest a unit from the unit system of unit parameter.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unit">the guiding unit to know the desired unit system</param>
        /// <returns></returns>
        public static IUnit UnitOf(AnyQuantity quantity, UnitSystem use)
        {
            if (use == UnitSystem.SIUnitSystem)
            {
                return UnitSystems.SIUnitSystem.UnitOf(quantity);
            }
            else if (use == UnitSystem.EEUnitSystem)
            {
                throw new NotImplementedException();
            }
            else if (use == UnitSystem.GlobalUnitSystem)
            {
                IUnit xunit = UnitOf(quantity.GetType());
                if (xunit != null)
                    return xunit;
                else
                    throw new UnitNotFoundException("UnKnown Unit.");

            }
            else
            {
                throw new UnitNotFoundException("UnKnown Unit System.");
            }
        }
    }
}
