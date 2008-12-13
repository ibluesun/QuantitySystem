﻿
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;


namespace QuantitySystem.Units.Imperial
{
    [Unit("grain", typeof(Mass<>))]
    [ReferenceUnit(1, 7000, UnitType = typeof(Pound))]
    public sealed class Grain : Unit
    {

    }
    
    [Unit("drachm", typeof(Mass<>))]
    [ReferenceUnit(1, 256, UnitType = typeof(Pound))]
    public sealed class Drachm : Unit
    {

    }

    [Unit("oz", typeof(Mass<>))]
    [ReferenceUnit(1, 16, UnitType = typeof(Pound))]
    public sealed class Ounce : Unit
    {

    }

    [DefaultUnit("lbm", typeof(Mass<>))]
    [ReferenceUnit(0.45359237)]
    public sealed class Pound : Unit
    {

    }

    [Unit("st", typeof(Mass<>))]
    [ReferenceUnit(14, UnitType = typeof(Pound))]
    public sealed class Stone : Unit
    {

    }

    [Unit("quarter", typeof(Mass<>))]
    [ReferenceUnit(28, UnitType = typeof(Pound))]
    public sealed class Quarter : Unit
    {

    }



    [Unit("cwt", typeof(Mass<>))]
    [ReferenceUnit(112, UnitType = typeof(Pound))]
    public sealed class Hundredweight : Unit
    {

    }


    [Unit("t", typeof(Mass<>))]
    [ReferenceUnit(2240, UnitType = typeof(Pound))]
    public sealed class Tonne : Unit
    {

    }


}
