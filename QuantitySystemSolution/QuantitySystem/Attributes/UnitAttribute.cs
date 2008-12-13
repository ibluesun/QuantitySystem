using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Attributes
{
    /*
     * Definitions:
     *      Quantity: The type of the value container.
     *      Value Container: The generic which hold the value.
     *      Units Cloud: set of units refer to the same Quantity by its Dimension in The Same system.
     *      System of Units: a set of different Quantities units grouped into known system {namespace} like imperial and SI or even egyptian 
     */


    /// <summary>
    /// The base unit attribute for all units.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class UnitAttribute : Attribute
    {

        private readonly string symbol;
        private readonly Type quantityType;


        /// <summary>
        /// Unit Attribute Constructor.
        /// </summary>
        /// <param name="symbol">Symbol used for this unit.</param>
        /// <param name="quantityType">Quantity Type of this unit.</param>
        public UnitAttribute(string symbol, Type quantityType)
        {
            this.symbol = symbol;
            this.quantityType = quantityType;
        }

        public string Symbol 
        {
            get
            {
                return symbol;
            }
        }

        public Type QuantityType 
        {
            get
            {
                return quantityType;
            }
        }
    }

}
