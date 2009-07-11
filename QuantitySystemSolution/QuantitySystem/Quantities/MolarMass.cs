using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class MolarMass<T> : DerivedQuantity<T>
    {
        public MolarMass()
            : base(1, new Mass<T>(), new AmountOfSubstance<T>(-1))
        {
        }

        public MolarMass(float exponent)
            : base(exponent, new Mass<T>(exponent), new AmountOfSubstance<T>(-1 * exponent))
        {
        }

        public static implicit operator MolarMass<T>(T value)
        {
            MolarMass<T> Q = new MolarMass<T>();

            Q.Value = value;

            return Q;
        }

    }
}
