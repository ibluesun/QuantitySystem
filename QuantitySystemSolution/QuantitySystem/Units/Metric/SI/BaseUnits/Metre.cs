using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.SI
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Metre"), MetricUnit("m", typeof(Length<>), CgsPrefix = MetricPrefixes.Centi)]
    public sealed class Metre : MetricUnit
    {

    }
}
