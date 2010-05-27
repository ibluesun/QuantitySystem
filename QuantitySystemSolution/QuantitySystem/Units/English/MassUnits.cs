using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;


namespace QuantitySystem.Units.English
{
    [Unit("slug", typeof(Mass<>))]
    [ReferenceUnit(32.17405, UnitType = typeof(Pound))]
    public sealed class Slug : Unit
    {

    }

    [Unit("gr", typeof(Mass<>))]
    [ReferenceUnit(1, 7000, UnitType = typeof(Pound))]
    public sealed class Grain : Unit
    {

    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Drachm"), Unit("drachm", typeof(Mass<>))]
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


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tonne"), Unit("t", typeof(Mass<>))]
    [ReferenceUnit(2240, UnitType = typeof(Pound))]
    public sealed class Tonne : Unit
    {

    }

}
