using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

using System.Reflection;
using QuantitySystem.Units.Attributes;

namespace QuantitySystem.Units
{
    public partial class Unit
    {
        /// <summary>
        /// Fill the instance of the unit with the attributes
        /// found on it.
        /// </summary>
        protected Unit()
        {
            
            //read the current attributes

            MemberInfo info = this.GetType();

            object[] attributes = (object[])info.GetCustomAttributes(true);            

            //get the UnitAttribute
            UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut=>ut is UnitAttribute);

            if (ua != null)
            {
                symbol = ua.Symbol;
                quantityType = ua.QuantityType;

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
                    referenceUnit = (Unit)Activator.CreateInstance(GetSIUnitTypeOf(quantityType));
                    
                }
                referenceUnitTimes = dua.Times;
            }

        }



        #region Characterisitics

        private readonly string symbol;
        protected bool isDefaultUnit;
        private readonly Type quantityType;
        private readonly bool isBaseUnit;

        private readonly Unit referenceUnit;
        private readonly double referenceUnitTimes;


        
        public virtual string Symbol
        {
            get
            {
                //from the attribute get the symbol
                return symbol;
            }
        }

        /// <summary>
        /// Determine if the unit is the default unit for the quantity type.
        /// </summary>
        public bool IsDefaultUnit
        {
            get
            {
                //based on the current unit attribute
                return isDefaultUnit;
            }
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
        /// </summary>
        public Unit ReferenceUnit
        {
            get { return referenceUnit; }
        }

        /// <summary>
        /// How much the current unit equal to the reference unit.
        /// </summary>
        public double ReferenceUnitTimes
        {
            get { return referenceUnitTimes; }
        } 

        #endregion

        #region Operations

        /// <summary>
        /// Invert the current unit simply from numerator to denominator and vice versa.
        /// </summary>
        /// <returns></returns>
        public Unit Invert()
        {
            Unit unit = (Unit)this.MemberwiseClone();
            unit.UnitExponent = 0 - UnitExponent;
            return unit;
        }

        /// <summary>
        /// Gets the quantity of this unit based on the desired container.
        /// <see cref="QuantityType"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public AnyQuantity<T> GetThisUnitQuantity<T>()
        {

            //from the attribute get the quantity.

            AnyQuantity<T> Quantity = (AnyQuantity<T>)Activator.CreateInstance(QuantityType.MakeGenericType(typeof(T)));


            return Quantity;
        }

        /// <summary>
        /// Multilpy unit by another unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual Unit Multiply(Unit unit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dividing unit by another unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual Unit Divide(Unit unit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SI absoulte value after conversion from this unit to SI.
        /// </summary>
        /// <param name="relativeValue"></param>
        /// <returns></returns>
        public virtual double ToSIUnit(double relativeValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets relative value of SI absolute value based on current unit.
        /// </summary>
        /// <param name="absoluteValue">SI absolute value.</param>
        /// <returns></returns>
        public virtual double FromSIUnit(double absoluteValue)
        {
            throw new NotImplementedException();
        }



        #endregion


        #region Properties

        private int unitExponent = 1;

        public int UnitExponent
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

        public string UnitSystem 
        { 
            get 
            { 
                //based on the current namespace of the unit
                //return the text of the namespace
                
                //return string.Empty; 
                throw new NotImplementedException();
            } 
        }


        /// <summary>
        /// Determine if the unit is inverted or not.
        /// </summary>
        public bool IsNegative
        {
            get
            {
                if (UnitExponent < 0) return true;
                else
                    return false;
            }
        }
        #endregion


        #region Helper Functions and Properties


        private static Type[] unitTypes;

        /// <summary>
        /// All Types inherited from Unit Type.
        /// </summary>
        public static Type[] UnitTypes
        {
            get
            {
                if (unitTypes == null)
                {
                    Type[] AllTypes = Assembly.GetExecutingAssembly().GetTypes();

                    var Types = from si in AllTypes
                                where si.IsSubclassOf(typeof(Unit))
                                select si;

                    unitTypes = Types.ToArray();

                }
                return unitTypes;
            }
        }


        /// <summary>
        /// Gets the default unit type of quantity type parameter based on the unit system (namespace)
        /// under the Units name space.
        /// The Default Unit Type for length in Imperial is Foot for example.
        /// </summary>
        /// <param name="quantityType">quantity type</param>
        /// <param name="unitSystem">The Unit System or explicitly the namespace under Units Namespace</param>
        /// <returns>Unit Type Based on the unit system</returns>
        public static Type GetDefaultUnitTypeOf(Type quantityType, string unitSystem)
        {
            if (unitSystem.ToLower() == "si")
            {
                return GetSIUnitTypeOf(quantityType);
            }
            else
            {
                //getting the generic type
                if (!quantityType.IsGenericTypeDefinition)
                {
                    //the passed type is AnyQuantity<object> for example
                    //I want to get the type without type parameters AnyQuantity<>

                    quantityType = quantityType.GetGenericTypeDefinition();

                }

                var SystemUnitTypes = from si in UnitTypes
                                  where si.Namespace.ToLower().EndsWith(unitSystem)
                                  select si;



                Func<Type, bool> SearchForQuantityType = unitType =>
                {
                    //search in the attributes of the unit type
                    MemberInfo info = unitType as MemberInfo;

                    object[] attributes = (object[])info.GetCustomAttributes(true);

                    //get the UnitAttribute
                    UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut => ut is UnitAttribute);

                    if (ua != null)
                    {
                        if (ua.QuantityType == quantityType)
                        {
                            if (ua is DefaultUnitAttribute) return true;
                            else return false;
                        }
                        else
                            return false;

                    }
                    else
                    {
                        return false;
                    }
                };


                Type SystemUnitType = SystemUnitTypes.SingleOrDefault(
                    SearchForQuantityType
                    );

                return SystemUnitType;
            }
        }


        /// <summary>
        /// Gets the unit type of quantity type parameter based on SI unit system.
        /// The function is direct mapping from types of quantities to types of units.
        /// if function returns null then this quantity dosen't have a statically linked unit to it.
        /// this means the quantity should return a unit in runtime.
        /// </summary>
        /// <param name="quantityType">Type of Quantity</param>
        /// <returns>SI Unit Type</returns>
        public static Type GetSIUnitTypeOf(Type quantityType)
        {
            //getting the generic type
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>

                quantityType = quantityType.GetGenericTypeDefinition();

            }

            var SIUnitTypes = from si in UnitTypes
                              where si.BaseType == typeof(SI.SIUnit)
                              select si;



            Func<Type, bool> SearchForQuantityType = unitType =>
            {
                //search in the attributes of the unit type
                MemberInfo info = unitType as MemberInfo;

                object[] attributes = (object[])info.GetCustomAttributes(true);            

                //get the UnitAttribute
                UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut=>ut is UnitAttribute);

                if (ua != null)
                {
                    if (ua.QuantityType == quantityType)
                        return true;
                    else
                        return false;

                }
                else
                {
                    return false;
                }
            };


            Type SIUnitType = SIUnitTypes.SingleOrDefault(
                SearchForQuantityType
                );

            return SIUnitType;



        }

        #endregion


    }
}
