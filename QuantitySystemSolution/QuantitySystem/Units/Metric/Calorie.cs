using QuantitySystem.Attributes;
using QuantitySystem.Quantities;
using QuantitySystem.Units.Metric.SI;

namespace QuantitySystem.Units.Metric
{
    
    [MetricUnit("cal", typeof(Energy<>))]
    [ReferenceUnit(4184, 1000)]
    public sealed class Calorie : MetricUnit
    {
    }

}
