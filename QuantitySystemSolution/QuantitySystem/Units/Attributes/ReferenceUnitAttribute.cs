using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Units
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
        private readonly double times;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="times">Times of the reference unit.</param>
        public ReferenceUnitAttribute(double times)
        {
            this.times = times;
        }
       
        public Type UnitType { get; set; }

        public double Times
        {
            get
            {
                return times;
            }
        }

    }
}
