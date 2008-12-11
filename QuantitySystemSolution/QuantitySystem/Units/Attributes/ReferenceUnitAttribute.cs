﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Units.Attributes
{
    /// <summary>
    /// Make a relation between the attributed unit 
    /// and a parent unit.
    /// If UnitType omitted the reference unit will be the default SI unit 
    /// of the QuantityType of the Unit or DefaultUnit Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public sealed class ReferenceUnitAttribute : Attribute
    {
        private readonly double numerator;
        private readonly double denumerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public ReferenceUnitAttribute(double numerator)
        {
            this.numerator = numerator;
            this.denumerator = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public ReferenceUnitAttribute(double numerator, double denumerator)
        {
            this.numerator = numerator;
            this.denumerator = denumerator;

            
        }
       
        public Type UnitType { get; set; }

        public double Numerator
        {
            get
            {
                return numerator;
            }
        }
        public double Denumerator
        {
            get
            {
                return denumerator;
            }
        }


        public double Times
        {
            get
            {
                return numerator / denumerator;
            }
        }

    }
}
