using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital
{
    [DefaultUnit("bit", typeof(Digital<>))]
    public sealed class Bit : Unit
    {
    }

    #region decimal 
    [Unit("kbit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(Bit))]
    public sealed class KiloBit : Unit
    {
    }

    [Unit("Mbit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(KiloBit))]
    public sealed class MegaBit : Unit
    {
    }

    [Unit("Gbit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(MegaBit))]
    public sealed class GigaBit : Unit
    {
    }

    [Unit("Tbit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(GigaBit))]
    public sealed class TeraBit : Unit
    {
    }

    [Unit("Pbit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(TeraBit))]
    public sealed class PetaBit : Unit
    {
    }

    [Unit("Ebit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(PetaBit))]
    public sealed class ExaBit : Unit
    {
    }

    [Unit("Zbit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(ExaBit))]
    public sealed class ZettaBit : Unit
    {
    }

    [Unit("Ybit", typeof(Digital<>))]
    [ReferenceUnit(1000, UnitType = typeof(ZettaBit))]
    public sealed class YottaBit : Unit
    {
    }

    #endregion


    #region Binary

    [Unit("Kibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(Bit))]
    public sealed class KibiBit : Unit
    {
    }

    [Unit("Mibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(KibiBit))]
    public sealed class MebiBit : Unit
    {
    }

    [Unit("Gibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(MebiBit))]
    public sealed class GibiBit : Unit
    {
    }

    [Unit("Tibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(GibiBit))]
    public sealed class TibiBit : Unit
    {
    }

    [Unit("Pibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(TibiBit))]
    public sealed class PebiBit : Unit
    {
    }

    [Unit("Eibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(PebiBit))]
    public sealed class ExbiBit : Unit
    {
    }

    [Unit("Zibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(ExbiBit))]
    public sealed class ZebiBit : Unit
    {
    }

    [Unit("Yibit", typeof(Digital<>))]
    [ReferenceUnit(1024, UnitType = typeof(ZebiBit))]
    public sealed class YobiBit : Unit
    {
    }

    #endregion

}
