

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class CatalyticActivity<T> : DerivedQuantity<T>
    {
        public CatalyticActivity()
            : base(1, new AmountOfSubstance<T>(), new Time<T>(-1))
        {
        }

        public CatalyticActivity(float exponent)
            : base(exponent, new AmountOfSubstance<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator CatalyticActivity<T>(T value)
        {
            CatalyticActivity<T> Q = new CatalyticActivity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
