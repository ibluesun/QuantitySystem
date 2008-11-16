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

    partial class AnyQuantity<T>
    {

        #region Strongly Typed Operations

        /*
        public static TQuantity Add<TQuantity>(TQuantity firstQuantity, TQuantity secondQuantity) where TQuantity : AnyQuantity, new()
        {
            TQuantity AQ = new TQuantity();


            //convert values to Absolute values

            double firstVal = firstQuantity.Unit.GetAbsoluteValue(firstQuantity.Value);
            double secondVal = secondQuantity.Unit.GetAbsoluteValue(secondQuantity.Value);

            //sum the values

            double val = firstVal + secondVal;

            //assign the unit of first quantity to the result.
            AQ.Unit = firstQuantity.Unit;

            //get relative value based on first quantity unit


            AQ.Value = AQ.Unit.GetRelativeValue(val);



            return AQ;
        }

        public static TQuantity Subtract<TQuantity>(TQuantity firstQuantity, TQuantity secondQuantity) where TQuantity : AnyQuantity, new()
        {
            TQuantity AQ = new TQuantity();
            //convert values to Absolute values

            double firstVal = firstQuantity.Unit.GetAbsoluteValue(firstQuantity.Value);
            double secondVal = secondQuantity.Unit.GetAbsoluteValue(secondQuantity.Value);

            //sum the values

            double val = firstVal - secondVal;

            //assign the unit of first quantity to the result.
            AQ.Unit = firstQuantity.Unit;

            //get relative value based on first quantity unit

            AQ.Value = AQ.Unit.GetRelativeValue(val);

            return AQ;
        }
        */



        public static AnyQuantity<T> Add(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {

                //define the new dynamically created method
                //with the return type and the input types

                DynamicMethod method = new DynamicMethod(
                    "Add_Method" + ":" + typeof(T).ToString(),
                    typeof(T),
                    new Type[] { typeof(T), typeof(T) });


                //get generator to construct the function.

                ILGenerator gen = method.GetILGenerator();


                gen.Emit(OpCodes.Ldarg_0);  //load the first value
                gen.Emit(OpCodes.Ldarg_1);  //load the second value


                if (typeof(T).IsPrimitive)
                {
                    gen.Emit(OpCodes.Add);              //adding them if they were premitive
                }
                else
                {
                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_Addition",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

                }

                gen.Emit(OpCodes.Ret);

                AnyQuantity<T> AQ = QuantityDimension.QuantityFrom<T>(firstQuantity.Dimension);

                ////convert values to Absolute values  {explicitly speaking SI values}


                //double firstVal = firstQuantity.Unit.GetAbsoluteValue(firstQuantity.Value);                
                T firstVal = (firstQuantity.Value);

                //double secondVal = secondQuantity.Unit.GetAbsoluteValue(secondQuantity.Value);
                T secondVal = (secondQuantity.Value);


                ////sum the values

                //double val = firstVal + secondVal;
                T result = (T)method.Invoke(null, new object[] { firstVal, secondVal });


                ////assign the unit of first quantity to the result.
                //AQ.Unit = firstQuantity.Unit;

                ////get relative value based on first quantity unit


                //AQ.Value = AQ.Unit.GetRelativeValue(val);
                AQ.Value = result;



                return AQ;

            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }


        public static AnyQuantity<T> Subtract(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {
                //define the new dynamically created method
                //with the return type and the input types

                DynamicMethod method = new DynamicMethod(
                    "Subtract_Method" + ":" + typeof(T).ToString(),
                    typeof(T),
                    new Type[] { typeof(T), typeof(T) });


                //get generator to construct the function.

                ILGenerator gen = method.GetILGenerator();


                gen.Emit(OpCodes.Ldarg_0);  //load the first value
                gen.Emit(OpCodes.Ldarg_1);  //load the second value


                if (typeof(T).IsPrimitive)
                {
                    gen.Emit(OpCodes.Sub);              //adding them if they were premitive
                }
                else
                {
                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_Subtraction",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);   //otherwise call its op_Addition method.

                }

                gen.Emit(OpCodes.Ret);


                AnyQuantity<T> AQ = QuantityDimension.QuantityFrom<T>(firstQuantity.Dimension);

                ////convert values to Absolute values
                //double firstVal = firstQuantity.Unit.GetAbsoluteValue(firstQuantity.Value);
                T firstVal = (firstQuantity.Value);


                //double secondVal = secondQuantity.Unit.GetAbsoluteValue(secondQuantity.Value);
                T secondVal = (secondQuantity.Value);

                ////sum the values

                //double val = firstVal - secondVal;
                T result = (T)method.Invoke(null, new object[] { firstVal, secondVal });

                ////assign the unit of first quantity to the result.
                //AQ.Unit = firstQuantity.Unit;

                ////get relative value based on first quantity unit

                //AQ.Value = AQ.Unit.GetRelativeValue(val);
                AQ.Value = result;

                return AQ;

            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }

        public static AnyQuantity<T> Multiply(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            AnyQuantity<T> qresult = (AnyQuantity<T>)ConstructDerivedQuantity<T>(firstQuantity, secondQuantity);

            ////deduce the unit from calculation.
            //IUnit qunit = firstQuantity.Unit.Multiply(secondQuantity.Unit);
            //qresult.Unit = qunit; //assign it to the quantity generated.

            try
            {
                //correct quantities and units
                qresult = QuantityDimension.QuantityFrom<T>(qresult.Dimension);

                ////get the pure unit of the quantity
                //IUnit unit = GlobalUnitSystem.UnitOf(qresult, firstQuantity.Unit.UnitSystem);

                ////take the prefix of calculated unit
                //// and associate it to the prefix of predicted unit.


                //qresult.Unit = unit.CorrectUnitBy(qunit);

            }
            catch (QuantityNotFoundException)
            {
            }


            //define the new dynamically created method
            //with the return type and the input types

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


            //qresult.Value = firstQuantity.Value * secondQuantity.Value;

            T result = (T)method.Invoke(null, new object[] { firstQuantity.Value, secondQuantity.Value });

            qresult.Value = result;


            return qresult;
        }


        public static AnyQuantity<T> Multiply(AnyQuantity<T> quantity, T value)
        {
            AnyQuantity<T> q = (AnyQuantity<T>)quantity.MemberwiseClone();

            //q.Value *= value;

            return q;

        }

        public static AnyQuantity<T> Divide(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {


            //to preserve the units we make a derivedquantity with passed quantities
            // then we take the derived unit in temporary store
            // then we try to get strongly typed Quantity
            // then assign the unit to the generated quantity.
            // this way we won't have to know the unit system of the quantity.

            AnyQuantity<T> sec_qty = (AnyQuantity<T>)secondQuantity.Invert(); //this is a divide process we must make 1/exponenet :)

            //QuantityDimension dim = firstQuantity.Dimension - secondQuantity.Dimension;
            //AnyQuantity qresult = (AnyQuantity)QuantityDimension.QuantityFrom(dim);

            AnyQuantity<T> qresult = (AnyQuantity<T>)ConstructDerivedQuantity(firstQuantity, sec_qty);


            //IUnit qunit = firstQuantity.Unit.Divide(secondQuantity.Unit);
            //qresult.Unit = qunit;

            try
            {
                qresult = QuantityDimension.QuantityFrom<T>(qresult.Dimension);


                //IUnit unit = GlobalUnitSystem.UnitOf(qresult, firstQuantity.Unit.UnitSystem);

                ////take the prefix of calculated unit
                //// and associate it to the prefix of predicted unit.


                //qresult.Unit = unit.CorrectUnitBy(qunit);



            }
            catch (QuantityNotFoundException)
            {
            }

            //define the new dynamically created method
            //with the return type and the input types

            DynamicMethod method = new DynamicMethod(
                "Divide_Method" + ":" + typeof(T).ToString(),
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


            //qresult.Value = firstQuantity.Value / secondQuantity.Value;
            T result = (T)method.Invoke(null, new object[] { firstQuantity.Value, secondQuantity.Value });

            qresult.Value = result;

            return qresult;
        }

        public static AnyQuantity<T> Divide(AnyQuantity<T> quantity, T value)
        {
            AnyQuantity<T> q = (AnyQuantity<T>)quantity.MemberwiseClone();

            //q.Value /= value;

            return q;

        }

        #endregion

    }
}