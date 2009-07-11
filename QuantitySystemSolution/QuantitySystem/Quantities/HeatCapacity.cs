using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class HeatCapacity<T> : DerivedQuantity<T>
    {

        public HeatCapacity()
            : base(1, new Energy<T>(), new Temperature<T>(-1))
        {
        }


        public HeatCapacity(float exponent)
            : base(exponent, new Energy<T>(exponent), new Temperature<T>(-1 * exponent))
        {
        }


        public static implicit operator HeatCapacity<T>(T value)
        {
            HeatCapacity<T> Q = new HeatCapacity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
