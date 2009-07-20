using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric
{
    // U+00B0 Degree Sign
    // Keystroke ALT+0167   Hex 0xB0

    [MetricUnit("°C", typeof(Temperature<>))]
    [ReferenceUnit(1, Shift = 273.15)]
    public sealed class Celsius : MetricUnit
    {
    }
}
