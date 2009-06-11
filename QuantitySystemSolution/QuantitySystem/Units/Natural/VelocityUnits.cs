﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Natural
{
    [Unit("c0", typeof(Velocity<>))]
    [ReferenceUnit(299792458)]
    public sealed class LightSpeed : Unit
    {

    }
}
