using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.SI
{
    [MetricUnit("m", typeof(Length<>), CgsPrefix = MetricPrefixes.Centi)]
    public sealed class Metre : MetricUnit
    {

    }
}
