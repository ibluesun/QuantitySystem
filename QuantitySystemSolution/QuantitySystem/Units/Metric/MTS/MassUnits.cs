
using QuantitySystem.Quantities.BaseQuantities;


using QuantitySystem.Attributes;


namespace QuantitySystem.Units.Metric.Mts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tonne"), MetricUnit("mt", typeof(Mass<>), true)]
    [ReferenceUnit(1000)]
    public sealed class MetricTonne : MetricUnit
    {

    }

}