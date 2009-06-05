using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.English
{

    [DefaultUnit("BTU", typeof(Energy<>))]
    [ReferenceUnit(1054.8)]
    public sealed class BTU : Unit
    {

    }

}