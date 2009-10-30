using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.English
{


    /// <summary>
    /// Nautical Mile Per Hour
    /// </summary>
    [DefaultUnit("kn", typeof(Speed<>))]
    [ReferenceUnit(1852, 3600)]
    public sealed class Knot : Unit
    {


    }

}