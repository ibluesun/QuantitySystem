using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Astronomical
{
    [Unit("au", typeof(Length<>))]
    [ReferenceUnit(1.495978706916E+11)]
    public sealed class AstronomicalUnit : Unit
    {

    }


    [MetricUnit("ly", typeof(Length<>))]
    [ReferenceUnit(9.460530E+15)]
    public sealed class LightYear : MetricUnit
    {

    }


    [MetricUnit("pc", typeof(Length<>))]
    [ReferenceUnit(30.857E+12)]
    public sealed class Parsec : MetricUnit
    {
    }

}
