using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric
{

    [MetricUnit("L", typeof(Volume<>))]
    [ReferenceUnit(1, 1000)]  //Litre = 1/1000 m^3
    public sealed class Litre : MetricUnit
    {

    }

}