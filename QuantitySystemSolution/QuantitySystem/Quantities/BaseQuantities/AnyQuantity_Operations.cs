﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Globalization;

using QuantitySystem.Units;
using System.Diagnostics;

namespace QuantitySystem.Quantities.BaseQuantities
{

    public abstract partial class AnyQuantity<T> : BaseQuantity, ICloneable
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

        #endregion

        #region Quantity By Quantity Operators

        public static AnyQuantity<T> Add(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {

                AnyQuantity<T> AQ = null;
                try
                {
                    AQ = QuantityDimension.QuantityFrom<T>(firstQuantity.Dimension);

                    //exception happen when adding two derived quantities together
                }
                catch (QuantityNotFoundException)
                {
                    //keep the first quantity configuration.
                    AQ = (AnyQuantity<T>)firstQuantity.Clone();
                }

                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;


                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPath stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        //secondVal =  stof.ConversionFactor * secondVal;  //original line without shift

                        secondVal = stof.ConversionFactor * secondVal;

                    }



                    ////sum the values

                    double result = firstVal + secondVal;


                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //assign the unit of first quantity to the result.
                        AQ.Unit = (Unit)firstQuantity.Unit.Clone();
                    }


                    AQ.Value = (T)(object)result;

                    return AQ;

                }
                else
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



                    T firstVal = (firstQuantity.Value);

                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPath stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }



                    ////sum the values

                    T result = (T)method.Invoke(null, new object[] { firstVal, secondVal });

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //assign the unit of first quantity to the result.
                        AQ.Unit = firstQuantity.Unit;
                    }


                    AQ.Value = result;

                    return AQ;
                }

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
                AnyQuantity<T> AQ = null;
                try
                {
                    AQ = QuantityDimension.QuantityFrom<T>(firstQuantity.Dimension);

                    //exception happen when adding two derived quantities together
                }
                catch (QuantityNotFoundException)
                {
                    //keep the first quantity configuration.
                    AQ = (AnyQuantity<T>)firstQuantity.Clone();
                }


                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPath stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        secondVal = stof.ConversionFactor * secondVal;
                    }



                    ////sum the values

                    double result = firstVal - secondVal;

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //assign the unit of first quantity to the result.
                        AQ.Unit = (Unit)firstQuantity.Unit.Clone();
                    }


                    AQ.Value = (T)(object)result;

                    return AQ;
                }
                else
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



                    T firstVal = (firstQuantity.Value);
                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPath stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }


                    T result = (T)method.Invoke(null, new object[] { firstVal, secondVal });


                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //assign the unit of first quantity to the result.
                        AQ.Unit = firstQuantity.Unit;
                    }

                    AQ.Value = result;

                    return AQ;
                }

            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }

        public static AnyQuantity<T> Multiply(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            AnyQuantity<T> qresult = (AnyQuantity<T>)ConstructDerivedQuantity<T>(firstQuantity, secondQuantity);

            try
            {
                //correct quantities and units
                qresult = QuantityDimension.QuantityFrom<T>(qresult.Dimension);

            }
            catch (QuantityNotFoundException)
            {
            }


            if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
            {
                qresult.Value = (T)(object)(((double)(object)firstQuantity.Value) * ((double)(object)secondQuantity.Value));

                if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                {

                    Unit un = new Unit(qresult.GetType(), firstQuantity.Unit, secondQuantity.Unit);
                    qresult.Unit = un;
                    if (un.IsOverflowed) qresult.Value = (T)(object)(un.GetUnitOverflow() * ((double)(object)qresult.Value));
                }
            }
            else
            {

                qresult.Value = MultiplyGenericByGeneric(firstQuantity.Value, secondQuantity.Value);

                //check if any of the two quantities have a valid unit 
                // to be able to derive the current quantity

                if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                {

                    Unit un = new Unit(qresult.GetType(), firstQuantity.Unit, secondQuantity.Unit);
                    qresult.Unit = un;
                    if (un.IsOverflowed) qresult.Value = MultiplyScalarByGeneric(un.GetUnitOverflow(), qresult.Value);


                }
            }


            return qresult;
        }



        public static AnyQuantity<T> Divide(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {


            //to preserve the units we make a derivedquantity with passed quantities
            // then we take the derived unit in temporary store
            // then we try to get strongly typed Quantity
            // then assign the unit to the generated quantity.
            // this way we won't have to know the unit system of the quantity.

            AnyQuantity<T> sec_qty = (AnyQuantity<T>)secondQuantity.Invert(); //this is a divide process we must make 1/exponenet :)


            AnyQuantity<T> qresult = (AnyQuantity<T>)ConstructDerivedQuantity(firstQuantity, sec_qty);


            try
            {
                qresult = QuantityDimension.QuantityFrom<T>(qresult.Dimension);
            }
            catch (QuantityNotFoundException)
            {
            }


            if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
            {
                qresult.Value = (T)(object)(((double)(object)firstQuantity.Value) * ((double)(object)sec_qty.Value));

                if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                {

                    Unit un = new Unit(qresult.GetType(), firstQuantity.Unit, sec_qty.Unit);
                    qresult.Unit = un;
                    if (un.IsOverflowed) qresult.Value = (T)(object)(un.GetUnitOverflow() * ((double)(object)qresult.Value));
                }
            }
            else
            {

                qresult.Value = MultiplyGenericByGeneric(firstQuantity.Value, sec_qty.Value);

                //check if any of the two quantities have a valid unit 
                // to be able to derive the current quantity

                if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                {

                    Unit un = new Unit(qresult.GetType(), firstQuantity.Unit, sec_qty.Unit);
                    qresult.Unit = un;
                    if (un.IsOverflowed) qresult.Value = MultiplyScalarByGeneric(un.GetUnitOverflow(), qresult.Value);

                }
            }

            return qresult;
        }

        public static AnyQuantity<T> Power(AnyQuantity<T> quantity, AnyQuantity<double> exponent)
        {
            if (!exponent.Dimension.IsDimensionless)
            {
                throw new QuantityException("Raising Quantity to a non dimensionless quantity are not implemented");
            }

            
            // and I am ignoring the units conversion also 

            

            
            Unit unit = quantity.Unit.RaiseUnitPower((float)exponent.Value);



            AnyQuantity<T> result = null;

            if (unit.QuantityType != typeof(DerivedQuantity<>))
                result = unit.MakeQuantity<T>(RaiseGenericByScalar(quantity.Value, exponent.Value));
            else
            {
                result = new DerivedQuantity<T>(unit.UnitDimension);
                result.Value = RaiseGenericByScalar(quantity.Value, exponent.Value);
                result.Unit = unit;
            }
            


            /*
             * Old code
            
            //for now I am casting the double to integer 
            int ex = (int)Math.Abs(exponent.Value);

            AnyQuantity<T> q = null;
            if (exponent.Value < 0) //then the number is 1/
                q = (AnyQuantity<T>)quantity.Invert();
            else
                q = quantity;


            if (exponent.Value == 0)
            {
                result = q / q;
            }
            else
            {
                result = q;



                ex--;


                while (ex > 0)
                {
                    result = result * q;
                    ex--;
                }
            }
            */

            Debug.Assert(result.Dimension.Equals(result.Unit.UnitDimension), "Dimensions are not equal after power");
            return result;

        }


        public static AnyQuantity<T> Modulus(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            AnyQuantity<T> qresult = (AnyQuantity<T>)firstQuantity.Clone();


            if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
            {
                qresult.Value = (T)(object)(((double)(object)firstQuantity.Value) % ((double)(object)secondQuantity.Value));
            }
            else
            {

                qresult.Value = ModuloGenericByGeneric(firstQuantity.Value, secondQuantity.Value);
            }

            return qresult;
        }
        #endregion



    }
}