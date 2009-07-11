using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class Angle<T> : DerivedQuantity<T>
    {

        public Angle()
            : base(1, new Length<T>(1, LengthType.Normal), new Length<T>(-1, LengthType.Radius))
        {
        }

        public Angle(float exponent)
            : base(exponent, new Length<T>(exponent, LengthType.Normal), new Length<T>(-1 * exponent, LengthType.Radius))
        {
        }

        //public Angle()
        //    : base(1, new Length<T>(), new RadiusLength<T>(-1))
        //{
        //}

        //public Angle(int exponent)
        //    : base(exponent, new Length<T>(), new RadiusLength<T>(-1))
        //{
        //}


        public static implicit operator Angle<T>(T value)
        {
            Angle<T> Q = new Angle<T>();

            Q.Value = value;

            return Q;
        }


    }
}
