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
    /// <summary>
    /// This class hold the mathmatical operations of quantity.
    /// </summary>
    public abstract class AnyQuantity : BaseQuantity
    {
        #region constructors
        protected AnyQuantity() : base(1) { }
        protected AnyQuantity(int exponent) : base(exponent) { }
        #endregion

        #region Value & Unit

        private double QuantityValue = 0.0;

        public double Value
        {
            get { return QuantityValue; }
            set { QuantityValue = value; }
        }

        private IUnit QuantityUnit;
        public IUnit Unit
        {
            get { return QuantityUnit; }
            set { QuantityUnit = value; }
        }

        public override string ToString()
        {
            return this.GetType().Name + ": " + Value.ToString(CultureInfo.InvariantCulture) + " " + (Unit != null ? Unit.Symbol : "");
        }
        #endregion


        #region Mathmatical Operations
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

        public static AnyQuantity operator +(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        {
            return Add(firstQuantity, secondQuantity);
        }

        public static AnyQuantity Add(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        { 
            if (firstQuantity.Equals(secondQuantity))
            {

                AnyQuantity AQ = QuantityDimension.QuantityFrom(firstQuantity.Dimension);

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
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
        }
    

        public static AnyQuantity operator -(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        {
            return Subtract(firstQuantity, secondQuantity);
        }

        public static AnyQuantity Subtract(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        {
             if (firstQuantity.Equals(secondQuantity))
            {

                AnyQuantity AQ = QuantityDimension.QuantityFrom(firstQuantity.Dimension);

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
            else
            {
                throw new QuantitiesNotDimensionallyEqualException();
            }
       }

        public static AnyQuantity operator *(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        {
            return Multiply(firstQuantity, secondQuantity);
        }

        public static AnyQuantity Multiply(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        {
            AnyQuantity qresult = (AnyQuantity)ConstructDerivedQuantity(firstQuantity, secondQuantity);

            //deduce the unit from calculation.
            IUnit qunit = firstQuantity.Unit.Multiply(secondQuantity.Unit);
            qresult.Unit = qunit; //assign it to the quantity generated.

            try
            {
                //correct quantities and units
                qresult = QuantityDimension.QuantityFrom(qresult.Dimension);

                //get the pure unit of the quantity
                IUnit unit = GlobalUnitSystem.UnitOf(qresult, firstQuantity.Unit.UnitSystem);

                //take the prefix of calculated unit
                // and associate it to the prefix of predicted unit.

                //if (unit.IsSpecialName)
                {
                    qresult.Unit = unit.CorrectUnitBy(qunit);
                }
                //else
                //{
                //    //leave the predicted quantity.
                //    qresult.Unit = qunit;
                //}
                
                
            }
            catch (QuantityNotFoundException)
            {
            }

            //get the new unit from the dimension

            qresult.Value = firstQuantity.Value * secondQuantity.Value;

            return qresult;
        }



        public static AnyQuantity operator /(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        {
            return Divide(firstQuantity, secondQuantity);
        }

        public static AnyQuantity Divide(AnyQuantity firstQuantity, AnyQuantity secondQuantity)
        {

            //to preserve the units we make a derivedquantity with passed quantities
            // then we take the derived unit in temporary store
            // then we try to get strongly typed Quantity
            // then assign the unit to the generated quantity.
            // this way we won't have to know the unit system of the quantity.

            AnyQuantity sec_qty = (AnyQuantity)secondQuantity.Invert(); //this is a divide process we must make 1/exponenet :)

            //QuantityDimension dim = firstQuantity.Dimension - secondQuantity.Dimension;
            //AnyQuantity qresult = (AnyQuantity)QuantityDimension.QuantityFrom(dim);

            AnyQuantity qresult = (AnyQuantity)ConstructDerivedQuantity(firstQuantity, sec_qty);


            IUnit qunit = firstQuantity.Unit.Divide(secondQuantity.Unit);
            qresult.Unit = qunit;

            try
            {
                qresult = QuantityDimension.QuantityFrom(qresult.Dimension);
                IUnit unit = GlobalUnitSystem.UnitOf(qresult, firstQuantity.Unit.UnitSystem);

                //take the prefix of calculated unit
                // and associate it to the prefix of predicted unit.

                //if (unit.IsSpecialName)
                {
                    qresult.Unit = unit.CorrectUnitBy(qunit);
                }
                //else
                //{
                //    //leave the predicted quantity.
                //    qresult.Unit = qunit;
                //}

            }
            catch (QuantityNotFoundException)
            {
            }

            //get the new unit from the dimension

            qresult.Value = firstQuantity.Value / secondQuantity.Value;

            return qresult;
        }


        public static DerivedQuantity ConstructDerivedQuantity(params AnyQuantity[] quantities)
        {
            DerivedQuantity DQ = new DerivedQuantity(1, quantities);
            return DQ;
        }

        #endregion




    }
}
