using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units
{
    public abstract partial class MetricUnit:Unit
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


        /// <summary>
        /// unit symbol with prefix.
        /// </summary>
        public override string Symbol
        {
            get
            {
                {
                    return unitPrefix.Symbol + base.Symbol;
                }
            }
        }

        /// <summary>
        /// Tells if the current unit in default mode or not.
        /// </summary>
        public override bool IsDefaultUnit
        {
            get
            {
                //return true only and if only the current prefix equal the default prefix.
                if (_ReferenceUnit == null)
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
                if (_ReferenceUnit == null)
                {
                    // I am native SI unit
                    if (IsDefaultUnit)
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
                        RefUnit.UnitExponent = this.UnitExponent;

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
                if (_ReferenceUnit == null)
                {
                    if (IsDefaultUnit)
                        return 0;
                    else
                        return 1;
                }
                else
                {
                    //return referenceUnitDenominator;
                    return Math.Pow(_ReferenceUnitDenominator, UnitExponent);

                }
            }
        }


        /// <summary>
        /// Reference unit is generated according to the state of the unit
        /// if the metric unit is not in the default prefix mode
        ///    the reference numerator is calculated based on difference between current prefix and default prefix
        ///    raised to the unit exponent.
        /// </summary>
        public override double ReferenceUnitNumerator
        {
            get
            {
                if (_ReferenceUnit == null)
                {
                    if (IsDefaultUnit)
                        return 0;
                    else
                        return Math.Pow(this.defaultUnitPrefix.GetFactorForConvertTo(UnitPrefix), UnitExponent);
                }
                else
                {
                    //convert me to default also if I had prefix over the default of me
                    double CorrectToDefault = Math.Pow(this.defaultUnitPrefix.GetFactorForConvertTo(UnitPrefix), UnitExponent);

                    //p.u   where
                    //      p: prefix
                    //      u: metric unit
                    //(p.u) i.e.  km, mm, Gare
                    //(p.u)^r  = p^r*u^r

                    return Math.Pow(_ReferenceUnitNumerator, UnitExponent) * CorrectToDefault;

                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.unitPrefix.Exponent == 0)
                return this.GetType().Name + " " + this.Symbol;
            else
                return this.UnitPrefix.Prefix + this.GetType().Name.ToLower() + " " + this.Symbol;
        }
    }
}
