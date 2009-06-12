using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using System.Collections.ObjectModel;
using System.Reflection;


namespace QuantitySystem.Quantities.BaseQuantities
{
    public abstract class BaseQuantity : ICloneable
    {
        

        #region Construction

        private int _Exponent;

        protected BaseQuantity(int exponent)
        {
            _Exponent = exponent;
        }

        public int Exponent
        {
            get
            {
                return _Exponent;
            }
        }

        internal void SetExponent(int exp)
        {
            _Exponent = exp;
        }
        
        /// <summary>
        /// make 1/x operation.
        /// </summary>
        public virtual BaseQuantity Invert()
        {
            BaseQuantity bq = (BaseQuantity)this.Clone();
            bq.SetExponent(0 - _Exponent);
            

            return bq;
        }

        #endregion

        #region M L T  processing

        public virtual QuantityDimension Dimension
        {
            get
            {
                return new QuantityDimension();
            }
        }


        #endregion




        #region Dimension Equality Algorithm

        /// <summary>
        /// Provides the Dimensional Equality algorithm.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BaseQuantity bd = obj as BaseQuantity;

            if(bd!=null)
            {
                return this.Dimension.Equals(bd.Dimension);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Dimension.GetHashCode();
        }

        #endregion






        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

}
