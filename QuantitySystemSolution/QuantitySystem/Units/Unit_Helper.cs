using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QuantitySystem.Units
{
    public partial class Unit
    {
        
        /// <summary>
        /// Returns quantity based on current unit instance.
        /// </summary>
        /// <typeparam name="T">Quatntity Storage Type</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal AnyQuantity<T> MakeQuantity<T>(T value)
        {


            //create the corresponding quantity
            AnyQuantity<T> qty = this.GetThisUnitQuantity<T>();

            //assign the unit to the created quantity
            qty.Unit = this;

            //assign the value to the quantity
            qty.Value = value;

            return qty;

        }

        
        

        /// <summary>
        /// Returns quantity from the current unit type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AnyQuantity<double> QuantityOf<TUnit>(double value) where TUnit:Unit, new()
        {

            Unit unit = new TUnit();
            return unit.MakeQuantity<double>(value);

        }


        #region Helper Properties
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
        #endregion

        #region Helper Functions and Properties




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
            unitSystem = unitSystem.ToLower(CultureInfo.InvariantCulture);

            if (unitSystem.Contains("metric.si"))
            {
                Type oUnitType = GetDefaultSIUnitTypeOf(quantityType);
                return oUnitType;
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
                                      where si.Namespace.ToLower(CultureInfo.InvariantCulture).EndsWith(unitSystem)
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
                            if (ua is DefaultUnitAttribute)
                            {
                                //explicitly default unit.
                                return true;
                            }
                            else if (ua is MetricUnitAttribute)
                            {
                                //check if the unit has SystemDefault flag true or not.
                                MetricUnitAttribute mua = ua as MetricUnitAttribute;
                                if (mua.SystemDefault)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
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

                if (SystemUnitType == null && unitSystem.Contains("metric"))
                {
                    //try another catch for SI unit for this quantity

                    SystemUnitType = GetDefaultSIUnitTypeOf(quantityType);
                }

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
        public static Type GetDefaultSIUnitTypeOf(Type quantityType)
        {
            //getting the generic type
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>

                quantityType = quantityType.GetGenericTypeDefinition();

            }

            //don't forget to include second in si units it is shared between all metric systems
            var SIUnitTypes = from si in UnitTypes
                              where si.Namespace.ToLower(CultureInfo.InvariantCulture).EndsWith("si") || si == typeof(Metric.Second)  //== typeof(SI.SIUnit)
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


        /// <summary>
        /// Returns the unit corresponding to the passed string.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Unit Parse(string unit)
        {
            //Phase 1: try direct mapping.
            foreach (Type unitType in UnitTypes)
            {
                UnitAttribute ua = GetUnitAttribute(unitType);
                if (ua != null)
                {
                    
                    //units are case sensitive
                    if (Regex.Match(ua.Symbol, "^"+unit+"$", RegexOptions.Singleline).Success)
                    {
                        return (Unit)Activator.CreateInstance(unitType);

                    }
                }
            }

            throw new UnitNotFoundException("Not found in strongly typed units");


        }


        /// <summary>
        /// Get the unit attribute which hold the unit information.
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public static UnitAttribute GetUnitAttribute(Type unitType)
        {

            object[] attributes = (object[])unitType.GetCustomAttributes(true);

            //get the UnitAttribute
            UnitAttribute ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut => ut is UnitAttribute);

            return ua;

        }
    }
}
