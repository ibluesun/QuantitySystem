using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Misc
{

    [MetricUnit("Å", typeof(Length<>))]
    [ReferenceUnit(1E-10)]
    public sealed class Angstrom : MetricUnit
    {

    }

}
