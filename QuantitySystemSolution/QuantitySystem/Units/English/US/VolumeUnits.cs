using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.English.US
{


    [Unit("lpt", typeof(Volume<>))]
    [ReferenceUnit(0.83267418463, UnitType=typeof(Pint))]
    public sealed class LiquidPint : Unit
    {

    }

    [Unit("dpt", typeof(Volume<>))]
    [ReferenceUnit(0.96893897192, UnitType = typeof(Pint))]
    public sealed class DryPint : Unit
    {

    }

    [Unit("dgal", typeof(Volume<>))]
    [ReferenceUnit(8, UnitType = typeof(DryPint))]
    public sealed class DryGallon : Unit
    {

    }


    [Unit("dqt", typeof(Volume<>))]
    [ReferenceUnit(2, UnitType = typeof(DryPint))]
    public sealed class DryQuart : Unit
    {

    }

}