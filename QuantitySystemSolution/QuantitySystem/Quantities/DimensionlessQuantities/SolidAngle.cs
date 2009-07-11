using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class SolidAngle<T> : DerivedQuantity<T>
    {
        public SolidAngle()
            : base(1, new Angle<T>(2))
        {
        }

        public SolidAngle(float exponent)
            : base(exponent, new Angle<T>(2 * exponent))
        {
        }

        public static implicit operator SolidAngle<T>(T value)
        {
            SolidAngle<T> Q = new SolidAngle<T>();

            Q.Value = value;

            return Q;
        }


    }
}
