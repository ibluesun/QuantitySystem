using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// The volume occupied by one mole of a substance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MolarVolume<T> : DerivedQuantity<T>
    {

        public MolarVolume()
            : base(1, new Volume<T>(1), new AmountOfSubstance<T>(-1))
        {
        }

        public MolarVolume(float exponent)
            : base(exponent, new Volume<T>(exponent), new AmountOfSubstance<T>(-1 * exponent))
        {
        }


        public static implicit operator MolarVolume<T>(T value)
        {
            MolarVolume<T> Q = new MolarVolume<T>();

            Q.Value = value;

            return Q;
        }

    }
}
