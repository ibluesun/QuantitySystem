﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities;
using System.Reflection;
using System.Text.RegularExpressions;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units
{
    public partial class Unit
    {
        #region Dynamically created unit

        private List<Unit> SubUnits { get; set; } //the list shouldn't been modified by sub classes

        /// <summary>
        /// Create the unit directly from the specfied dimension in its SI base units.
        /// </summary>
        /// <param name="dimension"></param>
        public static Unit DiscoverUnit(QuantityDimension dimension)
        {
            return DiscoverUnit(dimension, "Metric.SI");
        }


        /// <summary>
        /// Create the unit directly from the specfied dimension based on the unit system given.
        /// </summary>
        /// <param name="dimension"></param>
        public static Unit DiscoverUnit(QuantityDimension dimension, string unitSystem)
        {
            List<Unit> SubUnits = new List<Unit>();

            if (dimension.Mass.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Mass<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Mass.Exponent;
                u.UnitDimension = new QuantityDimension(dimension.Mass.Exponent, 0, 0);
                
                SubUnits.Add(u);
            }

            if (dimension.Length.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Length<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Length.Exponent;
                u.UnitDimension = new QuantityDimension() { Length = dimension.Length };

                SubUnits.Add(u);
            }

            if (dimension.Time.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Time<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Time.Exponent;
                u.UnitDimension = new QuantityDimension() { Time = dimension.Time };

                SubUnits.Add(u);
            }

            if (dimension.Temperature.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Temperature<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Temperature.Exponent;
                u.UnitDimension = new QuantityDimension() { Temperature = dimension.Temperature };

                SubUnits.Add(u);
            }

            if (dimension.LuminousIntensity.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(LuminousIntensity<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.LuminousIntensity.Exponent;
                u.UnitDimension = new QuantityDimension() { LuminousIntensity = dimension.LuminousIntensity };

                SubUnits.Add(u);
            }

            if (dimension.AmountOfSubstance.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(AmountOfSubstance<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.AmountOfSubstance.Exponent;
                u.UnitDimension = new QuantityDimension(0, 0, 0, 0, 0, dimension.AmountOfSubstance.Exponent, 0);

                SubUnits.Add(u);
            }

            if (dimension.ElectricCurrent.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(ElectricalCurrent<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.ElectricCurrent.Exponent;
                u.UnitDimension = new QuantityDimension() { ElectricCurrent = dimension.ElectricCurrent };


                SubUnits.Add(u);
            }


            Unit un = null;

            try
            {
                Type qType = QuantityDimension.QuantityTypeFrom(dimension);
                un = new Unit(qType, SubUnits.ToArray());
            }
            catch (QuantityNotFoundException)
            {
                un = new Unit(null, SubUnits.ToArray());

            }

            return un;

        }


        /// <summary>
        /// Construct a unit based on the quantity type in SI Base units.
        /// Any Dimensionless quantity will return  in its unit.
        /// </summary>
        /// <param name="quantityType"></param>
        public Unit(Type quantityType)
        {

            SubUnits = new List<Unit>();

            //try direct mapping first to get the unit

            Type InnerUnitType = Unit.GetDefaultSIUnitTypeOf(quantityType);

            if (InnerUnitType == null)
            {
                QuantityDimension dimension = QuantityDimension.DimensionFrom(quantityType);


                if (dimension.Mass.Exponent != 0)
                {
                    Unit u = new Metric.SI.Gram();
                    u.UnitExponent = dimension.Mass.Exponent;
                    u.UnitDimension = u.UnitDimension * dimension.Mass.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Length.Exponent != 0)
                {
                    Unit u = new Metric.SI.Metre();
                    u.UnitExponent = dimension.Length.Exponent;
                    u.UnitDimension = u.UnitDimension * dimension.Length.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Time.Exponent != 0)
                {
                    Unit u = new Shared.Second();
                    u.UnitExponent = dimension.Time.Exponent;
                    u.UnitDimension = u.UnitDimension * dimension.Time.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Temperature.Exponent != 0)
                {
                    Unit u = new Metric.SI.Kelvin();
                    u.UnitExponent = dimension.Temperature.Exponent;
                    u.UnitDimension = u.UnitDimension * dimension.Temperature.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.LuminousIntensity.Exponent != 0)
                {
                    Unit u = new Metric.SI.Candela();
                    u.UnitExponent = dimension.LuminousIntensity.Exponent;
                    u.UnitDimension = u.UnitDimension * dimension.LuminousIntensity.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.AmountOfSubstance.Exponent != 0)
                {
                    Unit u = new Metric.SI.Mole();
                    u.UnitExponent = dimension.AmountOfSubstance.Exponent;
                    u.UnitDimension = u.UnitDimension * dimension.AmountOfSubstance.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.ElectricCurrent.Exponent != 0)
                {
                    Unit u = new Metric.SI.Ampere();
                    u.UnitExponent = dimension.ElectricCurrent.Exponent;
                    u.UnitDimension = u.UnitDimension * dimension.ElectricCurrent.Exponent;

                    SubUnits.Add(u);
                }

            }

            else
            {
                //subclass of AnyQuantity
                //use direct mapping with the exponent of the quantity

                Unit un = (Unit)Activator.CreateInstance(InnerUnitType);

                SubUnits.Add(un);

            }

            this.symbol = GenerateUnitSymbolFromSubBaseUnits();


            this.isDefaultUnit = true;

            //the quantity may be derived quantity which shouldn't be referenced :check here.
            this.quantityType = quantityType;

            
            unitDimension = QuantityDimension.DimensionFrom(this.quantityType);

            this.isBaseUnit = false;

        }


        /// <summary>
        /// Construct a unit based on the default units of the internal quantities of passed quantity instance.
        /// Dimensionless quantity will return their native sub quantities units.
        /// this connstructor is useful like when you pass torque quantity it will return "N.m"
        /// but when you use Energy Quantity it will return J.
        /// </summary>
        /// <param name="quantity"></param>
        public static Unit DiscoverUnit(BaseQuantity quantity)
        {

            Type m_QuantityType = quantity.GetType();
            if (m_QuantityType.GetGenericTypeDefinition() == typeof(RadiusLength<>))
            {
                //because all length units associated with the Length<> Type
                m_QuantityType = typeof(Length<>).MakeGenericType(m_QuantityType.GetGenericArguments()[0]);
            }

            if (quantity.Dimension.IsDimensionless)
            {
                Type QtyType = m_QuantityType;
                if (!QtyType.IsGenericTypeDefinition)
                {
                    QtyType = QtyType.GetGenericTypeDefinition();

                }

                if (QtyType == typeof(DimensionlessQuantity<>))
                {
                    return DiscoverUnit(QuantityDimension.Dimensionless);
                }

            }



            List<Unit> SubUnits = new List<Unit>();

            //try direct mapping first to get the unit

            Type InnerUnitType = GetDefaultSIUnitTypeOf(m_QuantityType);


            if (InnerUnitType == null) //no direct mapping so get it from the inner quantities
            {
                //I can't cast BaseQuantity to AnyQuantity<object>  very annoying
                //so I used reflection.

                MethodInfo GIQ = m_QuantityType.GetMethod("GetInternalQuantities");

                //casted the array to BaseQuantity array also
                var InternalQuantities = GIQ.Invoke(quantity, null) as BaseQuantity[];

                foreach (var InnerQuantity in InternalQuantities)
                {
                    //try to get the quantity direct unit
                    Type l2_InnerUnitType = GetDefaultSIUnitTypeOf(InnerQuantity.GetType());

                    if (l2_InnerUnitType == null)
                    {
                        //this means for this quantity there is no direct mapping to SI Unit
                        // so we should create unit for this quantity

                        Unit un = DiscoverUnit(InnerQuantity);
                        if (un.SubUnits.Count > 0)
                        {
                            SubUnits.AddRange(un.SubUnits);
                        }
                        else
                        {
                            SubUnits.Add(un);
                        }
                    }
                    else
                    {
                        //found :) create it with the exponent

                        Unit un = (Unit)Activator.CreateInstance(l2_InnerUnitType);
                        un.UnitExponent = InnerQuantity.Exponent;
                        un.UnitDimension = InnerQuantity.Dimension;

                        SubUnits.Add(un);
                    }

                }

            }
            else
            {
                //subclass of AnyQuantity
                //use direct mapping with the exponent of the quantity

                Unit un = (Unit)Activator.CreateInstance(InnerUnitType);
                un.UnitExponent = quantity.Exponent;
                un.UnitDimension = quantity.Dimension;

                return un;   

            }

            
            return new Unit(m_QuantityType, SubUnits.ToArray());


        }




        /// <summary>
        /// This constructor creates a unit from several units.
        /// </summary>
        /// <param name="units"></param>
        internal Unit(Type quantityType, params Unit[] units)
        {
            SubUnits = new List<Unit>();

            foreach (Unit un in units)
            {
                //include only the units that isn't dimensionless
                //if (un.QuantityType != typeof(DimensionlessQuantity<>))
                {
                    SubUnits.Add(un);
                }
            }
           

            SubUnits = GroupUnits(SubUnits); //group similar units

            this.symbol = GenerateUnitSymbolFromSubBaseUnits();

            // if the passed type is AnyQuantity<object> for example
            //     then I want to get the type without type parameters AnyQuantity<>
            if (quantityType != null)
            {
                if (!quantityType.IsGenericTypeDefinition)
                    quantityType = quantityType.GetGenericTypeDefinition();
            }


            if (quantityType != typeof(DerivedQuantity<>) && quantityType != null)
            {
                if (quantityType != typeof(DimensionlessQuantity<>)) this.isDefaultUnit = true;

                this.quantityType = quantityType;

                //get the unit dimension from the passed type.
                unitDimension = QuantityDimension.DimensionFrom(quantityType);

            }
            else
            {
                //passed type is derivedQuantity which indicates that the units representing unknow derived quantity to the system
                //so that quantityType should be kept as derived quantity type.
                this.quantityType = quantityType;


                //get the unit dimension from the passed units.
                this.unitDimension = QuantityDimension.Dimensionless;
                foreach (Unit uu in SubUnits)
                    this.unitDimension += uu.UnitDimension;
            }


            this.isBaseUnit = false;

        }



        #endregion

        /// <summary>
        /// Take the sub units recursively and return all of in a flat list.
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        private List<Unit> FlattenUnits(List<Unit> units)
        {
            List<Unit> all = new List<Unit>();
            foreach (Unit un in units)
            {
                if (un.IsStronglyTyped)
                    all.Add(un);
                else
                    all.AddRange(un.SubUnits);
            }
            return all;
        }
        

        /// <summary>
        /// Group all similar units so it remove units that reached exponent zero
        /// also keep track of prefixes of metric units.
        /// </summary>
        /// <param name="bulk_units"></param>
        /// <returns></returns>
        private List<Unit> GroupUnits(List<Unit> bulk_units)
        {
            List<Unit> units = FlattenUnits(bulk_units);

            if (units.Count == 1) return units;

            

            List<Unit> GroupedUnits = new List<Unit>();


            Dictionary<Type, Unit> us = new Dictionary<Type, Unit>();
            foreach (Unit un in units)
            {
                
                if (us.ContainsKey(un.GetType()))
                {
                    //check for prefixes before accumulating units
                    //   otherwise I'll lose the UnitExponent value.
                    if (un is MetricUnit)
                    {
                        //check prefixes to consider milli+Mega for example for overflow

                        MetricPrefix accumPrefix = ((MetricUnit)us[un.GetType()]).UnitPrefix;
                        MetricPrefix sourcePrefix = ((MetricUnit)un).UnitPrefix;

                        //because prefixes in metric are alwayis postitive regardless of the state of the unit
                        //  when we sum them we must consider the unit state
                        //   so I invert them if the host unit is inverted for the sake of computation.
                        if (us[un.GetType()].IsInverted) accumPrefix = ((MetricUnit)us[un.GetType()]).UnitPrefix.Invert();
                        if (un.IsInverted) sourcePrefix = ((MetricUnit)un).UnitPrefix.Invert();

                        try
                        {
                            //Word about MetricPrefix
                            //   The prefix takes the unit exponent as another exponent to it
                            //  so if we are talking about cm^2 actually it is c^2*m^2
                            //  suppose we multiply cm*cm this will give cm^2
                            //     so no need to alter the prefix value
                            // however remain a problem of different prefixes
                            // for example km * cm = ?m^2
                            //  k*c = ?^2
                            //    so ? = (k+c)/2  ;)
                            //  if there is a fraction remove the prefixes totally and substitute them 
                            //  in the overflow flag.

                            // about division
                            // km / cm = ?<1>
                            // k/c = ?   or in exponent k-c=?

                           
                            double sourceExponent = sourcePrefix.Exponent;
                            double accumExponent = accumPrefix.Exponent;

                            double resultExponent = (accumExponent + sourceExponent);

                            if (!(us[un.GetType()].IsInverted ^ un.IsInverted))
                            {
                                //multiplication
                                

                                if (resultExponent % 2 == 0)
                                {
                                    //we can get the symbol of the sqrt of this
                                    double unknown = resultExponent / 2;

                                    ((MetricUnit)us[un.GetType()]).UnitPrefix = MetricPrefix.FromExponent(unknown);
                                }
                                else
                                {
                                    //we can't get the approriate symbol because we have a fraction
                                    // like  kilo * centi = 3-2=1    1/2=0.5   or 1%2=1 
                                    // so we will take the whole fraction and make an overflow

                                    ((MetricUnit)us[un.GetType()]).UnitPrefix = MetricPrefix.None;
                                    unitOverflow += Math.Pow(10, resultExponent);
                                    isOverflowed = true;

                                }
                            }
                            else
                            {
                                //division
                                //resultExponent = (accumExponent - sourceExponent);

                                ((MetricUnit)us[un.GetType()]).UnitPrefix = MetricPrefix.None;
                                unitOverflow += Math.Pow(10, resultExponent);
                                isOverflowed = true;

                            }

                             


                        }
                        catch(MetricPrefixException mpe)
                        {
                            ((MetricUnit)us[un.GetType()]).UnitPrefix = mpe.CorrectPrefix;
                            unitOverflow += Math.Pow(10, mpe.OverflowExponent);
                            isOverflowed = true;
                        }

                    }
                    us[un.GetType()].UnitExponent += un.UnitExponent;
                    us[un.GetType()].UnitDimension += un.UnitDimension;
                }
                else
                {
                    us[un.GetType()] = (Unit)un.MemberwiseClone();
                }
            }
            foreach (Unit un in us.Values)
            {
                if (un.UnitExponent != 0)
                {
                    GroupedUnits.Add(un);
                }
                else
                {
                    //zero means units should be skipped
                    // however we are testing for prefix if the unit is metric
                    //  if the unit is metric and deprecated the prefix should be taken into consideration
                    if (un is MetricUnit)
                    {
                        MetricUnit mu = (MetricUnit)un;
                        if (mu.UnitPrefix.Exponent != 0)
                        {
                            isOverflowed = true;
                            unitOverflow += Math.Pow(10, mu.UnitPrefix.Exponent);
                        }
                    }
                }
            }
            
            return GroupedUnits;
        }

        #region overflow code
        protected bool isOverflowed = false;

        /// <summary>
        /// Overflow flag.
        /// </summary>
        public bool IsOverflowed { get { return isOverflowed; } }

        protected double unitOverflow=0.0;
        /// <summary>
        /// This method get the overflow from multiplying/divding metric units with different 
        /// prefixes and then the unit exponent goes to ZERO
        ///     or when result prefix is over the 
        /// the value should be used to be multiplied by the quantity that units were associated to.
        /// after the execution of this method the overflow flag is reset again.
        /// </summary>
        public double GetUnitOverflow()
        {    
            double u =  unitOverflow;
            unitOverflow = 0.0;
            isOverflowed = false;
            return u;
        }
        #endregion

        #region Unit Symbol processing

        /// <summary>
        /// adjust the symbol string.
        /// </summary>
        /// <returns></returns>
        private string GenerateUnitSymbolFromSubBaseUnits()
        {
            string UnitNumerator="";
            string UnitDenominator="";

            Func<string, int, Object> ConcatenateUnit = delegate(string symbol, int exponent)
            {

                if (exponent > 0)
                {
                    if (UnitNumerator.Length > 0) UnitNumerator += ".";
                    UnitNumerator += symbol;
                    if (exponent > 1) UnitNumerator += "^" + exponent.ToString(CultureInfo.InvariantCulture);
                }

                if (exponent < 0)
                {
                    if (UnitDenominator.Length > 0) UnitDenominator += ".";
                    UnitDenominator += symbol;
                    if (exponent < -1) UnitDenominator += "^" + Math.Abs(exponent).ToString(CultureInfo.InvariantCulture);
                }

                return null;
            };


            foreach (Unit unit in SubUnits)
            {
                ConcatenateUnit(unit.Symbol, unit.UnitExponent);
            }

            //return <UnitNumerator / UnitDenominator>
            Func<string> FormatUnitSymbol = delegate()
            {
               string UnitSymbol = "<";

                if (UnitNumerator.Length > 0) UnitSymbol += UnitNumerator;
                else UnitSymbol += "1";

                if (UnitDenominator.Length > 0) UnitSymbol += "/" + UnitDenominator;

                UnitSymbol += ">";

                return UnitSymbol;
            };

            string PreFinalSymbol =  FormatUnitSymbol();

            string FinalSymbol = PreFinalSymbol;


            //remove .<1/.  to be 
            string pattern = @"\.<1/(.+?)>";

            Match m = Regex.Match(PreFinalSymbol, pattern);

            while (m.Success)
            {
                FinalSymbol = FinalSymbol.Replace(m.Groups[0].Value, "/" + m.Groups[1].Value);
                m = m.NextMatch();
            }

            return FinalSymbol;

        }


        #endregion



    }
}
