using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units
{
    public abstract class MetricUnit:Unit
    {

        #region Constructors


        protected MetricUnit()
        {

            //access the SIUnitAttribute
            MemberInfo info = this.GetType();

            object[] attributes = (object[])info.GetCustomAttributes(true);

            //get the UnitAttribute
            MetricUnitAttribute siua = (MetricUnitAttribute)attributes.SingleOrDefault<object>(ut => ut is MetricUnitAttribute);

            if (siua != null)
            {
                defaultUnitPrefix = MetricPrefix.FromPrefixName(siua.SiPrefix.ToString());
                unitPrefix = MetricPrefix.FromPrefixName(siua.SiPrefix.ToString());

            }
            else
            {
                throw new UnitException("SIUnitAttribute Not Found");
            }


            //ReferenceUnit attribute may or may not appear
            // if it appears then the unit is not a default SI unit
            // but act as SI Unit {means take prefixes}.  and accepted by the SI poids
            // however the code of reference unit is in the base class code.


        }


        #endregion

        private readonly MetricPrefix defaultUnitPrefix;

        /// <summary>
        /// Current unit default prefix.
        /// </summary>
        public MetricPrefix DefaultUnitPrefix
        {
            get { return defaultUnitPrefix; }
        } 


        private MetricPrefix unitPrefix;

        /// <summary>
        /// Current instance unit prefix.
        /// </summary>
        public MetricPrefix UnitPrefix
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

        /// <summary>
        /// Tells if the current unit in default mode or not.
        /// </summary>
        public override bool DefaultUnit
        {
            get
            {
                //return true only and if only the current prefix equal the default prefix.
                if (referenceUnit == null)
                {
                    //means I am native SI unit.
                    if (unitPrefix.Exponent == defaultUnitPrefix.Exponent)
                        return true;
                    else
                        return false;
                }
                else
                {
                    //reference unit exist this is not native SI unit
                    return false;
                }
            }
        }

        public override Unit ReferenceUnit
        {
            get
            {
                if (referenceUnit == null)
                {
                    // I am native SI unit
                    if (DefaultUnit)
                    {
                        //I am already default don't return extra parents.
                        return null;
                    }
                    else
                    {
                        // native si not in the default mode.
                        //put the default of this unit by creating it again
                        MetricUnit RefUnit = (MetricUnit)this.MemberwiseClone();
                        RefUnit.UnitPrefix = this.defaultUnitPrefix;

                        return RefUnit;
                    }
                }
                else
                {
                    //although it is inherited from SIUnit but the current instance
                    // is most probably a unit accepted to be used in SIUnit
                    // so it is not the default unit.
                    // so we need the si reference unit.
                    // the reference must be SI.
                    return base.ReferenceUnit;
                }
            }
        }

        public override double ReferenceUnitDenominator
        {
            get
            {
                if (referenceUnit == null)
                {
                    if (DefaultUnit)
                        return 0;
                    else
                        return 1;
                }
                else
                {
                    return referenceUnitDenominator;

                }
            }
        }

        public override double ReferenceUnitNumerator
        {
            get
            {
                
                if (referenceUnit == null)
                {
                    if (DefaultUnit)
                        return 0;
                    else
                        return this.defaultUnitPrefix.GetFactorForConvertTo(UnitPrefix);
                }
                else
                {
                    //convert me to default also if I had prefix over the default of me
                    double CorrectToDefault = this.defaultUnitPrefix.GetFactorForConvertTo(UnitPrefix);

                    return referenceUnitNumerator * CorrectToDefault;

                }
            }
        }

        

        




        /* exclude from code for now */

        //public bool IsSpecialName { get { return true; } }
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
