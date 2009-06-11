using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Attributes
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
        private readonly double denominator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public ReferenceUnitAttribute(double numerator)
        {
            this.numerator = numerator;
            this.denominator = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public ReferenceUnitAttribute(double numerator, double denumerator)
        {
            this.numerator = numerator;
            this.denominator = denumerator;

            
        }

        public Type UnitType { get; set; }


        /// <summary>
        /// Shift the conversion factor forward and backward.
        /// </summary>
        public double Shift { get; set; }

        public double Numerator
        {
            get
            {
                return numerator;
            }
        }
        public double Denominator
        {
            get
            {
                return denominator;
            }
        }


        public double Times
        {
            get
            {
                return (numerator / denominator) + Shift;
            }
        }

    }
}
