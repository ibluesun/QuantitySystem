using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.English
{
    [DefaultUnit("lbf", typeof(Force<>))]
    [ReferenceUnit(4.4482216152605)]
    public sealed class PoundForce : Unit
    {

    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Poundal"), Unit("pdl", typeof(Force<>))]
    [ReferenceUnit(0.031080950171567253, UnitType=typeof(PoundForce))]
    public sealed class Poundal : Unit
    {

    }
}