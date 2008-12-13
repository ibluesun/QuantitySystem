using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Units;

namespace QuantitySystem.Attributes
{
    /// <summary>
    /// Special Attribute for the Metric Units.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public sealed class MetricUnitAttribute : UnitAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Symbol of the unit that appears in display</param>
        /// <param name="quantityType">The CLR type of quantity that this unit is associated to.</param>
        public MetricUnitAttribute(string symbol, Type quantityType)
            : base(symbol, quantityType)
        {
            systemDefault = false;  //but si units are implicitly defaults even if this field is true.
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Symbol of the unit that appears in display</param>
        /// <param name="quantityType">The CLR type of quantity that this unit is associated to.</param>
        /// <param name="systemDefault">Indicates that this unit is default unit of this system in this quantity. {SI namespace not affected}</param>
        public MetricUnitAttribute(string symbol, Type quantityType, bool systemDefault)
            : base(symbol, quantityType)
        {
            this.systemDefault = systemDefault;
        }


        private readonly bool systemDefault;

        public bool SystemDefault
        {
            get { return systemDefault; }
        }



        private MetricPrefixes siPrefix = MetricPrefixes.None;
        private MetricPrefixes cgsPrefix = MetricPrefixes.None;
        private MetricPrefixes mtsPrefix = MetricPrefixes.None;

        public MetricPrefixes SiPrefix
        {
            get { return siPrefix; }
            set { siPrefix = value; }
        }

        public MetricPrefixes CgsPrefix
        {
            get { return cgsPrefix; }
            set { cgsPrefix = value; }
        }

        public MetricPrefixes MtsPrefix
        {
            get { return mtsPrefix; }
            set { mtsPrefix = value; }
        }



    }

}
