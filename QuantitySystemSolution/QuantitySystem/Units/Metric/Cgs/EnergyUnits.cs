using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.Metric.Cgs
{
    [MetricUnit("erg", typeof(Energy<>), true)]
    [ReferenceUnit(1E-7)]
    public sealed class Erg : MetricUnit
    {

    }

}