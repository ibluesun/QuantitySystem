using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.Attributes;

namespace QuantitySystem.Units.SI
{
    public abstract class SIUnit:Unit
    {

        #region Constructors


        protected SIUnit()
        {
            isDefaultUnit = true;  //SI units always default units because default is depending on the default prefix

            //access the SIUnitAttribute
            MemberInfo info = this.GetType();

            object[] attributes = (object[])info.GetCustomAttributes(true);

            //get the UnitAttribute
            SIUnitAttribute siua = (SIUnitAttribute)attributes.SingleOrDefault<object>(ut => ut is SIUnitAttribute);

            if (siua != null)
            {

                unitPrefix = SIPrefix.FromPrefixName(siua.DefaultPrefix.ToString());

            }
            else
            {
                throw new UnitException("SIUnitAttribute Not Found");
            }
        }


        #endregion


        private SIPrefix unitPrefix;

        public SIPrefix UnitPrefix
        {
            get { return unitPrefix; }
            set { unitPrefix = value; }
        }


        public override string Symbol
        {
            get
            {
                return unitPrefix.Symbol + base.Symbol;
            }
        }

        public override double ToSIUnit(double relativeValue)
        {
            return relativeValue * UnitPrefix.Factor;
        }

        public override double FromSIUnit(double absoluteValue)
        {
            
            return absoluteValue / UnitPrefix.Factor;            
        }

        

        public double ToPrefix(SIPrefix prefix, double value)
        {
            double abs = ToSIUnit(value);
            return abs * prefix.Factor;
        }


        //public bool IsSpecialName { get { return true; } }


        /* exclude from code for now */

        //#region Unit Operations
        
         
        ///// <summary>
        ///// multiply units by adding prefixes.
        ///// </summary>
        ///// <param name="unit"></param>
        ///// <returns></returns>
        //public override IUnit Multiply(IUnit unit)
        //{
        //    //convert the passed unit to si unit
        //    //make a derived unit from this unit and passed unit

        //    return new DerivedSIUnit(this, (ISIUnit)unit);

        //    /*
        //     * m * km := m.km prefix = k
        //     * km * km := km.km prefix = M
        //     * kg^2 * kN^3 := kg^2.kN^3  prefix = M
        //     */
        //}

        //public override IUnit Divide(IUnit unit)
        //{
        //    IUnit InvertedUnit = unit.Invert();
        //    return new DerivedSIUnit(this, (ISIUnit)InvertedUnit);
        //}

        ///// <summary>
        ///// Summing two units prefixes after getting the unit to its base units.
        ///// </summary>
        ///// <param name="unit"></param>
        ///// <returns></returns>
        //public override IUnit Add(IUnit unit)
        //{
        //    ISIUnit siUnit = unit as ISIUnit;
        //    if (siUnit != null)
        //    {
        //        //if current unit is special name then its base units may be different in prefixes/
        //        // get the BaseUnits with their default prefixes
        //        // calculate the prefixes difference
        //        // add the difference to the current unit


        //        ISIUnit nun = (ISIUnit)this.MemberwiseClone();

        //        ISIUnit MeInBaseUnits = this.GetUnitInBaseUnits(); //get me as base units
        //        ISIUnit unitBaseUnits = siUnit.GetUnitInBaseUnits();

        //        if (MeInBaseUnits.Dimension.Equals(unitBaseUnits.Dimension) == false)
        //        {
        //            throw new UnitsNotDimensionallyEqualException();
        //        }

        //        if (MeInBaseUnits.Prefix.Exponent == unitBaseUnits.Prefix.Exponent)
        //        {
        //            return nun;
        //        }
        //        else 
        //        {
        //            nun.Prefix = unitBaseUnits.Prefix - MeInBaseUnits.Prefix;
        //        }

        //        return nun;
        //    }
        //    else
        //    {
        //        throw new UnitNotFoundException("The unit in this block is from another unit system");
        //    }


        //}

        //public override IUnit Subtract(IUnit unit)
        //{
        //    throw new NotImplementedException();
        //}

        ////public override QuantitySystem.Units.IUnit Invert()
        ////{
        ////    ISIUnit unit = (ISIUnit)base.Invert();
        ////    unit.Prefix = unit.Prefix.Invert();
        ////    return unit;
        ////}

        //public override IUnit CorrectUnitBy(IUnit unit)
        //{
            
        //    ISIUnit neu = (ISIUnit)this.MemberwiseClone();

        //    ISIUnit MeInBaseUnits = this.GetUnitInBaseUnits(); //get me as base units

        //    ISIUnit unitBaseUnits = ((ISIUnit)unit).GetUnitInBaseUnits();

        //    //if (MeInBaseUnits.Dimension.Equals(unitBaseUnits.Dimension) == false)
        //    if (MeInBaseUnits.Dimension.IsEqual(unitBaseUnits.Dimension) == false)
        //    {
        //        throw new UnitsNotDimensionallyEqualException();
        //    }

        //    if (neu.Dimension.ForceExponent != 0)
        //    {
        //        //reduce the other unit prefix by 3 and associate the prefix to the neu
        //        SIPrefixes neuPrefix = SIPrefixes.FromExponent(unitBaseUnits.Prefix.Exponent - 3);
        //        neu.Prefix = neuPrefix;
        //    }
        //    else
        //    {
        //        neu.Prefix = unitBaseUnits.Prefix;
        //    }


        //    return neu;

        //}

        //*/
        //#endregion
        
        //public virtual ISIUnit GetUnitInBaseUnits()
        //{
        //    if (this.IsBaseUnit)
        //        return this;
        //    else
        //    {
        //        DerivedSIUnit dsi = new DerivedSIUnit(Dimension);
        //        //dsi.Prefix += Prefix;
        //        dsi.AddPrefix(Prefix);

        //        //if (Exponent < 0) dsi = (DerivedSIUnit)dsi.Invert();
        //        return dsi;
        //    }
        //}
        
        //public override QuantityDimension Dimension
        //{
        //    get
        //    {
        //        QuantityDimension qd = QuantityDimension.DimensionFrom(typeof(TQuantity));
        //        return qd * Exponent;
        //    }
        //}
    
        /* end exclude */
    
    }
}
