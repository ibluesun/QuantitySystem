using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units
{
    public interface IUnit
    {

        AnyQuantity CreateThisUnitQuantity();

        string Symbol { get; }

        int Exponent { get; set; }
        bool Negative { get; }
        IUnit Invert();

        /// <summary>
        /// Gets the SI absoulte value after conversion from this unit to SI.
        /// </summary>
        /// <param name="relativeValue"></param>
        /// <returns></returns>
        double GetAbsoluteValue(double relativeValue);

        /// <summary>
        /// Gets relative value of SI absolute value based on current unit.
        /// </summary>
        /// <param name="absoluteValue">SI absolute value.</param>
        /// <returns></returns>
        double GetRelativeValue(double absoluteValue);

        UnitSystem UnitSystem { get; }

        bool IsSpecialName { get; }

        bool IsBaseUnit { get; }


        /// <summary>
        /// multiply units by generating new units
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        IUnit Multiply(IUnit unit);

        IUnit Divide(IUnit unit);

        /// <summary>
        /// add unit return the same unit
        /// </summary>
        /// <param name="unit"></param>
        IUnit Add(IUnit unit);

        IUnit Subtract(IUnit unit);

        IUnit CorrectUnitBy(IUnit unit);
    }
}
