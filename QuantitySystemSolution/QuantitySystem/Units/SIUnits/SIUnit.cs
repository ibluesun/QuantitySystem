using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.UnitSystems;

namespace QuantitySystem.Units.SIUnits
{
    public abstract class SIUnit<TQuantity> : Unit<TQuantity>, ISIUnit where TQuantity : AnyQuantity, new()
    {

        #region Constructors
        protected SIUnit()
        {
            UnitPrefix = SIPrefix.Default;
        }

        protected SIUnit(SIPrefix prefix)
        {
            UnitPrefix = prefix;
        }
        #endregion

        private SIPrefix UnitPrefix;

        public SIPrefix Prefix
        {
            get { return UnitPrefix; }
            set { UnitPrefix = value; }
        }


        public override double GetAbsoluteValue(double relativeValue)
        {
            return relativeValue * Prefix.Factor;
        }

        public override double GetRelativeValue(double absoluteValue)
        {
            
            return absoluteValue / Prefix.Factor;            
        }

        public override string Symbol
        {
            get { return Prefix.Symbol; }
        }

        public double ToPrefix(SIPrefix prefix, double value)
        {
            double abs = GetAbsoluteValue(value);
            return abs * prefix.Factor;
        }

        public override UnitSystem UnitSystem { get { return UnitSystem.SIUnitSystem; } }

        public override bool IsSpecialName { get { return true; } }


        #region Unit Operations

        /// <summary>
        /// multiply units by adding prefixes.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public override IUnit Multiply(IUnit unit)
        {
            //convert the passed unit to si unit
            //make a derived unit from this unit and passed unit

            return new DerivedSIUnit(this, (ISIUnit)unit);

            /*
             * m * km := m.km prefix = k
             * km * km := km.km prefix = M
             * kg^2 * kN^3 := kg^2.kN^3  prefix = M
             */
        }

        public override IUnit Divide(IUnit unit)
        {
            IUnit InvertedUnit = unit.Invert();
            return new DerivedSIUnit(this, (ISIUnit)InvertedUnit);
        }

        /// <summary>
        /// Summing two units prefixes after getting the unit to its base units.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public override IUnit Add(IUnit unit)
        {
            ISIUnit siUnit = unit as ISIUnit;
            if (siUnit != null)
            {
                //if current unit is special name then its base units may be different in prefixes/
                // get the BaseUnits with their default prefixes
                // calculate the prefixes difference
                // add the difference to the current unit


                ISIUnit nun = (ISIUnit)this.MemberwiseClone();

                ISIUnit MeInBaseUnits = this.GetUnitInBaseUnits(); //get me as base units
                ISIUnit unitBaseUnits = siUnit.GetUnitInBaseUnits();

                if (MeInBaseUnits.Dimension.Equals(unitBaseUnits.Dimension) == false)
                {
                    throw new UnitsNotDimensionallyEqualException();
                }

                if (MeInBaseUnits.Prefix.Exponent == unitBaseUnits.Prefix.Exponent)
                {
                    return nun;
                }
                else 
                {
                    nun.Prefix = unitBaseUnits.Prefix - MeInBaseUnits.Prefix;
                }

                return nun;
            }
            else
            {
                throw new UnitNotFoundException("The unit in this block is from another unit system");
            }


        }

        public override IUnit Subtract(IUnit unit)
        {
            throw new NotImplementedException();
        }

        //public override QuantitySystem.Units.IUnit Invert()
        //{
        //    ISIUnit unit = (ISIUnit)base.Invert();
        //    unit.Prefix = unit.Prefix.Invert();
        //    return unit;
        //}

        public override IUnit CorrectUnitBy(IUnit unit)
        {
            
            ISIUnit neu = (ISIUnit)this.MemberwiseClone();

            ISIUnit MeInBaseUnits = this.GetUnitInBaseUnits(); //get me as base units

            ISIUnit unitBaseUnits = ((ISIUnit)unit).GetUnitInBaseUnits();

            if (MeInBaseUnits.Dimension.Equals(unitBaseUnits.Dimension) == false)
            {
                throw new UnitsNotDimensionallyEqualException();
            }

            if (neu.Dimension.ForceExponent != 0)
            {
                //reduce the other unit prefix by 3 and associate the prefix to the neu
                SIPrefix neuPrefix = SIPrefix.FromExponent(unitBaseUnits.Prefix.Exponent - 3);
                neu.Prefix = neuPrefix;
            }
            else
            {
                neu.Prefix = unitBaseUnits.Prefix;
            }


            return neu;

        }

        #endregion

        public virtual ISIUnit GetUnitInBaseUnits()
        {
            if (this.IsBaseUnit)
                return this;
            else
            {
                DerivedSIUnit dsi = new DerivedSIUnit(Dimension);
                //dsi.Prefix += Prefix;
                dsi.AddPrefix(Prefix);

                //if (Exponent < 0) dsi = (DerivedSIUnit)dsi.Invert();
                return dsi;
            }
        }

        public virtual QuantityDimension Dimension
        {
            get
            {
                QuantityDimension qd = QuantityDimension.DimensionFrom(typeof(TQuantity));
                return qd * Exponent;
            }
        }
    }
}
