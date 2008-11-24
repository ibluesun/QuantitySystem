using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Units.UnitSystems;

namespace QuantitySystem.Units.SIUnits
{
    public interface ISIUnit : IUnit
    {
        SIPrefix Prefix { get; set; }

        double ToPrefix(SIPrefix prefix, double value);

        ISIUnit GetUnitInBaseUnits();

    }
}
