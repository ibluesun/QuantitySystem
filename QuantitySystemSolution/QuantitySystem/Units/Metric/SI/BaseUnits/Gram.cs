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


    }
}
