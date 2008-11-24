using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.Attributes;


namespace QuantitySystem.Units.Imperial
{

    [Unit("in", typeof(Length<>))]
    [ReferenceUnit(1.0/12.0, UnitType = typeof(Foot))]
    public class Inch : Unit
    {
    }

    [DefaultUnit("ft", typeof(Length<>))]
    [ReferenceUnit(0.30480)]
    public class Foot : Unit
    {
    }

    [Unit("yd", typeof(Length<>))]
    [ReferenceUnit(3, UnitType = typeof(Foot))]
    public class Yard : Unit
    {
    }

    [Unit("mil", typeof(Length<>))]
    [ReferenceUnit(5280, UnitType = typeof(Foot))]
    public class Mile : Unit
    {
    }

}
