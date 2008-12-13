using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.Metric.Cgs
{
    [MetricUnit("Ba", typeof(Pressure<>), true)]
    [ReferenceUnit(0.1)]
    public sealed class Barye: MetricUnit
    {

    }
}