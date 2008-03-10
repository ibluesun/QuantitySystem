using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Velocity : DerivedQuantity
    {
        public Velocity()
            : base(1, new Length(), new Time(-1))
        {
        }

        public Velocity(int exponent)
            : base(exponent, new Length(exponent), new Time(-1 * exponent))
        {
        }

        public static Velocity LightSpeed
        {
            get
            {
                Velocity v = new Velocity();
                v.Value = 299792458;

                //assign the unit here  m/s
                Units.SIUnits.BaseUnits.Metre m = new QuantitySystem.Units.SIUnits.BaseUnits.Metre();

                Units.Second s = new Units.Second();

                s = (Units.Second)s.Invert();

                v.Unit = new Units.SIUnits.DerivedSIUnit(m, s);
                return v;
            }
        }
    }
}
