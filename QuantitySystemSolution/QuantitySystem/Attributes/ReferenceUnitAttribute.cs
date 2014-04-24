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

        private readonly string source;

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
        public ReferenceUnitAttribute(double numerator, double denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="source">FunctionName.UnitName</param>
        public ReferenceUnitAttribute(Type unitType, string source)
        {
            this.source = source;
            UnitType = unitType;
            
            this.denominator = 1;

            // the numerator will be calculated based on source value

            
            string SourceFunctionName = source.Substring(0, source.IndexOf('.'));

            if (!DynamicQuantitySystem.DynamicSourceFunctions.ContainsKey(SourceFunctionName))
                DynamicQuantitySystem.DynamicSourceFunctions[SourceFunctionName] = (u) => 1.0;   // always return 1.0;
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
                if (!string.IsNullOrEmpty(source))
                {
                    string SourceFunctionName = source.Substring(0, source.IndexOf('.'));
                    string UnitKey = source.Substring(source.IndexOf('.') + 1);
                    return DynamicQuantitySystem.DynamicSourceFunctions[SourceFunctionName](UnitKey);
                }
                
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
