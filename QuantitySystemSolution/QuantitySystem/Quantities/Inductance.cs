using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Inductance <T>  : DerivedQuantity<T>
    {
        public Inductance()
            : base(1, new MagneticFlux<T>(), new ElectricalCurrent<T>(-1))
        {
        }

        public Inductance(int exponent)
            : base(exponent, new MagneticFlux<T>(exponent), new ElectricalCurrent<T>(-1 * exponent))
        {
        }


        public static implicit operator Inductance<T>(T value)
        {
            Inductance<T> Q = new Inductance<T>();

            Q.Value = value;

            return Q;
        }

    }
}
