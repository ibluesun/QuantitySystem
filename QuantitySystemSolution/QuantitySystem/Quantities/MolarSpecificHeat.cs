using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Energy per molar unit x termperature unit
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MolarSpecificHeat<T> : DerivedQuantity<T>
    {

        public MolarSpecificHeat()
            : base(1, new Energy<T>(), new AmountOfSubstance<T>(-1), new Temperature<T>(-1))
        {
        }


        public MolarSpecificHeat(float exponent)
            : base(exponent, new Energy<T>(exponent), new AmountOfSubstance<T>(-1 * exponent), new Temperature<T>(-1 * exponent))
        {
        }


        public static implicit operator MolarSpecificHeat<T>(T value)
        {
            MolarSpecificHeat<T> Q = new MolarSpecificHeat<T>();

            Q.Value = value;

            return Q;
        }

    }
}
