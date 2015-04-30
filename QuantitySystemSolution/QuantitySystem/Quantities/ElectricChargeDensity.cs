

namespace QuantitySystem.Quantities
{
    public class ElectricChargeDensity<T> : DerivedQuantity<T>
    {
        public ElectricChargeDensity()
            : base(1, new ElectricCharge<T>(), new Volume<T>(-1))
        {
        }

        public ElectricChargeDensity(float exponent)
            : base(exponent, new ElectricCharge<T>(exponent), new Volume<T>(-1 * exponent))
        {
        }


        public static implicit operator ElectricChargeDensity<T>(T value)
        {
            ElectricChargeDensity<T> Q = new ElectricChargeDensity<T>();

            Q.Value = value;

            return Q;
        }


    }
}
