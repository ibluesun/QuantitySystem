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
    public abstract partial class AnyQuantity<T> : BaseQuantity, ICloneable
    {

        #region constructors

        protected AnyQuantity() : base(1) { }
        protected AnyQuantity(float exponent) : base(exponent) { }


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
            string qname = this.GetType().Name;
            qname = qname.Substring(0, qname.Length - 2);

            return qname + ": " + Value.ToString() + " " + (Unit != null ? Unit.Symbol : "");
        }

        public string ToShortString()
        {
            
            string un = string.Empty;
            if (Unit != null)
            {
                un = Unit.Symbol.Trim();

                if (un[0] != '<') un = "<" + un + ">"; 
            }

            return Value.ToString() + " " + un;
        
        }

        #endregion




        #region Helper Functions
        public static DerivedQuantity<T> ConstructDerivedQuantity<T>(params AnyQuantity<T>[] quantities)
        {
            DerivedQuantity<T> DQ = new DerivedQuantity<T>(1, quantities);
            return DQ;
        }
       #endregion


        #region Generic Helper Calculations
        
        public static T MultiplyScalarByGeneric(double factor, T value)
        {
            if (factor == 1.0) return value;

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

        public static T DivideScalarByGeneric(double factor, T value)
        {
            if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
            {
                return (T)(object)(factor / (double)(object)value);
            }
            else
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
        }

        public static T MultiplyGenericByGeneric(T firstValue, T secondValue)
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


            T result = (T)method.Invoke(null, new object[] { firstValue, secondValue });

            return result;
        }

        public static T DivideGenericByGeneric(T firstValue, T secondValue)
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
                gen.Emit(OpCodes.Div);              //adding them if they were premitive
            }
            else
            {
                MethodInfo info = typeof(T).GetMethod
                    (
                    "op_Division",
                    new Type[] { typeof(T), typeof(T) },
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

            }

            gen.Emit(OpCodes.Ret);


            T result = (T)method.Invoke(null, new object[] { firstValue, secondValue });

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static T RaiseGenericByScalar(T value, double factor)
        {
            if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
            {
                return (T)(object)(Math.Pow((double)(object)value, factor));

            }
            else
            {
                //create the dynamic method here 

                DynamicMethod method = new DynamicMethod(
                    "Power_Method" + ":" + typeof(T).ToString(),
                    typeof(T),
                    new Type[] { typeof(double), typeof(T) });


                //get generator to construct the function.

                ILGenerator gen = method.GetILGenerator();


                gen.Emit(OpCodes.Ldarg_0);  //load the first value
                gen.Emit(OpCodes.Ldarg_1);  //load the second value


                if (typeof(T).IsPrimitive)
                {

                    MethodInfo info = typeof(Math).GetMethod
                        (
                        "Pow",
                        new Type[] { typeof(double), typeof(double) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

                }
                else
                {
                    MethodInfo info = typeof(T).GetMethod
                        (
                        "Power",
                        new Type[] { typeof(T), typeof(double) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

                }

                gen.Emit(OpCodes.Ret);


                T result = (T)method.Invoke(null, new object[] { value, factor });


                return result;
            }
        }


        /// <summary>
        /// Raise power of generic to generic 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static T RaiseGenericByGeneric(T value, T factor)
        {
            //create the dynamic method here 

            DynamicMethod method = new DynamicMethod(
                "Power_Method" + ":" + typeof(T).ToString(),
                typeof(T),
                new Type[] { typeof(T), typeof(T) });

            //get generator to construct the function.

            ILGenerator gen = method.GetILGenerator();

            gen.Emit(OpCodes.Ldarg_0);  //load the first value
            gen.Emit(OpCodes.Ldarg_1);  //load the second value

            if (typeof(T).IsPrimitive)
            {

                MethodInfo info = typeof(Math).GetMethod
                    (
                    "Pow",
                    new Type[] { typeof(double), typeof(double) },
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   

            }
            else
            {
                MethodInfo info = typeof(T).GetMethod
                    (
                    "Pow",
                    new Type[] { typeof(T), typeof(T) },
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   

            }

            gen.Emit(OpCodes.Ret);

            T result = (T)method.Invoke(null, new object[] { value, factor });

            return result;


        }



        /// <summary>
        /// Remainder of two generic objects  a % b
        /// </summary>
        /// <param name="firstValue"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static T ModuloGenericByGeneric(T firstValue, T secondValue)
        {
            DynamicMethod method = new DynamicMethod(
                "Modulo_Method" + ":" + typeof(T).ToString(),
                typeof(T),
                new Type[] { typeof(T), typeof(T) });


            //get generator to construct the function.

            ILGenerator gen = method.GetILGenerator();


            gen.Emit(OpCodes.Ldarg_0);  //load the first value
            gen.Emit(OpCodes.Ldarg_1);  //load the second value


            if (typeof(T).IsPrimitive)
            {
                gen.Emit(OpCodes.Rem);              //adding them if they were premitive
            }
            else
            {
                MethodInfo info = typeof(T).GetMethod
                    (
                    "op_Modulus",
                    new Type[] { typeof(T), typeof(T) },
                    null
                    );

                gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

            }

            gen.Emit(OpCodes.Ret);


            T result = (T)method.Invoke(null, new object[] { firstValue, secondValue });

            return result;
        }

        #endregion

        public override QuantitySystem.Quantities.BaseQuantities.BaseQuantity Invert()
        {
            AnyQuantity<T> q = (AnyQuantity<T>)base.Invert();


            q.Value = DivideScalarByGeneric(1.0, q.Value);
            
            if (q.Unit != null)
            {
                q.Unit = this.Unit.Invert();
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


        #region ICloneable Members

        public object Clone()
        {
            object t = this.MemberwiseClone();
            var t2 = ((AnyQuantity<T>)t);
            if (t2.Unit != null) t2.Unit = (Unit)Unit.Clone();
            return t2;
        
        }

        #endregion
    }
}
