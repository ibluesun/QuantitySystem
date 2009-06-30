using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English
{
    [DefaultUnit("lbmol", typeof(AmountOfSubstance<>))]
    [ReferenceUnit(453.59237)]
    public sealed class PoundMole:Unit 
    {
    }
}
