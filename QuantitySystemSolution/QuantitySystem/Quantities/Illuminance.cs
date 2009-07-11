using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Quantities
{
    public class Illuminance<T> : DerivedQuantity<T>
    {
        public Illuminance()
            : base(1, new LuminousFlux<T>(), new Area<T>(-1))
        {
        }

        public Illuminance(float exponent)
            : base(exponent, new LuminousFlux<T>(exponent), new Area<T>(-1 * exponent))
        {
        }


        public static implicit operator Illuminance<T>(T value)
        {
            Illuminance<T> Q = new Illuminance<T>();

            Q.Value = value;

            return Q;
        }

    }
}
