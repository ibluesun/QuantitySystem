using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Natural
{
    [Unit("me", typeof(Mass<>))]
    [ReferenceUnit(9.109382616E-31)]
    public sealed class ElectronMass : Unit
    {

    }

    [Unit("mp", typeof(Mass<>))]
    [ReferenceUnit(1.672621637E-27)]
    public sealed class ProtonMass : Unit
    {

    }
}
