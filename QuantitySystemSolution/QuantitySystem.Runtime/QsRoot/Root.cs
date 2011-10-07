using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Qs.Types;
using Qs;
using System.Linq.Expressions;
using QuantitySystem.Quantities.BaseQuantities;
using System.Runtime.CompilerServices;

namespace QsRoot
{
    /// <summary>
    /// Root Class contains built in functions for quantity system execution.
    /// </summary>
    public class Root
    {
        
        public static void LoadLibrary(string library)
        {
            Assembly a = Assembly.LoadFrom(library);
        }

        static bool allrefloaded = false;

        /// <summary>
        /// Get External Library Type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Type GetExternalType(string type)
        {
            if (!allrefloaded)
            {
                var ss = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
                foreach (var vo in ss) Assembly.Load(vo);
                allrefloaded = true;
            }

            var allassemblies = AppDomain.CurrentDomain.GetAssemblies();

            System.Text.RegularExpressions.Regex.Match("", "");

            Type dt = null;
            foreach (var a in allassemblies)
            {
                dt = a.GetType(type, false, true);

                if (dt != null) break;
            }
            
            return dt;
        }

        internal static object[] QsParametersToNativeValues(MethodInfo method, params QsParameter[] parameters)
        {
            List<object> NativeParameters = new List<object>();

            int iy = 0;
            ParameterInfo[] paramInfos = method.GetParameters();

            foreach (var p in parameters)
            {
                if (p.QsNativeValue == null)
                {
                    NativeParameters.Add(null);
                }
                else if (p.QsNativeValue is QsScalar)
                {
                    var scalar = (QsScalar)p.QsNativeValue;
                    object nativeValue = System.Convert.ChangeType(scalar.NumericalQuantity.Value, paramInfos[iy].ParameterType);
                    NativeParameters.Add(nativeValue);
                }
                else if (p.QsNativeValue is QsText)
                {
                    NativeParameters.Add(((QsText)p.QsNativeValue).Text);
                }
                else if (p.QsNativeValue is QsVector)
                {

                    if (paramInfos[iy].ParameterType.IsArray)
                    {

                        QsVector vec = (QsVector)p.QsNativeValue;
                        System.Type ArrayType = System.Type.GetType(paramInfos[iy].ParameterType.FullName.Trim('[', ']'));
                        System.Array arr = System.Array.CreateInstance(ArrayType, vec.Count);
                        for (int i = 0; i < vec.Count; i++)
                        {
                            object val = System.Convert.ChangeType(vec[i].NumericalQuantity.Value, ArrayType);
                            arr.SetValue(val, i);
                        }

                        NativeParameters.Add(arr);
                    }
                    else
                    {
                        throw new QsException("The target parameter is not an array");
                    }
                }
                else
                {
                    throw new QsException("Converting Qs values other than scalars to native function is not supported.");
                }

                iy++;

            }

            return NativeParameters.ToArray();
        }



        /// <summary>
        /// Created to be used for the retuen values of the function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        internal static QsValue NativeToQsConvert(object value)
        {
            if (value == null) return null;

            Type vType = value.GetType();

            if (vType == typeof(int) || vType == typeof(double) || vType == typeof(Single) || vType == typeof(float) || vType == typeof(Int64) || vType == typeof(uint) || vType == typeof(UInt16) || vType == typeof(UInt64))
            {
                double vv = (double)System.Convert.ChangeType(value, typeof(double));
                return new QsScalar
                {
                    NumericalQuantity = vv.ToQuantity()
                };
            }

            if (vType == typeof(String))
                return new QsText((string)value);

            if (vType.BaseType == typeof(QsValue)) return (QsValue)value;

            if (vType.BaseType == typeof(AnyQuantity<double>)) return new QsScalar { NumericalQuantity = (AnyQuantity<double>)value };
            
            if (vType == typeof(int[]) || vType == typeof(double[]) || vType == typeof(Single[]) || vType == typeof(float[]) || vType == typeof(Int64[]) || vType == typeof(uint[]) || vType == typeof(UInt16[]) || vType == typeof(UInt64[]))
            {
                Array rr = (Array)value;
                QsVector v = new QsVector(rr.Length);
                foreach (var m in rr)
                {
                    var r = (double)System.Convert.ChangeType(value, typeof(double));
                    v.AddComponent(new QsScalar { NumericalQuantity = r.ToQuantity() });
                }
                return v;
            }


            // the last thing is to return object from this type
            if (!vType.IsValueType) return QsObject.CreateNativeObject(value);

            throw new QsException(vType + " doesn't have corresponding type in Quantity System");
        }




