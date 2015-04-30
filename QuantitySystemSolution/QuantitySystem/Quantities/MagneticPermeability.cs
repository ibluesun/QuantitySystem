using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class MagneticPermeability<T>  : DerivedQuantity<T>
    {
        public MagneticPermeability()
            : base(1, new Inductance<T>(), new Length<T>(-1))
        {
        }

        public MagneticPermeability(float exponent)
            : base(exponent, new Inductance<T>(exponent), new Length<T>(-1 * exponent))
        {
        }


        public static implicit operator MagneticPermeability<T>(T value)
        {
            MagneticPermeability<T> Q = new MagneticPermeability<T>();

            Q.Value = value;

            return Q;
        }

    }
}
