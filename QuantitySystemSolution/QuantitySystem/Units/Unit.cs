using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

using System.Reflection;
using QuantitySystem.Attributes;
using System.Globalization;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units
{
    public partial class Unit : ICloneable
    {
        #region Fields
        
        protected string symbol;

        protected bool isDefaultUnit;
        private readonly bool isBaseUnit;


        protected Type quantityType;
        protected QuantityDimension unitDimension;



        
        //the reference unit information.
        protected readonly Unit referenceUnit;

        protected readonly double referenceUnitNumerator;
        protected readonly double referenceUnitDenominator;



        private readonly bool isStronglyTyped = false;

        #endregion


        /// <summary>
        /// Fill the instance of the unit with the attributes
        /// found on it.
        /// </summary>
        protected Unit()
        {
            //only called on the strongly typed units

            isStronglyTyped = true;
            
            //read the current attributes

            MemberInfo info = this.GetType();

            object[] attributes = (object[])info.GetCustomAttributes(true);            

            //get the UnitAttribute
            UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut=>ut is UnitAttribute);

            if (ua != null)
            {
                symbol = ua.Symbol;
                quantityType = ua.QuantityType;
                unitDimension = QuantityDimension.DimensionFrom(quantityType);


                if (ua is DefaultUnitAttribute)
                {
                    isDefaultUnit = true;  //indicates that this unit is the default when creating the quantity in this system
                    //also default unit is the unit that relate its self to the SI Unit.
                }
                else
                {
                    isDefaultUnit = false;
                }
            }
            else
            {
                throw new UnitException("Unit Attribute not found");
            }

            if (quantityType.Namespace == "QuantitySystem.Quantities.BaseQuantities")
            {
                isBaseUnit = true;
            }
            else
            {
                isBaseUnit = false;
            }

            //Get the reference attribute
            ReferenceUnitAttribute dua = (ReferenceUnitAttribute)attributes.SingleOrDefault<object>(ut => ut is ReferenceUnitAttribute);

            if (dua != null)
            {
                if (dua.UnitType != null)
                {
                    referenceUnit = (Unit)Activator.CreateInstance(dua.UnitType);
                }
                else
                {
                    //get the SI Unit Type for this quantity
                    //first search for direct mapping
                    Type SIUnitType = GetDefaultSIUnitTypeOf(quantityType);
                    if (SIUnitType != null)
                    {
                        referenceUnit = (Unit)Activator.CreateInstance(SIUnitType);
                    }
                    else
                    {
                        //try dynamic creation of the unit.
                        referenceUnit = new Unit(quantityType);

                    }
                    
                }

                referenceUnitNumerator = dua.Numerator;
                referenceUnitDenominator = dua.Denominator;

            }

        }



        #region Characterisitics


        
        public virtual string Symbol
        {
            get
            {

                //symbol in strong typed are fetched from the attributes

                if (IsStronglyTyped)
                {
                    return symbol;
                }
                else
                {

                    return symbol;
                }
            }
        }

        /// <summary>
        /// Determine if the unit is the default unit for the quantity type.
        /// </summary>
        public virtual bool IsDefaultUnit
        {
            get
            {
                //based on the current unit attribute
                return isDefaultUnit;
            }
        }

        /// <summary>
        /// The dimension that this unit represents.
        /// </summary>
        public QuantityDimension UnitDimension
        {
            get { return unitDimension; }
            internal set { unitDimension = value; }
        } 


        /// <summary>
        /// The Type of the Quantity of this unit.
        /// </summary>
        public Type QuantityType
        {
            get
            {
                return quantityType;
            }
            internal set
            {
                quantityType = value;
                unitDimension = QuantityDimension.DimensionFrom(value);
            }
        }

        /// <summary>
        /// Tells if Unit is related to one of the seven base quantities.
        /// </summary>
        public bool IsBaseUnit
        {
            get
            {
                return isBaseUnit;
            }
        }



        /// <summary>
        /// The unit that serve a parent for this unit.
        /// and should take the same exponent of the unit.
        /// </summary>
        public virtual Unit ReferenceUnit
        {
            get 
            {
                if (referenceUnit != null)
                {
                    if (referenceUnit.UnitExponent != this.UnitExponent)
                        referenceUnit.UnitExponent = this.UnitExponent;
                }

                return referenceUnit; 
            }
        }

        /// <summary>
        /// How much the current unit equal to the reference unit.
        /// </summary>
        public double ReferenceUnitTimes
        {
            get { return ReferenceUnitNumerator / ReferenceUnitDenominator; }
        }

        public virtual double ReferenceUnitNumerator
        {
            get 
            {

                return Math.Pow(referenceUnitNumerator, unitExponent);
            }
        }

        public virtual double ReferenceUnitDenominator
        {
            get
            {
                return Math.Pow(referenceUnitDenominator, unitExponent);
            }
        }




        #endregion

        #region Operations

        /// <summary>
        /// Invert the current unit simply from numerator to denominator and vice versa.
        /// </summary>
        /// <returns></returns>
        public Unit Invert()
        {
            Unit unit = null;
            if (SubUnits != null)
            {
                //convert sub units if this were only a generated unit.

                List<Unit> InvertedUnits = new List<Unit>();

                foreach (Unit lun in SubUnits)
                {
                    InvertedUnits.Add(lun.Invert());

                }

                unit = new Unit(this.QuantityType, InvertedUnits.ToArray());
                
            }
            else
            {
                //convert exponent because this is a strongly typed unit.

                unit = (Unit)this.MemberwiseClone();
                unit.UnitExponent = 0 - UnitExponent;
                unit.UnitDimension = unit.UnitDimension.Invert();
                
                
            }
            return unit;
        }

        #region Manipulating quantities
        //I want to get away from Activator.CreateInstance because it is very slow :)
        // so I'll cach resluts 
        static Dictionary<Type, object> Qcach = new Dictionary<Type, object>();



        /// <summary>
        /// Gets the quantity of this unit based on the desired container.
        /// <see cref="QuantityType"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AnyQuantity<T> GetThisUnitQuantity<T>()
        {

            AnyQuantity<T> Quantity = null;

            Type qt = QuantityType.MakeGenericType(typeof(T));
                
            object j;
            if (Qcach.TryGetValue(qt, out j))
            {
                Quantity = (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();  //optimization for created quantities before

            }
            else
            {
                Quantity = (AnyQuantity<T>)Activator.CreateInstance(QuantityType.MakeGenericType(typeof(T)));
                Qcach.Add(qt, Quantity);

            }

            Quantity.Unit = this;


            return Quantity;
        }

        
        public AnyQuantity<T> GetThisUnitQuantity<T>(T value)
        {

            AnyQuantity<T> Quantity = null;
            if (QuantityType != typeof(DerivedQuantity<>) && QuantityType != null)
            {
                Type qt = QuantityType.MakeGenericType(typeof(T));
                
                object j;
                if (Qcach.TryGetValue(qt, out j))
                {

                    Quantity = (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();  //optimization for created quantities before
                }
                else
                {

                    Quantity = (AnyQuantity<T>)Activator.CreateInstance(qt);

                    Qcach.Add(qt, Quantity);
                }
            }
            else
            {
                //create it from the unit dimension
                Quantity = new DerivedQuantity<T>(UnitDimension);

            }
            Quantity.Unit = this;

            Quantity.Value = value;

            if (this.IsOverflowed) Quantity.Value =
                 AnyQuantity<T>.MultiplyScalarByGeneric(this.GetUnitOverflow(), value);
            
            return Quantity;
        }

        #endregion



        #endregion


        #region Properties

        private float unitExponent = 1;

        public float UnitExponent
        {
            get
            {
                return unitExponent;
            }
            set
            {
                unitExponent = value;
                
            }
        }

        public const string MixedSystem = "MixedSystem";

        public string UnitSystem 
        { 
            get 
            { 
                //based on the current namespace of the unit
                //return the text of the namespace 
                // after Unit.

                if (IsStronglyTyped)
                {
                    Type UnitType = this.GetType();

                    string ns = UnitType.Namespace.Substring(UnitType.Namespace.LastIndexOf("Units.") + 6);
                    return ns;
                }
                else
                {
                    //mixed system
                    // check all sub units if there unit system is the same then
                    //  return it 


                    if (SubUnits.Count > 0)
                    {
                        string ns = SubUnits[0].UnitSystem;

                        int suidx = 1;

                        while (suidx < SubUnits.Count)
                        {
                            if (SubUnits[suidx].UnitSystem != ns && SubUnits[suidx].UnitSystem!="Shared")
                            {
                                ns = MixedSystem;
                                break;
                            }
                            suidx++;
                        }

                        return ns;
                    }
                    else
                        return "Unknown";

                }
            } 
        }


        /// <summary>
        /// Determine if the unit is inverted or not.
        /// </summary>
        public bool IsInverted
        {
            get
            {
                if (UnitExponent < 0) return true;
                else
                    return false;
            }
        }

        public bool IsStronglyTyped
        {
            get
            {
                return isStronglyTyped;
            }
        }
        #endregion




        public override string ToString()
        {
            return this.GetType().Name + " " + this.Symbol;
        }



        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
