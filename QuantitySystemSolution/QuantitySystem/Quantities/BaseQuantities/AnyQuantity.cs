using System;
using QuantitySystem.Units;
using System.Linq.Expressions;

namespace QuantitySystem.Quantities.BaseQuantities
{
    /// <summary>
    /// This class hold the mathmatical operations of quantity.
    /// </summary>
    public abstract partial class AnyQuantity<T> : BaseQuantity
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


        /// <summary>
        /// Text represent the unit part.
        /// </summary>
        public string UnitText
        {
            get
            {
                string un = string.Empty;
                if (Unit != null)
                {
                    un = Unit.Symbol.Trim();

                    if (un[0] != '<') un = "<" + un + ">";
                }
                return un;
            }
        }

        public string ToShortString()
        {
            return Value.ToString() + UnitText;
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

        /// <summary>
        /// Multiply scalar value by generic values
        /// </summary>
        /// <typeparam name="Q"></typeparam>
        /// <param name="factor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Q MultiplyScalarByGeneric<Q>(double factor, Q value)
        {
            if (factor == 1.0) return value;  // return the same value

            var expr = Expression.Multiply(Expression.Constant(factor), Expression.Constant(value));

            // Construct Lambda function which return one object.
            Expression<Func<Q>> cq = Expression.Lambda<Func<Q>>(expr);

            // compile the function
            Func<Q> aqf = cq.Compile();

            // execute the function
            Q result = aqf();

            // return the result
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
                var expr = Expression.Divide(Expression.Constant(factor), Expression.Constant(value));

                // Construct Lambda function which return one object.
                Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

                // compile the function
                Func<T> aqf = cq.Compile();

                // execute the function
                T result = aqf();

                // return the result
                return result;
            }
        }

        public static T MultiplyGenericByGeneric(T firstValue, T secondValue)
        {
            var expr = Expression.Multiply(Expression.Constant(firstValue), Expression.Constant(secondValue));

            // Construct Lambda function which return one object.
            Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

            // compile the function
            Func<T> aqf = cq.Compile();

            // execute the function
            T result = aqf();

            // return the result
            return result;
        }

        public static T DivideGenericByGeneric(T firstValue, T secondValue)
        {
            var expr = Expression.Divide(Expression.Constant(firstValue), Expression.Constant(secondValue));

            // Construct Lambda function which return one object.
            Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

            // compile the function
            Func<T> aqf = cq.Compile();

            // execute the function
            T result = aqf();

            // return the result
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
                var m = typeof(T).GetMethod("Pow", new Type[]{typeof(T), typeof(double)});
                var expr = Expression.Power(Expression.Constant(value), Expression.Constant(factor), m );

                // Construct Lambda function which return one object.
                Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

                // compile the function
                Func<T> aqf = cq.Compile();

                // execute the function
                T result = aqf();

                // return the result
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
            var expr = Expression.Power(Expression.Constant(value), Expression.Constant(factor));

            // Construct Lambda function which return one object.
            Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

            // compile the function
            Func<T> aqf = cq.Compile();

            // execute the function
            T result = aqf();

            // return the result
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
            var expr = Expression.Modulo(Expression.Constant(firstValue), Expression.Constant(secondValue));

            // Construct Lambda function which return one object.
            Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

            // compile the function
            Func<T> aqf = cq.Compile();

            // execute the function
            T result = aqf();

            // return the result
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
            Type QuantityType = QuantityDimension.QuantityTypeFrom(quantityName);

            if (QuantityType == null)
            {
                throw new QuantityNotFoundException();
            }
            else
            {
                
                //QuantityType = QuantityType.MakeGenericType(typeof(T));
                //AnyQuantity<T> qty = (AnyQuantity<T>)Activator.CreateInstance(QuantityType);

                AnyQuantity<T> qty = QuantityDimension.QuantityFrom<T>(QuantityDimension.DimensionFrom(QuantityType));
                return qty;
            }

        }


        #region ICloneable Members

        public AnyQuantity<T> Clone()
        {
            object t = this.MemberwiseClone();
            var t2 = ((AnyQuantity<T>)t);
            if (t2.Unit != null) t2.Unit = (Unit)Unit.Clone();
            return t2;
        }

        #endregion
    }
}
