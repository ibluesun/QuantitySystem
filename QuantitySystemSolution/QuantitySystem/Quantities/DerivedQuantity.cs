﻿using System;
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

        public DerivedQuantity(QuantityDimension dimension)
        {
            //create quantity for each sub quantity
            List<AnyQuantity<T>> quantities = new List<AnyQuantity<T>>();

            if (dimension.Mass.Exponent != 0)
            {
                quantities.Add(new Mass<T>(dimension.Mass.Exponent));

            }

            if (dimension.Length.Exponent != 0)
            {
                quantities.Add(new Length<T>(dimension.Length.Exponent));
            }

            if (dimension.Time.Exponent != 0)
            {
                quantities.Add(new Time<T>(dimension.Time.Exponent));
            }

            if (dimension.Temperature.Exponent != 0)
            {
                quantities.Add(new Temperature<T>(dimension.Temperature.Exponent));
            }

            if (dimension.LuminousIntensity.Exponent != 0)
            {
                quantities.Add(new LuminousIntensity<T>(dimension.LuminousIntensity.Exponent));
            }

            if (dimension.AmountOfSubstance.Exponent != 0)
            {
                quantities.Add(new AmountOfSubstance<T>(dimension.AmountOfSubstance.Exponent));
            }

            if (dimension.ElectricCurrent.Exponent != 0)
            {
                quantities.Add(new ElectricalCurrent<T>(dimension.ElectricCurrent.Exponent));
            }

            InternalQuantities = quantities.ToArray();
        }

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
