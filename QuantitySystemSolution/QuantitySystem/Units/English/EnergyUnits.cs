using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.English
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "BTU"), DefaultUnit("BTU", typeof(Energy<>))]
    [ReferenceUnit(1054.8)]
    public sealed class BTU : Unit
    {

    }

}