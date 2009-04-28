﻿using System;
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


        public override string Symbol
        {
            get
            {
                //if (IsInverted)
                //{
                //    return "<1/" + unitPrefix.Symbol + symbol + ">";
                //}
                //else
                {
                    return unitPrefix.Symbol + base.Symbol;
                }
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
    
    }
}