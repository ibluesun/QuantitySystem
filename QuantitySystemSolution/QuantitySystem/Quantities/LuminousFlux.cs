using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;


namespace QuantitySystem.Quantities
{
    public class LuminousFlux<T> : DerivedQuantity<T>
    {
        public LuminousFlux()
            : base(1, new LuminousIntensity<T>(), new SolidAngle<T>())
        {
        }

        public LuminousFlux(float exponent)
            : base(exponent, new LuminousIntensity<T>(exponent), new SolidAngle<T>(exponent))
        {
        }


        public static implicit operator LuminousFlux<T>(T value)
        {
            LuminousFlux<T> Q = new LuminousFlux<T>();

            Q.Value = value;

            return Q;
        }

    }
}
