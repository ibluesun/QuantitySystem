using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Units.UnitSystems;
using QuantitySystem.Quantities.BaseQuantities;
using System.Globalization;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.SIUnits
{

    /// <summary>
    /// Coherent derived SI unit.
    /// generated when no suitable Strongly Typed unit exist for the quantity.
    /// the class should represent the unit in base units
    /// then suggest if it can include 
    /// </summary>
    public sealed class DerivedSIUnit : ISIUnit
    {


        #region Constructors Revised

        private List<ISIUnit> SubUnits;

        /// <summary>
        /// generate a derived SI unit from base units by investigating the passed dimension.
        /// </summary>
        /// <param name="dimension"></param>
        public DerivedSIUnit(QuantityDimension dimension)
        {
            SubUnits = new List<ISIUnit>();

            Dimension = dimension;

            if (Dimension.MassExponent != 0)
            {
                ISIUnit u = new BaseUnits.Gram();
                u.Exponent = dimension.MassExponent;
                SubUnits.Add(u);
            }

            if (Dimension.LengthExponent != 0)
            {
                ISIUnit u = new BaseUnits.Metre();
                u.Exponent = dimension.LengthExponent;
                SubUnits.Add(u);
            }

            if (Dimension.TimeExponent != 0)
            {
                ISIUnit u = new Second();
                u.Exponent = dimension.TimeExponent;
                SubUnits.Add(u);
            }

            if (Dimension.TemperatureExponent != 0)
            {
                ISIUnit u = new BaseUnits.Kelvin();
                u.Exponent = dimension.TemperatureExponent;
                SubUnits.Add(u);
            }

            if (Dimension.LuminousIntensityExponent != 0)
            {
                ISIUnit u = new BaseUnits.Candela();
                u.Exponent = dimension.LuminousIntensityExponent;
                SubUnits.Add(u);
            }

            if (Dimension.AmountOfSubstanceExponent != 0)
            {
                ISIUnit u = new BaseUnits.Mole();
                u.Exponent = dimension.AmountOfSubstanceExponent;
                SubUnits.Add(u);
            }

            if (Dimension.ElectricCurrentExponent != 0)
            {
                ISIUnit u = new BaseUnits.Ampere();
                u.Exponent = dimension.ElectricCurrentExponent;
                SubUnits.Add(u);
            }

            GenerateUnitNameFromSubBaseUnits();


            CalculatePrefix();


        }

        /// <summary>
        /// Construct Derived SI unit from base units or strongly typed units.
        /// </summary>
        /// <param name="siUnits"></param>
        public DerivedSIUnit(params ISIUnit[] siUnits)
        {
            Dimension = new QuantityDimension();
            SubUnits = new List<ISIUnit>();


            foreach (ISIUnit si in siUnits)
            {
                Dimension += si.Dimension;

                DerivedSIUnit dsi = si as DerivedSIUnit;

                if (dsi != null)
                {
                    SubUnits.AddRange(dsi.SubUnits);
                }
                else
                    SubUnits.Add(si);

            }

            GenerateUnitNameFromSubBaseUnits();

            CalculatePrefix();

        }

        public void Refresh()
        {
            GenerateUnitNameFromSubBaseUnits();

            CalculatePrefix();
        }

        /// <summary>
        /// Assign the prefix of the unit to the current unit and return new corrected instance.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public IUnit CorrectUnitBy(IUnit unit)
        {
            //throw new NotSupportedException("review your code");
            DerivedSIUnit neu = (DerivedSIUnit)this.MemberwiseClone();

            neu.GetSubunits()[0].Prefix += ((ISIUnit)unit).Prefix;
            //neu.Prefix = ((ISIUnit)unit).Prefix;

            neu.Refresh();

            return neu;
        }

        public void AddPrefix(SIPrefix prefix)
        {
            this.SubUnits[0].Prefix += prefix;
            this.Refresh();
        }





        #endregion



        #region Helper functions

        #region Sub Units
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ISIUnit> GetSubBaseUnits()
        {
            List<ISIUnit> bunits = new List<ISIUnit>();

            //if SubUnit(i) is derivedsiunit then it may hold another base units, get them

            foreach (ISIUnit bu in SubUnits)
            {
                if (bu.IsBaseUnit == false)
                {
                    ISIUnit si = bu.GetUnitInBaseUnits();
                    if (si.IsBaseUnit == false)
                    {
                        bunits.AddRange(((DerivedSIUnit)si).GetSubunits());
                    }
                    else
                    {
                        bunits.Add(si);
                    }
                }
                else
                {
                    bunits.Add(bu);
                }
            }

            return bunits;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ISIUnit> GetSubunits()
        {
            return SubUnits;
        }



        /// <summary>
        /// I want to transfer functionality in the begining
        /// of class construction.
        /// </summary>
        /// <returns></returns>
        public bool AreSubUnitsInBaseUnits()
        {
            foreach (IUnit unit in SubUnits)
            {
                if (unit.IsBaseUnit == false)
                    return false;
            }
            return true;
        }
        
        #endregion

        #region Unit Symbol processing

        private string UnitNumerator;
        private string UnitDenominator;

        private void ConcatenateUnit(string symbol, int exponent)
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

            
        }


        /// <summary>
        /// adjust the symbol string.
        /// </summary>
        /// <returns></returns>
        private void GenerateUnitNameFromSubBaseUnits()
        {
            UnitNumerator = "";
            UnitDenominator = "";
            foreach (ISIUnit unit in SubUnits)
            {
                ConcatenateUnit(unit.Symbol, unit.Exponent);
            }
        }

        /// <summary>
        /// UnitNumerator + / + UnitDenominator
        /// </summary>
        private string FormatUnitSymbol()
        {
            string UnitSymbol = "<";

            if (UnitNumerator.Length > 0) UnitSymbol += UnitNumerator;
            else UnitSymbol += "1";

            if (UnitDenominator.Length > 0) UnitSymbol += "/" + UnitDenominator;

            UnitSymbol += ">";

            return UnitSymbol;

        }

        #endregion
        
        
        
        
        #endregion



        #region ISIUnit Members

        SIPrefix TotalPrefix;
        private void CalculatePrefix()
        {
            TotalPrefix = SIPrefix.Default;
            //sum all prefixes in sub units
            foreach (ISIUnit unit in SubUnits)
            {
                //SIPrefix sip = SIPrefix.FromExponent(unit.Prefix.Exponent * unit.Exponent); // copy the prefix

                if (unit.Exponent > 0)
                    TotalPrefix += unit.Prefix;
                else
                    TotalPrefix -= unit.Prefix;

            }
        }

        /// <summary>
        /// returns prefix that is sum of all involved prefixes.
        /// </summary>
        public SIPrefix Prefix
        {
            get
            {

                return TotalPrefix;
            }
            set
            {
                throw new NotImplementedException("Wrong assigning");

            }
        }

        public double ToPrefix(SIPrefix prefix, double value)
        {
            double abs = GetAbsoluteValue(value);
            return abs * prefix.Factor;

        }

        public ISIUnit GetUnitInBaseUnits()
        {
            if (AreSubUnitsInBaseUnits() == false)
            {
                DerivedSIUnit dsi = new DerivedSIUnit(this.GetSubBaseUnits().ToArray());
                //the returned unit prefix is default mode  exponent = 1
                // so we need to add the current prefix to it.
                //dsi.Prefix += this.Prefix;
                return dsi;
            }
            else
            {
                return this;
            }
        }

        private QuantityDimension _Dimension;


        public QuantityDimension Dimension
        {
            get
            {
                return _Dimension;
            }
            set
            {
                _Dimension = value;
            }
        }
        #endregion

        #region IUnit Members

        public AnyQuantity CreateThisUnitQuantity()
        {
            //should return derived unit in future.
            AnyQuantity qty =  QuantityDimension.QuantityFrom(Dimension);
            qty.Unit = this;
            return qty;
        }

        public string Symbol
        {
            get { return FormatUnitSymbol(); }
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
                //UnitExponent = 1;
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
            List<ISIUnit> units = new List<ISIUnit>();

            foreach (ISIUnit unit in SubUnits)
            {
                units.Add((ISIUnit)unit.Invert());
            }

            DerivedSIUnit uu = new DerivedSIUnit(units.ToArray());
            uu.Exponent = 0 - UnitExponent;
            return uu;
        }


        public UnitSystem UnitSystem { get { return UnitSystem.SIUnitSystem; } }

        public bool IsSpecialName { get { return false; } }
        
        public bool IsBaseUnit 
        { 
            get 
            { 
                return false; 
            } 
        }

        public IUnit Multiply(IUnit unit)
        {
            return new DerivedSIUnit(this, (ISIUnit)unit);

        }

        public IUnit Divide(IUnit unit)
        {
            IUnit un = unit.Invert();
            
            
            return new DerivedSIUnit(this, (ISIUnit)un);
            
        }

        public  IUnit Add(IUnit unit)
        {
            throw new NotImplementedException();
            
        }
        public IUnit Subtract(IUnit unit)
        {
            ISIUnit siUnit = unit as ISIUnit;
            if (siUnit != null)
            {
                ISIUnit nun = (ISIUnit)this.MemberwiseClone();

                ISIUnit MeInBaseUnits = this.GetUnitInBaseUnits(); //get me as base units
                ISIUnit unitBaseUnits = siUnit.GetUnitInBaseUnits();

                if (MeInBaseUnits.Dimension.Equals(unitBaseUnits.Dimension) == false)
                {
                    throw new UnitsNotDimensionallyEqualException();
                }


                if (unitBaseUnits.Prefix.Exponent == MeInBaseUnits.Prefix.Exponent)
                {
                    return nun;
                }
                else
                {
                    nun.Prefix = unitBaseUnits.Prefix + MeInBaseUnits.Prefix;
                }

                return nun;
            }
            else
            {
                throw new UnitNotFoundException("The unit in this block is from another unit system");
            }
        }

       public double GetAbsoluteValue(double relativeValue)
        {
            return relativeValue * Prefix.Factor;
        }

        public double GetRelativeValue(double absoluteValue)
        {
            return absoluteValue / Prefix.Factor;
        }        
        
        #endregion


 








    }
}
