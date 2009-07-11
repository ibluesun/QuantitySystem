using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class ElectromotiveForce<T> : DerivedQuantity<T>
    {
        public ElectromotiveForce()
            : base(1, new Power<T>(), new ElectricalCurrent<T>(-1))
        {
        }

        public ElectromotiveForce(float exponent)
            : base(exponent, new Power<T>(exponent), new ElectricalCurrent<T>(-1 * exponent))
        {
        }


        public static implicit operator ElectromotiveForce<T>(T value)
        {
            ElectromotiveForce<T> Q = new ElectromotiveForce<T>();

            Q.Value = value;

            return Q;
        }

    }
}