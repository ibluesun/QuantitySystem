using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using System;

namespace QuantitySystem.Units.Misc
{


    [Unit("r", typeof(Angle<>))]
    [ReferenceUnit(360, UnitType = typeof(ArcDegree))]
    public sealed class Revolution : Unit
    {
    }





    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gradian"), Unit("grad", typeof(Angle<>))]
    [ReferenceUnit(9, 10, UnitType = typeof(ArcDegree))]
    public sealed class Gradian : Unit
    {
    }



    [DefaultUnit("deg", typeof(Angle<>))]
    [ReferenceUnit(Math.PI, 180)]
    public sealed class ArcDegree : Unit
    {
    }

    [Unit("arcmin", typeof(Angle<>))]
    [ReferenceUnit(1, 60, UnitType = typeof(ArcDegree))]
    public sealed class ArcMinute : Unit
    {
    }


    [Unit("arcsec", typeof(Angle<>))]
    [ReferenceUnit(1, 60, UnitType = typeof(ArcMinute))]
    public sealed class ArcSecond : Unit
    {
    }


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Milli"), Unit("mas", typeof(Angle<>))]
    [ReferenceUnit(1, 1000, UnitType = typeof(ArcSecond))]
    public sealed class MilliArcSecond : Unit
    {
    }

}
