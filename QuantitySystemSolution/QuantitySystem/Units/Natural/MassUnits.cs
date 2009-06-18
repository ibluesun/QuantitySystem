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



    [MetricUnit("Da", typeof(Mass<>))]
    [ReferenceUnit(1.6605388628E-27)]
    public sealed class Dalton : MetricUnit
    {

    }


    /// <summary>
    /// Same as Dalton
    /// </summary>
    [Unit("u", typeof(Mass<>))]
    [ReferenceUnit(1, UnitType = typeof(Dalton))]
    public sealed class UnifiedAtomicMass : Unit
    {

    }




}
