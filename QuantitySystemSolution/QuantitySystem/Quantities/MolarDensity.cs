using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Moles per unit volume
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MolarDensity<T> : DerivedQuantity<T>
    {

        public MolarDensity()
            : base(1, new AmountOfSubstance<T>(), new Volume<T>(-1))
        {
        }

        public MolarDensity(float exponent)
            : base(exponent, new AmountOfSubstance<T>(exponent), new Volume<T>(-1 * exponent))
        {
        }


        public static implicit operator MolarDensity<T>(T value)
        {
            MolarDensity<T> Q = new MolarDensity<T>();

            Q.Value = value;

            return Q;
        }


    }
}
