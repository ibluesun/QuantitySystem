using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.SI
{
    [MetricUnit("Hz", typeof(Frequency<>))]
    public sealed class Hertz : MetricUnit
    {
    }
}
