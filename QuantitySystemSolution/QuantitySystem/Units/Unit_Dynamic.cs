using System;
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
        public Unit(QuantityDimension dimension)
        {
            SubUnits = new List<Unit>();

            if (dimension.Mass.Exponent != 0)
            {
                Unit u = new Metric.SI.Gram();
                u.UnitExponent = dimension.Mass.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.Length.Exponent != 0)
            {
                Unit u = new Metric.SI.Metre();
                u.UnitExponent = dimension.Length.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.Time.Exponent != 0)
            {
                Unit u = new Metric.Second();
                u.UnitExponent = dimension.Time.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.Temperature.Exponent != 0)
            {
                Unit u = new Metric.SI.Kelvin();
                u.UnitExponent = dimension.Temperature.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.LuminousIntensity.Exponent != 0)
            {
                Unit u = new Metric.SI.Candela();
                u.UnitExponent = dimension.LuminousIntensity.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.AmountOfSubstance.Exponent != 0)
            {
                Unit u = new Metric.SI.Mole();
                u.UnitExponent = dimension.AmountOfSubstance.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.ElectricCurrent.Exponent != 0)
            {
                Unit u = new Metric.SI.Ampere();
                u.UnitExponent = dimension.ElectricCurrent.Exponent;
                SubUnits.Add(u);
            }



            this.symbol = GenerateUnitSymbolFromSubBaseUnits();


            this.isDefaultUnit = false;

            try
            {
                Type qType = QuantityDimension.QuantityTypeFrom(dimension);
                this.quantityType = qType;
            }
            catch(QuantityNotFoundException)
            {
                this.quantityType = typeof(AnyQuantity<>);

            }
            

            this.isBaseUnit = false;

        }


        /// <summary>
        /// Create the unit directly from the specfied dimension based on the unit system given.
        /// </summary>
        /// <param name="dimension"></param>
        public Unit(QuantityDimension dimension, string unitSystem)
        {
            SubUnits = new List<Unit>();

            if (dimension.Mass.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Mass<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Mass.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.Length.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Length<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Length.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.Time.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Time<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Time.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.Temperature.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(Temperature<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.Temperature.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.LuminousIntensity.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(LuminousIntensity<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.LuminousIntensity.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.AmountOfSubstance.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(AmountOfSubstance<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.AmountOfSubstance.Exponent;
                SubUnits.Add(u);
            }

            if (dimension.ElectricCurrent.Exponent != 0)
            {
                Type UnitType = Unit.GetDefaultUnitTypeOf(typeof(ElectricalCurrent<>), unitSystem);

                Unit u = (Unit)Activator.CreateInstance(UnitType);

                u.UnitExponent = dimension.ElectricCurrent.Exponent;
                SubUnits.Add(u);
            }



            this.symbol = GenerateUnitSymbolFromSubBaseUnits();


            this.isDefaultUnit = false;

            try
            {
                Type qType = QuantityDimension.QuantityTypeFrom(dimension);
                this.quantityType = qType;
            }
            catch (QuantityNotFoundException)
            {
                this.quantityType = typeof(AnyQuantity<>);

            }


            this.isBaseUnit = false;

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
                    SubUnits.Add(u);
                }

                if (dimension.Length.Exponent != 0)
                {
                    Unit u = new Metric.SI.Metre();
                    u.UnitExponent = dimension.Length.Exponent;
                    SubUnits.Add(u);
                }

                if (dimension.Time.Exponent != 0)
                {
                    Unit u = new Metric.Second();
                    u.UnitExponent = dimension.Time.Exponent;
                    SubUnits.Add(u);
                }

                if (dimension.Temperature.Exponent != 0)
                {
                    Unit u = new Metric.SI.Kelvin();
                    u.UnitExponent = dimension.Temperature.Exponent;
                    SubUnits.Add(u);
                }

                if (dimension.LuminousIntensity.Exponent != 0)
                {
                    Unit u = new Metric.SI.Candela();
                    u.UnitExponent = dimension.LuminousIntensity.Exponent;
                    SubUnits.Add(u);
                }

                if (dimension.AmountOfSubstance.Exponent != 0)
                {
                    Unit u = new Metric.SI.Mole();
                    u.UnitExponent = dimension.AmountOfSubstance.Exponent;
                    SubUnits.Add(u);
                }

                if (dimension.ElectricCurrent.Exponent != 0)
                {
                    Unit u = new Metric.SI.Ampere();
                    u.UnitExponent = dimension.ElectricCurrent.Exponent;
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

            this.quantityType = quantityType;

            this.isBaseUnit = false;

        }


        /// <summary>
        /// Construct a unit based on the default units of the internal quantities of passed quantity instance.
        /// Dimensionless quantity will return their native sub quantities units.
        /// this connstructor is useful like when you pass torque quantity it will return "N.m"
        /// but when you use Energy Quantity it will return J.
        /// </summary>
        /// <param name="quantity"></param>
        public Unit(BaseQuantity quantity)
        {
            SubUnits = new List<Unit>();

            Type m_QuantityType = quantity.GetType();


            //try direct mapping first to get the unit

            Type InnerUnitType = Unit.GetDefaultSIUnitTypeOf(m_QuantityType);



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
                    Type l2_InnerUnitType = Unit.GetDefaultSIUnitTypeOf(InnerQuantity.GetType());

                    if (l2_InnerUnitType == null)
                    {
                        //this means for this quantity there is no direct mapping to SI Unit
                        // so we should create unit for this quantity

                        Unit un = new Unit(InnerQuantity);
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

                SubUnits.Add(un);

            }

            SubUnits = GroupUnits(SubUnits); //group similar units

            this.symbol = GenerateUnitSymbolFromSubBaseUnits();

            this.isDefaultUnit = true;

            if (!m_QuantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>

                this.quantityType = m_QuantityType.GetGenericTypeDefinition();

            }
            else
            {

                this.quantityType = m_QuantityType;
            }

            this.isBaseUnit = false;


        }




        /// <summary>
        /// This constructor creates a unit from several units.
        /// </summary>
        /// <param name="units"></param>
        public Unit(Type quantityType, params Unit[] units)
        {
            SubUnits = new List<Unit>();

            foreach (Unit un in units)
            {
                //include only the units that isn't dimensionless
                if (un.QuantityType != typeof(DimensionlessQuantity<>))
                {
                    SubUnits.Add(un);
                }
            }
           

            SubUnits = GroupUnits(SubUnits); //group similar units

            this.symbol = GenerateUnitSymbolFromSubBaseUnits();

            this.isDefaultUnit = true;

            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>

                this.quantityType = quantityType.GetGenericTypeDefinition();

            }
            else
            {
                this.quantityType = quantityType;
            }

            this.isBaseUnit = false;

        }



        #endregion

        
        private List<Unit> GroupUnits(List<Unit> units)
        {
            if (units.Count == 1) return units;
            List<Unit> GroupedUnits = new List<Unit>();

            //i'll two indexes with two inner loops
            // outer loop will put the unit in the Grouped Units
            // inner loop will accumulate the equivalent repeated units.

            int udx = 0; //sub units index
            int idx = udx + 1;

            while (udx < units.Count)
            {
                Unit CurrentUnit = (Unit)units[udx].MemberwiseClone(); //copy the object due to we don't want to alter the original list.

                //inner loop
                while (idx < units.Count)
                {
                    Unit PointedUnit = units[idx];

                    //by pass the repeated units.
                    
                    //  Make sure they are strongly typed not dynamically created.
                    if (CurrentUnit.IsStronglyTyped == true && PointedUnit.IsStronglyTyped == true)
                    {
                        //by checking the unit type equality.
                        if ((CurrentUnit.GetType() != PointedUnit.GetType()))
                        {

                            //the units are different in type.
                            //  so exit the loop and increase the index
                            //   this will make the next current unit is the new one.

                            break;
                        }
                    }
                    else
                    {
                        //check something I don't know.
                        break;

                    }

                    //this code is executed when the two units are identical.
                    CurrentUnit.UnitExponent += PointedUnit.UnitExponent;
                    idx++;

                }
                //add the accumlated unit   //however the udx is pointing to the new point.
                GroupedUnits.Add(CurrentUnit);
                udx = idx;
                idx++;

               

            }

            return GroupedUnits;
        }


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
