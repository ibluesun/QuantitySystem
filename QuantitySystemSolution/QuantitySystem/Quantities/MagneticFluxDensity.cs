

namespace QuantitySystem.Quantities
{
    public class MagneticFluxDensity<T>  : DerivedQuantity<T>
    {
        public MagneticFluxDensity()
            : base(1, new MagneticFlux<T>(), new Area<T>(-1))
        {
        }

        public MagneticFluxDensity(float exponent)
            : base(exponent, new MagneticFlux<T>(exponent), new Area<T>(-1 * exponent))
        {
        }


        public static implicit operator MagneticFluxDensity<T>(T value)
        {
            MagneticFluxDensity<T> Q = new MagneticFluxDensity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
