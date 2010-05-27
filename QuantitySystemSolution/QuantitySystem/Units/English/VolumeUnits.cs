using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.English
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "FluidOunce"), Unit("fl_oz", typeof(Volume<>))]
    [ReferenceUnit(1, 20, UnitType = typeof(Pint))]
    public sealed class FluidOunce : Unit
    {

    }

    [Unit("gill", typeof(Volume<>))]
    [ReferenceUnit(1, 4, UnitType = typeof(Pint))]
    public sealed class Gill : Unit
    {

    }

    [Unit("cup", typeof(Volume<>))]
    [ReferenceUnit(1, 2, UnitType = typeof(Pint))]
    public sealed class Cup : Unit
    {

    }

    [DefaultUnit("pt", typeof(Volume<>))]
    [ReferenceUnit(0.5682, 1000)]
    public sealed class Pint : Unit
    {

    }

    [Unit("qt", typeof(Volume<>))]
    [ReferenceUnit(2, UnitType = typeof(Pint))]
    public sealed class Quart : Unit
    {

    }

    [Unit("gal", typeof(Volume<>))]
    [ReferenceUnit(8, UnitType = typeof(Pint))]
    public sealed class Gallon : Unit
    {

    }



}