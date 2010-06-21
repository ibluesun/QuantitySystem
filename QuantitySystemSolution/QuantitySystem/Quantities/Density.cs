using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Mass per unit volume
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Density<T> : DerivedQuantity<T>
    {

        public Density()
            : base(1, new Mass<T>(), new Volume<T>(-1))
        {
        }

        public Density(float exponent)
            : base(exponent, new Mass<T>(exponent), new Volume<T>(-1 * exponent))
        {
        }


        public static implicit operator Density<T>(T value)
        {
            Density<T> Q = new Density<T>();

            Q.Value = value;

            return Q;
        }


    }
}
