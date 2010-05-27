using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.Gravitational
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hyl"), MetricUnit("hyl", typeof(Mass<>), true)]
    [ReferenceUnit(9.80665)]
    public sealed class Hyl : MetricUnit
    {
    }
}
