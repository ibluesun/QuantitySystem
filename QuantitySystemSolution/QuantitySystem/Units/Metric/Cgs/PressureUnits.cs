using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.Metric.Cgs
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Barye"), MetricUnit("Ba", typeof(Stress<>), true)]
    [ReferenceUnit(0.1)]
    public sealed class Barye: MetricUnit
    {

    }
}