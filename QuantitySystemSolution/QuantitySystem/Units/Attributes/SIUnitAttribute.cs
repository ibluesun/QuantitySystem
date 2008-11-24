using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Units.Attributes
{
    /// <summary>
    /// Special Attribute for the SI Units.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public sealed class SIUnitAttribute : UnitAttribute
    {

        public SIUnitAttribute(string symbol, Type quantityType, SIPrefixes defaultPrefix)
            :base(symbol, quantityType)
        {
            this.defaultPrefix = defaultPrefix;
        }

        private readonly SIPrefixes defaultPrefix;

        public SIPrefixes DefaultPrefix
        {
            get
            {
                return defaultPrefix;
            }
        }


    }

}
