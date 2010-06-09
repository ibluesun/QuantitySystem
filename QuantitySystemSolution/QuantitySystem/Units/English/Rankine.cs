using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English
{

    [DefaultUnit("R", typeof(Temperature<>))]
    [ReferenceUnit(5, 9)]
    public sealed class Rankine : Unit
    {
    }




    [Unit("°F", typeof(Temperature<>))]
    [ReferenceUnit(1, Shift = 459.67, UnitType = typeof(Rankine))]
    public sealed class Fahrenheit : Unit
    {

    }
}
