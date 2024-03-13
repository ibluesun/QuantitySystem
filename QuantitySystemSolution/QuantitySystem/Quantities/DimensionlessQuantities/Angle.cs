using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class Angle<T> : DerivedQuantity<T>
    {

        public Angle()
            :base(1, new Length<T>(), new Displacement<T>(-1))
        {
        }

        public Angle(float exponent)
            : base(exponent, new Length<T>(exponent), new Displacement<T>(-1 * exponent))
        {
        }


        public static implicit operator Angle<T>(T value)
        {
            Angle<T> Q = new Angle<T>();

            Q.Value = value;

            return Q;
        }


    }
}
