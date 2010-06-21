using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Energy per unit mass.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecificEnergy<T> : DerivedQuantity<T>
    {

        public SpecificEnergy()
            : base(1, new Energy<T>(), new Mass<T>(-1))
        {
        }


        public SpecificEnergy(float exponent)
            : base(exponent, new Energy<T>(exponent), new Mass<T>(-1 * exponent))
        {
        }


        public static implicit operator SpecificEnergy<T>(T value)
        {
            SpecificEnergy<T> Q = new SpecificEnergy<T>();

            Q.Value = value;

            return Q;
        }

    }
}