        /// <summary>
        /// Converts the value expression into the target type if needed.
        /// </summary>
        /// <param name="targetType">The type which we will convert to.</param>
        /// <param name="value">the value expression needed to be converted.</param>
        /// <returns></returns>
        internal static Expression QsToNativeConvert(Type targetType, Expression value)
        {
            if (targetType == typeof(QsValue) || targetType.BaseType == typeof(QsValue)) 
                return Expression.Convert(value, targetType);   //no need to change because target type is QsValue which is the Qs primary type.

            if (targetType.BaseType == typeof(AnyQuantity<double>) || targetType == typeof(AnyQuantity<double>))
            {
                //QsScalar.One.NumericalQuantity
                // convert to the target

                // convert to scalar and get NumericalQuantity
                var nq = Expression.Property(Expression.Convert(value, typeof(QsScalar)), "NumericalQuantity");

                return Expression.Convert(nq, targetType);

            }

            if (targetType == typeof(string))
            {
                //QsText s; s.Text
                var nq = Expression.Property(Expression.Convert(value, typeof(QsText)), "Text");

                return nq;

            }


            if (targetType == typeof(int) || targetType == typeof(double) || targetType == typeof(Single) || targetType == typeof(float) || targetType == typeof(Int64) || targetType == typeof(uint) || targetType == typeof(UInt16) || targetType == typeof(UInt64))
            {
                var nq = Expression.Property(Expression.Convert(value, typeof(QsScalar)), "NumericalQuantity");

                var nqv = Expression.Property(nq, "Value");
                return Expression.Convert(nqv, targetType);
            }

            if (targetType == typeof(int[]) || targetType == typeof(double[]) || targetType == typeof(Single[]) || targetType == typeof(float[]) || targetType == typeof(Int64[]) || targetType == typeof(uint[]) || targetType == typeof(UInt16[]) || targetType == typeof(UInt64[]))
            {
                // then the value should be a qsvector or qsmatrix
                
                
                var mi = typeof(Root).GetMethod("ToNumericArray", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(targetType.GetElementType());

                return Expression.Call(mi, value);
                
            }
            
            // at last test if the target type may be a an object 
            // and the source may be a QsObject <===>  I can't test the type of the expression because everything is QsValue
            //  so I will make an assumption that the target type
            if (!targetType.IsSubclassOf(typeof(QsValue)) && !targetType.IsValueType)
            {
                var v = Expression.Convert(Expression.Property(Expression.Convert(value, typeof(QsObject)), "ThisObject"), targetType);
                return v;
            }

            throw new QsException("Couldn't convert the value (" + value.Type.ToString() + ") expression to the target type (" + targetType.ToString() + ")");
        }

        /// <summary>
        /// Gets the double values of the vector components
        /// Warning: Unit information will be missed.
        /// </summary>
        /// <returns></returns>
        internal static Numeric[] ToNumericArray<Numeric>(QsValue value) where Numeric : struct
        {
            if (value is QsVector)
            {
                var ListStorage = ((QsVector)value).ToArray();

                Numeric[] dd = new Numeric[ListStorage.Length];
                int ix = ListStorage.Length - 1;
                while (ix >= 0)
                {
                    dd[ix] = (Numeric)(object)ListStorage[ix].NumericalQuantity.Value;
                    ix--;
                }
                return dd;
            }
            throw new QsException("Not Vector");
        }

        internal static object QsToNativeConvert(Type targetType, object value)
        {
            if (targetType == typeof(QsValue)) return value;   //no need to change because target type is QsValue which is the Qs primary type.

            if (targetType.BaseType == typeof(AnyQuantity<double>))
            {
                //QsScalar.One.NumericalQuantity
                // convert to the target

                return new QsScalar { NumericalQuantity = (AnyQuantity<double>)value };

            }

            if (targetType == typeof(string))
            {
                return  value.ToString();

            }

            if (targetType == typeof(int) || targetType == typeof(double) || targetType == typeof(Single) || targetType == typeof(float))
            {
                var vs = value as QsScalar;

                return Convert.ChangeType(vs.NumericalQuantity.Value, targetType);
                //return Convert.ToDouble(value).ToQuantity().ToScalar();

            }


            throw new QsException("Couldn't convert the value (" + value.GetType().ToString() + ") expression to the target type (" + targetType.ToString() + ")");
        }

    }
}