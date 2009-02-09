using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using System.Collections.ObjectModel;


using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class DerivedQuantity<T> : AnyQuantity<T>
    {


        #region class instantiation
        private AnyQuantity<T>[] InternalQuantities;


        public DerivedQuantity(int exponent, params AnyQuantity<T>[] internalQuantities)
            : base(exponent)
        {
            InternalQuantities = internalQuantities;

        }

        public AnyQuantity<T>[] GetInternalQuantities()
        {
            return InternalQuantities;
        }

        internal void SetInternalQuantities(AnyQuantity<T>[] quantities)
        {
            InternalQuantities = quantities;
        }
        

        #endregion

        #region M L T Processing


        public override QuantityDimension Dimension
        {
            get
            {
                QuantityDimension QDTotal = new QuantityDimension();

                foreach (AnyQuantity<T>  aq in InternalQuantities)
                {
                    QDTotal += aq.Dimension;
                }

                return QDTotal;
            }
        }

        public override BaseQuantity Invert()
        {
            List<AnyQuantity<T>> lq = new List<AnyQuantity<T>>();

            foreach (AnyQuantity<T> qty in InternalQuantities)
            {
                lq.Add((AnyQuantity<T>)qty.Invert());
            }

            DerivedQuantity<T> dq = (DerivedQuantity<T>)this.Clone();

            dq.SetInternalQuantities(lq.ToArray());
            dq.Value = AnyQuantity<T>.DivideScalarByGeneric(1.0, dq.Value);
            if (this.Unit != null)
            {
                dq.Unit = this.Unit.Invert();

            }

            return dq;
        }
        
        
        #endregion        
    }
}
