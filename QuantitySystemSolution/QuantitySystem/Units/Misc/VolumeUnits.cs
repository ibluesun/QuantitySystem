using QuantitySystem.Quantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Misc
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Centimetre"), Unit("cc", typeof(Volume<>))]
    [ReferenceUnit(1e-6)]
    public sealed class CubicCentimetre : Unit
    {
    }
}
