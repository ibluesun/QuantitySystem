using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units
{
    public abstract class Unit<TQuantity> : IUnit where TQuantity : AnyQuantity, new()
    {
        public TQuantity GetMineQuantity()
        {
            TQuantity qty = new TQuantity();
            qty.Unit = this;
            return qty;

        }


        public AnyQuantity CreateThisUnitQuantity()
        {
            return GetMineQuantity();
        }


        public virtual string Symbol
        {
            get
            {
                return "";
            }
        }


        public virtual double GetAbsoluteValue(double relativeValue)
        {
            return relativeValue;
        }

        public virtual double GetRelativeValue(double absoluteValue)
        {
            return absoluteValue;
        }


        public virtual UnitSystem UnitSystem { get { return UnitSystem.GlobalUnitSystem; } }

        public virtual bool IsSpecialName { get { return false; } }
        public virtual bool IsBaseUnit { get { return false; } }

        public virtual IUnit Multiply(IUnit unit)
        {
            throw new NotImplementedException();
        }

        public virtual IUnit Divide(IUnit unit)
        {
            throw new NotImplementedException();
        }

        public virtual IUnit Add(IUnit unit)
        {
            throw new NotImplementedException();
        }

        public virtual IUnit Subtract(IUnit unit)
        {
            throw new NotImplementedException();
        }


        public virtual IUnit CorrectUnitBy(IUnit unit)
        {
            throw new NotImplementedException();
        }


        private int UnitExponent = 1;

        public int Exponent
        {
            get
            {
                return UnitExponent;
            }
            set
            {
                UnitExponent = value;
            }
        }

        public bool Negative
        {
            get
            {
                if (UnitExponent < 0) return true;
                else
                    return false;
            }
        }

        public IUnit Invert()
        {
            IUnit unit = (IUnit)this.MemberwiseClone();
            unit.Exponent = 0 - UnitExponent;
            return unit;
        }


        public virtual QuantityDimension Dimension 
        {
            get
            {
                return null;
            }
        }

    }
}
