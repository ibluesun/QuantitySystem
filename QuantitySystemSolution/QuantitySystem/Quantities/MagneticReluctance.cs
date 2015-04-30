using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class MagneticReluctance<T>  : DerivedQuantity<T>
    {
        public MagneticReluctance()
            : base(1, new Length<T>(), new Inductance<T>(-1))
        {
        }

        public MagneticReluctance(float exponent)
            : base(exponent, new Length<T>(exponent), new Inductance<T>(-1 * exponent))
        {
        }


        public static implicit operator MagneticReluctance<T>(T value)
        {
            MagneticReluctance<T> Q = new MagneticReluctance<T>();

            Q.Value = value;

            return Q;
        }

    }
}
