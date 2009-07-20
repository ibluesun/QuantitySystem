using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric.Gravitational
{
    [MetricUnit("gf", typeof(Force<>))]
    [ReferenceUnit(1, UnitType = typeof(Pond))]
    public sealed class GramForce : MetricUnit
    {
    }

    [MetricUnit("p", typeof(Force<>), true, GravitationalPrefix = MetricPrefixes.Kilo)]
    [ReferenceUnit(9.80665)]
    public sealed class Pond : MetricUnit
    {
    }

}
