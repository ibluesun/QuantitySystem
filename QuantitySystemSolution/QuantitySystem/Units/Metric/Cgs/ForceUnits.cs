using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.Metric.Cgs
{
    [MetricUnit("dyn", typeof(Force<>), true)]
    [ReferenceUnit(1e-5)]
    public sealed class Dyne : MetricUnit
    {

    }

}