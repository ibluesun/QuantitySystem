
using QuantitySystem.Quantities.BaseQuantities;


using QuantitySystem.Attributes;


namespace QuantitySystem.Units.Metric
{

    [MetricUnit("ua", typeof(Length<>))]
    [ReferenceUnit(1.495978706916E+11)]
    public sealed class AstronomicalUnit : MetricUnit
    {

    }

    [MetricUnit("Å", typeof(Length<>))]
    [ReferenceUnit(1E-10)]
    public sealed class Angstrom : MetricUnit
    {

    }


}