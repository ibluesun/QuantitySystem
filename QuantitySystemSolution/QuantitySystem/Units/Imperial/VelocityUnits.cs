


using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.Imperial
{


    /// <summary>
    /// Nautical Mile Per Hour
    /// </summary>
    [Unit("kn", typeof(Velocity<>))]
    [ReferenceUnit(1852, 3600)]
    public sealed class Knot : Unit
    {

    }

}