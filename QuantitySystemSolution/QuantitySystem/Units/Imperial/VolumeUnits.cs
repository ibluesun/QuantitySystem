﻿
using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.Imperial
{

    [Unit("fl_oz", typeof(Volume<>))]
    [ReferenceUnit(1, 20, UnitType = typeof(Pint))]
    public sealed class Fluidounce : Unit
    {

    }


    [Unit("gill", typeof(Volume<>))]
    [ReferenceUnit(1, 4, UnitType = typeof(Pint))]
    public sealed class Gill : Unit
    {

    }

    [DefaultUnit("pt", typeof(Volume<>))]
    [ReferenceUnit(0.5682, UnitType = typeof(Metric.Litre))]
    public sealed class Pint : Unit
    {

    }

    [Unit("qt", typeof(Volume<>))]
    [ReferenceUnit(2, UnitType = typeof(Pint))]
    public sealed class Quart : Unit
    {

    }

    [Unit("gal", typeof(Volume<>))]
    [ReferenceUnit(8, UnitType = typeof(Pint))]
    public sealed class Gallon : Unit
    {

    }



}