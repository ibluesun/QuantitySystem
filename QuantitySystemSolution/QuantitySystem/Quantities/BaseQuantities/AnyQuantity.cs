using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Globalization;

using QuantitySystem.Units;

namespace QuantitySystem.Quantities.BaseQuantities
{
    /// <summary>
    /// This class hold the mathmatical operations of quantity.
    /// </summary>
    public abstract partial class AnyQuantity<T> : BaseQuantity
    {
        #region constructors
        protected AnyQuantity() : base(1) { }
        protected AnyQuantity(int exponent) : base(exponent) { }
        #endregion

        #region Value & Unit

        private T QuantityValue;

        public T Value
        {
            get { return QuantityValue; }
            set { QuantityValue = value; }
        }

        private IUnit QuantityUnit;
        public IUnit Unit
        {
            get { return QuantityUnit; }
            set { QuantityUnit = value; }
        }

        public override string ToString()
        {
            return this.GetType().Name + ": " + Value.ToString() + " " + (Unit != null ? Unit.Symbol : "");
        }
        #endregion


        #region Quantity Operations


        #region Overloaded Operators        



        public static AnyQuantity<T> operator +(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Add(firstQuantity, secondQuantity);
        }



        public static AnyQuantity<T> operator -(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Subtract(firstQuantity, secondQuantity);
        }


        public static AnyQuantity<T> operator *(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Multiply(firstQuantity, secondQuantity);
        }



        public static AnyQuantity<T> operator *(AnyQuantity<T> quantity, T value)
        {
            return Multiply(quantity, value);
        }


        public static AnyQuantity<T> operator /(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Divide(firstQuantity, secondQuantity);
        }



        public static AnyQuantity<T> operator /(AnyQuantity<T> quantity, T value)
        {
            return Divide(quantity, value);
        }

        #endregion


        #endregion


        #region Helper Functions
        public static DerivedQuantity<T> ConstructDerivedQuantity<T>(params AnyQuantity<T>[] quantities)
        {
            DerivedQuantity<T> DQ = new DerivedQuantity<T>(1, quantities);
            return DQ;
        }
       #endregion

        
    }
}
