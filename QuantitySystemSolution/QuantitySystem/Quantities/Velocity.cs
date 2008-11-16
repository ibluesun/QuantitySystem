using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Velocity<T> : DerivedQuantity<T>
    {
        public Velocity()
            : base(1, new Length<T>(), new Time<T>(-1))
        {
        }

        public Velocity(int exponent)
            : base(exponent, new Length<T>(exponent), new Time<T>(-1 * exponent))
        {
        }

        public static Velocity<double> LightSpeed
        {
            get
            {
                Velocity<double> v = new Velocity<double>();
                v.Value = 299792458;

                ////assign the unit here  m/s
                //Units.SIUnits.BaseUnits.Metre m = new QuantitySystem.Units.SIUnits.BaseUnits.Metre();

                //Units.Second s = new Units.Second();

                //s = (Units.Second)s.Invert();

                //v.Unit = new Units.SIUnits.DerivedSIUnit(m, s);
                return v;
            }
        }


        public static implicit operator Velocity<T>(T value)
        {
            Velocity<T> Q = new Velocity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
