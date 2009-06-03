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

        private Unit QuantityUnit;
        public Unit Unit
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


        public static AnyQuantity<T> operator /(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Divide(firstQuantity, secondQuantity);
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


        #region Generic Helper Calculations
        
        protected static T MultiplyScalarByGeneric(double factor, T value)
        {
            //create the dynamic method here 

            DynamicMethod method = new DynamicMethod(
                "Multiply_Method" + ":" + typeof(T).ToString(),
                typeof(T),
                new Type[] { typeof(double), typeof(T) });


            //get generator to construct the function.

            ILGenerator gen = method.GetILGenerator();


            gen.Emit(OpCodes.Ldarg_0);  //load the first value
            gen.Emit(OpCodes.Ldarg_1);  //load the second value


            if (typeof(T).IsPrimitive)
            {
                gen.Emit(OpCodes.Mul);              //adding them if they were premitive
            }
            else
            {
                MethodInfo info = typeof(T).GetMethod
                    (
                    "op_Multiply",
                    new Type[] { typeof(double), typeof(T) },
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

            }

            gen.Emit(OpCodes.Ret);


            T result = (T)method.Invoke(null, new object[] { factor, value });


            return result;
        }

        protected static T DivideScalarByGeneric(double factor, T value)
        {
            //create the dynamic method here 

            DynamicMethod method = new DynamicMethod(
                "Multiply_Method" + ":" + typeof(T).ToString(),
                typeof(T),
                new Type[] { typeof(double), typeof(T) });


            //get generator to construct the function.

            ILGenerator gen = method.GetILGenerator();


            gen.Emit(OpCodes.Ldarg_0);  //load the first value
            gen.Emit(OpCodes.Ldarg_1);  //load the second value


            if (typeof(T).IsPrimitive)
            {
                gen.Emit(OpCodes.Div);              //adding them if they were premitive
            }
            else
            {
                MethodInfo info = typeof(T).GetMethod
                    (
                    "op_Division",
                    new Type[] { typeof(double), typeof(T) },
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

            }

            gen.Emit(OpCodes.Ret);


            T result = (T)method.Invoke(null, new object[] { factor, value });


            return result;
        }

        protected static T MultiplyGenericByGeneric(T firstVal, T secondVal)
        {
            DynamicMethod method = new DynamicMethod(
                "Multiply_Method" + ":" + typeof(T).ToString(),
                typeof(T),
                new Type[] { typeof(T), typeof(T) });


            //get generator to construct the function.

            ILGenerator gen = method.GetILGenerator();


            gen.Emit(OpCodes.Ldarg_0);  //load the first value
            gen.Emit(OpCodes.Ldarg_1);  //load the second value


            if (typeof(T).IsPrimitive)
            {
                gen.Emit(OpCodes.Mul);              //adding them if they were premitive
            }
            else
            {
                MethodInfo info = typeof(T).GetMethod
                    (
                    "op_Multiply",
                    new Type[] { typeof(T), typeof(T) },
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

            }

            gen.Emit(OpCodes.Ret);


            T result = (T)method.Invoke(null, new object[] { firstVal, secondVal });

            return result;
        }

        #endregion

        public override QuantitySystem.Quantities.BaseQuantities.BaseQuantity Invert()
        {
            AnyQuantity<T> q = (AnyQuantity<T>)base.Invert();


            q.Value = DivideScalarByGeneric(1.0, q.Value);
            
            if (q.Unit != null)
            {
                q.Unit = q.Unit.Invert();
            }

            return q;

        }


        /// <summary>
        /// Parse the input name and return the quantity object from it.
        /// </summary>
        /// <typeparam name="T">container type of the value</typeparam>
        /// <param name="quantityName"></param>
        /// <returns></returns>
        public static AnyQuantity<T> Parse(string quantityName)
        {
            //search in follwing name spaces :)
            string QuantitiesNameSpace = "QuantitySystem.Quantities";
            string BaseQuantitiesNameSpace = QuantitiesNameSpace + ".BaseQuantities";
            string DimensionlessQuantitiesNameSpace = QuantitiesNameSpace + ".DimensionlessQuantities";

            Type QuantityType = Type.GetType(QuantitiesNameSpace + "." + quantityName + "`1");
            if (QuantityType == null) QuantityType = Type.GetType(BaseQuantitiesNameSpace + "." + quantityName + "`1");
            if (QuantityType == null) QuantityType = Type.GetType(DimensionlessQuantitiesNameSpace + "." + quantityName + "`1");

            if (QuantityType == null)
            {
                throw new QuantityNotFoundException();
            }
            else
            {
                QuantityType = QuantityType.MakeGenericType(typeof(T));
                AnyQuantity<T> qty = (AnyQuantity<T>)Activator.CreateInstance(QuantityType);
                return qty;
            }

        }
    }
}
