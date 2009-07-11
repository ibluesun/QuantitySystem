using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class ElectricConductance<T> : DerivedQuantity<T>
    {
        public ElectricConductance()
            : base(1, new ElectricalCurrent<T>(), new ElectromotiveForce<T>(-1))
        {
        }

        public ElectricConductance(float exponent)
            : base(exponent, new ElectricalCurrent<T>(exponent), new ElectromotiveForce<T>(-1 * exponent))
        {
        }


        public static implicit operator ElectricConductance<T>(T value)
        {
            ElectricConductance<T> Q = new ElectricConductance<T>();

            Q.Value = value;

            return Q;
        }

    }
}
