using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.English
{

    [DefaultUnit("BTU", typeof(Energy<>))]
    [ReferenceUnit(1054.8)]
    public sealed class BTU : Unit
    {
    }

    [Unit("MBTU", typeof(Energy<>))]
    [ReferenceUnit(1000, UnitType = typeof(BTU))]
    public sealed class MBTU : Unit
    {
    }

    [Unit("MMBTU", typeof(Energy<>))]
    [ReferenceUnit(1000000, UnitType = typeof(BTU))]
    public sealed class MMBTU : Unit
    {
    }

    [Unit("lbcal", typeof(Energy<>))]
    [ReferenceUnit(1.8, UnitType = typeof(BTU))]
    public sealed class PoundCalorie : Unit
    {
    }


}