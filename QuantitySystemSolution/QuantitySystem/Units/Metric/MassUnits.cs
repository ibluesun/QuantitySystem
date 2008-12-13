using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;


namespace QuantitySystem.Units.Metric
{

    [MetricUnit("Da", typeof(Mass<>))]
    [ReferenceUnit(1.6605388628E-27)]
    public sealed class Dalton : MetricUnit
    {

    }

    [MetricUnit("me", typeof(Mass<>))]
    [ReferenceUnit(9.109382616E-31)]
    public sealed class ElectronMass : MetricUnit
    {

    }
}