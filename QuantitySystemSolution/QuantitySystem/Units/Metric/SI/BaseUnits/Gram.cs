using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;


using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.SI
{
    [MetricUnit("g", typeof(Mass<>), SiPrefix =  MetricPrefixes.Kilo)]
    public sealed class Gram : MetricUnit
    {



        /*
        public override double ToSIUnit(double relativeValue)
        {
            //because this unit base is in KiloGram as SI standard
            // its absolute is always with kg
            // 1000000   /  1000  == 1000 
            // relative: 5 MegaGram = 5 * 1000; 5000 KiloGram

            double factor = Prefix.Factor / SIPrefix.Kilo.Factor;
            return relativeValue * factor;
        }

        public override double FromSIUnit(double absoluteValue)
        {

            double factor = Prefix.Factor / SIPrefix.Kilo.Factor;

            return absoluteValue / factor;
        }

        */
    }
}
