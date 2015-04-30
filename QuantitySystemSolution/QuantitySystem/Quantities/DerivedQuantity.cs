using System;
using System.Collections.Generic;
using System.Linq;
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
            
            {
                if (dimension.Length.RegularExponent != 0)
                    quantities.Add(new Length<T>(dimension.Length.RegularExponent));

                if (dimension.Length.PolarExponent != 0)
                    quantities.Add(new PolarLength<T>(dimension.Length.PolarExponent));

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

            if (dimension.Currency.Exponent != 0)
            {
                quantities.Add(new Currency<T>(dimension.Currency.Exponent));
            }

            if (dimension.Digital.Exponent != 0)
            {
                quantities.Add(new Digital<T>(dimension.Digital.Exponent));
            }

            InternalQuantities = quantities.ToArray();
        }

        public DerivedQuantity(float exponent, params AnyQuantity<T>[] internalQuantities)
            : base(exponent)
        {
            InternalQuantities = internalQuantities;
            var qtypes = from qt in internalQuantities
                         select new Tuple<Type, float>(qt.GetType().GetGenericTypeDefinition(), qt.Exponent);

            BaseQuantity.SetInternalQuantities(this.GetType().GetGenericTypeDefinition(), qtypes.ToArray());
        }

        public AnyQuantity<T>[] GetInternalQuantities()
        {
            return InternalQuantities;
        }

        internal void SetInternalQuantities(AnyQuantity<T>[] quantities)
        {
            InternalQuantities = quantities;

            // Break the fucking caching HERE :S :S
            _Dimension = null;
        }
        

        #endregion

        #region M L T Processing


        /// <summary>
        /// Cached Dimension.
        /// </summary>
        private QuantityDimension _Dimension = null;

        /// <summary>
        /// The current dimension of the quantity.
        /// </summary>
        public override QuantityDimension Dimension
        {
            get
            {
                if (_Dimension == null)
                {
                    QuantityDimension QDTotal = new QuantityDimension();

                    foreach (AnyQuantity<T> aq in InternalQuantities)
                    {
                        QDTotal += aq.Dimension;
                    }
                    _Dimension = QDTotal;
                }
                return _Dimension;
            }
        }

        /// <summary>
        /// Invert the derived quantity by inverting every inner quantity in its internal quantities.
        /// </summary>
        /// <returns></returns>
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
