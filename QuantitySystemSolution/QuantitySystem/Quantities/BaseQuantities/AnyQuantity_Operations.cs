using System;
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


        #region Operation Methods

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
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

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
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


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
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

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
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

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

                qresult.Value = DivideGenericByGeneric(firstQuantity.Value, secondQuantity.Value);

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

        /// <summary>
        /// This is a specific raise to double storage quantity power.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public static AnyQuantity<T> Power(AnyQuantity<T> quantity, AnyQuantity<double> exponent)
        {
            if (!exponent.Dimension.IsDimensionless)
            {
                throw new QuantityException("Raising Quantity to a non dimensionless quantity are not implemented", new NotImplementedException());
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

            Debug.Assert(result.Dimension.Equals(result.Unit.UnitDimension), "Dimensions are not equal after power");
            return result;

        }



        /// <summary>
        /// Raising power of the same storage types.
        /// </summary>
        /// <param name="firstQuantity"></param>
        /// <param name="secondQuantity"></param>
        /// <returns></returns>
        public static AnyQuantity<T> Power(AnyQuantity<T> quantity, AnyQuantity<T> exponent)
        {

            if (!(exponent.Dimension.IsDimensionless & quantity.Dimension.IsDimensionless))
            {
                throw new QuantityException("All quantities should be dimensionless, because I don't have a clue about your object power technique", new NotImplementedException());
            }


            AnyQuantity<T> result = new DimensionlessQuantities.DimensionlessQuantity<T>();
            result.Value = RaiseGenericByGeneric(quantity.Value, exponent.Value);
            
            return result;
                
        }

        public static AnyQuantity<T> Mod(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
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


        #region Relation methods
        public static bool LessThan(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {
                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    #region Premitive
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;


                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        //secondVal =  stof.ConversionFactor * secondVal;  //original line without shift

                        secondVal = stof.ConversionFactor * secondVal;

                    }

                    return firstVal < secondVal;
                    #endregion
                }
                else
                {
                    #region Custom Types
                    //define the new dynamically created method
                    //with the return type and the input types

                    DynamicMethod method = new DynamicMethod(
                        "Relation_Method" + ":" + typeof(T).ToString(),
                        typeof(bool),
                        new Type[] { typeof(T), typeof(T) });


                    //get generator to construct the function.

                    ILGenerator gen = method.GetILGenerator();


                    gen.Emit(OpCodes.Ldarg_0);  //load the first value
                    gen.Emit(OpCodes.Ldarg_1);  //load the second value


                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_LessThan",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);


                    gen.Emit(OpCodes.Ret);



                    T firstVal = (firstQuantity.Value);

                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }

                    return (bool)method.Invoke(null, new object[] { firstVal, secondVal });
                    #endregion

                }
            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }
        public static bool LessThanOrEqual(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {
                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    #region Premitive
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;


                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        //secondVal =  stof.ConversionFactor * secondVal;  //original line without shift

                        secondVal = stof.ConversionFactor * secondVal;

                    }

                    return firstVal <= secondVal;
                    #endregion
                }
                else
                {
                    #region Custom Types
                    //define the new dynamically created method
                    //with the return type and the input types

                    DynamicMethod method = new DynamicMethod(
                        "Relation_Method" + ":" + typeof(T).ToString(),
                        typeof(bool),
                        new Type[] { typeof(T), typeof(T) });


                    //get generator to construct the function.

                    ILGenerator gen = method.GetILGenerator();


                    gen.Emit(OpCodes.Ldarg_0);  //load the first value
                    gen.Emit(OpCodes.Ldarg_1);  //load the second value


                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_LessThanOrEqual",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);


                    gen.Emit(OpCodes.Ret);



                    T firstVal = (firstQuantity.Value);

                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }

                    return (bool)method.Invoke(null, new object[] { firstVal, secondVal });
                    #endregion

                }
            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }
        public static bool GreaterThan(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {
                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    #region Premitive
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;


                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        //secondVal =  stof.ConversionFactor * secondVal;  //original line without shift

                        secondVal = stof.ConversionFactor * secondVal;

                    }

                    return firstVal > secondVal;
                    #endregion
                }
                else
                {
                    #region Custom Types
                    //define the new dynamically created method
                    //with the return type and the input types

                    DynamicMethod method = new DynamicMethod(
                        "Relation_Method" + ":" + typeof(T).ToString(),
                        typeof(bool),
                        new Type[] { typeof(T), typeof(T) });


                    //get generator to construct the function.

                    ILGenerator gen = method.GetILGenerator();


                    gen.Emit(OpCodes.Ldarg_0);  //load the first value
                    gen.Emit(OpCodes.Ldarg_1);  //load the second value


                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_GreaterThan",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);


                    gen.Emit(OpCodes.Ret);



                    T firstVal = (firstQuantity.Value);

                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }

                    return (bool)method.Invoke(null, new object[] { firstVal, secondVal });
                    #endregion

                }
            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }
        public static bool GreaterThanOrEqual(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {
                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    #region Premitive
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;


                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        //secondVal =  stof.ConversionFactor * secondVal;  //original line without shift

                        secondVal = stof.ConversionFactor * secondVal;

                    }

                    return firstVal >= secondVal;
                    #endregion
                }
                else
                {
                    #region Custom Types
                    //define the new dynamically created method
                    //with the return type and the input types

                    DynamicMethod method = new DynamicMethod(
                        "Relation_Method" + ":" + typeof(T).ToString(),
                        typeof(bool),
                        new Type[] { typeof(T), typeof(T) });


                    //get generator to construct the function.

                    ILGenerator gen = method.GetILGenerator();


                    gen.Emit(OpCodes.Ldarg_0);  //load the first value
                    gen.Emit(OpCodes.Ldarg_1);  //load the second value


                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_GreaterThanOrEqual",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);


                    gen.Emit(OpCodes.Ret);



                    T firstVal = (firstQuantity.Value);

                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }

                    return (bool)method.Invoke(null, new object[] { firstVal, secondVal });
                    #endregion

                }
            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }
        public static bool Equality(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {
                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    #region Premitive
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;


                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        //secondVal =  stof.ConversionFactor * secondVal;  //original line without shift

                        secondVal = stof.ConversionFactor * secondVal;

                    }

                    return firstVal == secondVal;
                    #endregion
                }
                else
                {
                    #region Custom Types
                    //define the new dynamically created method
                    //with the return type and the input types

                    DynamicMethod method = new DynamicMethod(
                        "Relation_Method" + ":" + typeof(T).ToString(),
                        typeof(bool),
                        new Type[] { typeof(T), typeof(T) });


                    //get generator to construct the function.

                    ILGenerator gen = method.GetILGenerator();


                    gen.Emit(OpCodes.Ldarg_0);  //load the first value
                    gen.Emit(OpCodes.Ldarg_1);  //load the second value


                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_Equality",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);


                    gen.Emit(OpCodes.Ret);



                    T firstVal = (firstQuantity.Value);

                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }

                    return (bool)method.Invoke(null, new object[] { firstVal, secondVal });
                    #endregion

                }
            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }
        public static bool Inequality(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            if (firstQuantity.Equals(secondQuantity))
            {
                if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
                {
                    #region Premitive
                    //use direct calculations

                    double firstVal = (double)(object)firstQuantity.Value;

                    double secondVal = (double)(object)secondQuantity.Value;


                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);

                        //secondVal =  stof.ConversionFactor * secondVal;  //original line without shift

                        secondVal = stof.ConversionFactor * secondVal;

                    }

                    return firstVal != secondVal;
                    #endregion
                }
                else
                {
                    #region Custom Types
                    //define the new dynamically created method
                    //with the return type and the input types

                    DynamicMethod method = new DynamicMethod(
                        "Relation_Method" + ":" + typeof(T).ToString(),
                        typeof(bool),
                        new Type[] { typeof(T), typeof(T) });


                    //get generator to construct the function.

                    ILGenerator gen = method.GetILGenerator();


                    gen.Emit(OpCodes.Ldarg_0);  //load the first value
                    gen.Emit(OpCodes.Ldarg_1);  //load the second value


                    MethodInfo info = typeof(T).GetMethod
                        (
                        "op_Inequality",
                        new Type[] { typeof(T), typeof(T) },
                        null
                        );

                    gen.EmitCall(OpCodes.Call, info, null);


                    gen.Emit(OpCodes.Ret);



                    T firstVal = (firstQuantity.Value);

                    T secondVal = (secondQuantity.Value);

                    //correct the values according to left unit or first unit.
                    //the resulted quantity has the values of the first unit.

                    if (firstQuantity.Unit != null && secondQuantity.Unit != null)
                    {
                        //factor from second unit to first unit
                        UnitPathStack stof = secondQuantity.Unit.PathToUnit(firstQuantity.Unit);


                        secondVal = MultiplyScalarByGeneric(stof.ConversionFactor, secondVal);
                    }

                    return (bool)method.Invoke(null, new object[] { firstVal, secondVal });
                    #endregion

                }
            }
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }

        #endregion
        #endregion



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

        public static AnyQuantity<T> operator %(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Mod(firstQuantity, secondQuantity);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static AnyQuantity<T> operator ^(AnyQuantity<T> quantity, AnyQuantity<double> exponent)
        {
            return Power(quantity, exponent);
        }


        #region Relations Operators
        public static bool operator <(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return LessThan(firstQuantity, secondQuantity);
        }

        public static bool operator <=(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return LessThanOrEqual(firstQuantity, secondQuantity);
        }

        public static bool operator >(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return GreaterThan(firstQuantity, secondQuantity);
        }

        public static bool operator >=(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return GreaterThanOrEqual(firstQuantity, secondQuantity);
        }

        public static bool operator ==(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Equality(firstQuantity, secondQuantity);
        }

        public static bool operator !=(AnyQuantity<T> firstQuantity, AnyQuantity<T> secondQuantity)
        {
            return Inequality(firstQuantity, secondQuantity);
        }
        #endregion

        #endregion


    }
}