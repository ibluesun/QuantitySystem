
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;


namespace QuantitySystem.Units.English
{

    [Unit("thou", typeof(Length<>))]
    [ReferenceUnit(1.0, 12000.0, UnitType = typeof(Foot))]
    public sealed class Thou : Unit
    {
    }


    [Unit("in", typeof(Length<>))]
    [ReferenceUnit(1.0, 12.0, UnitType = typeof(Foot))]
    public sealed class Inch : Unit
    {
    }

    [DefaultUnit("ft", typeof(Length<>))]
    //[ReferenceUnit(1200, 3937)]
    [ReferenceUnit(0.3048)]
    public sealed class Foot : Unit
    {
    }

    [Unit("yd", typeof(Length<>))]
    [ReferenceUnit(3, UnitType = typeof(Foot))]
    public sealed class Yard : Unit
    {
    }

    [Unit("ftm", typeof(Length<>))]
    [ReferenceUnit(6, UnitType = typeof(Foot))]
    public sealed class Fathom : Unit
    {
    }


    [Unit("fur", typeof(Length<>))]
    [ReferenceUnit(220, UnitType = typeof(Yard))]
    public sealed class Furlong : Unit
    {
    }

    [Unit("mil", typeof(Length<>))]
    [ReferenceUnit(1760, UnitType = typeof(Yard))]
    public sealed class Mile : Unit
    {
    }


    [Unit("league", typeof(Length<>))]
    [ReferenceUnit(3, UnitType = typeof(Mile))]
    public sealed class League : Unit
    {
    }


    [Unit("cable", typeof(Length<>))]
    [ReferenceUnit(608, UnitType = typeof(Foot))]
    public sealed class Cable : Unit
    {
    }


    /// <summary>
    /// Minute of arc along any meridian.
    /// </summary>
    [Unit("nmi", typeof(Length<>))]
    [ReferenceUnit(2315000, 381, UnitType = typeof(Foot))]
    public sealed class NauticalMile : Unit
    {

    }



    [Unit("lnk", typeof(Length<>))]
    [ReferenceUnit(66, 100, UnitType = typeof(Foot))]
    public sealed class Link : Unit
    {

    }



    [Unit("rod", typeof(Length<>))]
    [ReferenceUnit(25, UnitType = typeof(Link))]
    public sealed class Rod : Unit
    {

    }

    [Unit("chain", typeof(Length<>))]
    [ReferenceUnit(4, UnitType = typeof(Rod))]
    public sealed class Chain : Unit
    {

    }
}
