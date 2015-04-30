using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital
{
    [Unit("B", typeof(Digital<>))]
    [ReferenceUnit(8, UnitType = typeof(Bit))]
    public sealed class Byte : Unit
    {
    }


    #region decimal
    [Unit("kB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(Byte))]
    public sealed class KiloByte : Unit
    {
    }

    [Unit("MB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(KiloByte))]
    public sealed class MegaByte : Unit
    {
    }

    [Unit("GB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(MegaByte))]
    public sealed class GigaByte : Unit
    {
    }

    [Unit("TB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(GigaByte))]
    public sealed class TeraByte : Unit
    {
    }

    [Unit("PB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(TeraByte))]
    public sealed class PetaByte : Unit
    {
    }

    [Unit("EB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(PetaByte))]
    public sealed class ExaByte : Unit
    {
    }

    [Unit("ZB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(ExaByte))]
    public sealed class ZettaByte : Unit
    {
    }

    [Unit("YB", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(ZettaByte))]
    public sealed class YottaByte : Unit
    {
    }

    #endregion


    #region Binary

    [Unit("KiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(Byte))]
    public sealed class KibiByte : Unit
    {
    }

    [Unit("MiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(KibiByte))]
    public sealed class MebiByte : Unit
    {
    }

    [Unit("GiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(MebiByte))]
    public sealed class GibiByte : Unit
    {
    }

    [Unit("TiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(GibiByte))]
    public sealed class TibiByte : Unit
    {
    }

    [Unit("PiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(TibiByte))]
    public sealed class PebiByte : Unit
    {
    }

    [Unit("EiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(PebiByte))]
    public sealed class ExbiByte : Unit
    {
    }

    [Unit("ZiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(ExbiByte))]
    public sealed class ZebiByte : Unit
    {
    }

    [Unit("YiB", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(ZebiByte))]
    public sealed class YobiByte : Unit
    {
    }

    #endregion

}
