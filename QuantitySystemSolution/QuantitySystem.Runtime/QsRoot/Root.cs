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

        private static Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
        public static void LoadLibrary(string library)
        {
            Assembly a = Assembly.LoadFrom(library);

            LoadedAssemblies[a.FullName] = a;
        }


        /// <summary>
        /// Get External Library Type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Type GetExternalType(string type)
        {
            Type dt = null;
            foreach (var a in LoadedAssemblies.Values)
            {
                dt = a.GetType(type, false, true);

                if (dt != null) break;
            }

            return dt;
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

            if (targetType == typeof(int) || targetType == typeof(double) || targetType == typeof(Single) || targetType == typeof(float))
            {
                var nq = Expression.Property(Expression.Convert(value, typeof(QsScalar)), "NumericalQuantity");

                var nqv = Expression.Property(nq, "Value");
                return Expression.Convert(nqv, targetType);
            }


            throw new QsException("Couldn't convert the value (" + value.Type.ToString() + ") expression to the target type (" + targetType.ToString() + ")");
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
                return new QsText((string)value);

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