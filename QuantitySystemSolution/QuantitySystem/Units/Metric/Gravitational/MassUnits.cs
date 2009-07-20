using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.Gravitational
{

    [MetricUnit("hyl", typeof(Mass<>), true)]
    [ReferenceUnit(9.80665)]
    public sealed class Hyl : MetricUnit
    {
    }
}
