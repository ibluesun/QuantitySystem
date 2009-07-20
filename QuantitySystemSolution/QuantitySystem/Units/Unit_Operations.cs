using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

using System.Reflection;
using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units
{
    public partial class Unit
    {


        /// <summary>
        /// Creat units path from the current unit instance to the default unit of the current 
        /// unit system in the current quantity dimension.
        /// if the unit in the current system have no default unit and direct reference to SI
        /// then the path is stopped on the unit itself and shouldn't bypass it.
        /// </summary>
        /// <returns></returns>
        public UnitPath PathToDefaultUnit()
        {

            //from this unit get my path to the default unit.

            UnitPath path = new UnitPath();


            if (this.ReferenceUnit != null) //check that first parent exist.
            {
                Unit RefUnit = this;
                double RefTimesNum = 1;
                double RefTimesDen = 1;

                //double RefShift = 0.0;

                // do the iteration until we reach the default unit.
                while (RefUnit.IsDefaultUnit == false)
                {

                    path.Push(
                        new UnitPathItem
                        {
                            Unit = RefUnit,
                            Numerator = RefTimesNum,
                            Denumenator = RefTimesDen,
                            //Shift = RefShift
                        }
                        );

                    RefTimesNum = RefUnit.ReferenceUnitNumerator;  //get the value before changing the RefUnit
                    RefTimesDen = RefUnit.ReferenceUnitDenominator;
                    //RefShift = RefUnit.ReferenceUnitShift;
                    
                    RefUnit = RefUnit.ReferenceUnit;
                }

                path.Push(
                    new UnitPathItem
                    {
                        Unit = RefUnit,
                        Numerator = RefTimesNum,
                        Denumenator = RefTimesDen,
                        //Shift = RefShift
                    }
                    );
            }
            else
            {
                // no referenceUnit so this is SI unit because all my units ends with SI
                // and it is default unit because all si units have default units with the default prefix.
                if (this.QuantityType != typeof(DimensionlessQuantity<>))
                {
                    path.Push(
                        new UnitPathItem
                        {
                            Unit = this,
                            Numerator = 1,
                            Denumenator = 1,
                            //Shift = 0.0

                        }
                        );
                }
            }

            return path;
        }



        /// <summary>
        /// Create units path from default unit in the dimension of the current unit system to the running unit instance.
        /// </summary>
        /// <returns></returns>
        public UnitPath PathFromDefaultUnit()
        {
            UnitPath Forward = PathToDefaultUnit();

            UnitPath Backward = new UnitPath();

            while (Forward.Count > 0)
            {
                UnitPathItem upi = Forward.Pop();

                if (upi.Unit.IsDefaultUnit)
                {
                    upi.Numerator = 1;
                    upi.Denumenator = 1;
                    //upi.Shift = 0;
                }
                else
                {
                    upi.Numerator = upi.Unit.ReferenceUnitDenominator;  //invert the number
                    upi.Denumenator = upi.Unit.ReferenceUnitNumerator;
                    //upi.Shift = 0 - upi.Unit.ReferenceUnitShift;
                }

                Backward.Push(upi);
            }

            return Backward;
        }


        /// <summary>
        /// Create units path from unit to unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public UnitPath PathFromUnit(Unit unit)
        {

            return unit.PathToUnit(this);

        }



        public Func<Unit, Unit, string> UnitToUnitSymbol = (Unit x, Unit y) => "[" + x.Symbol + ":" + x.UnitDimension.ToString() + "]" + "__" + "[" + y.Symbol + ":" + y.UnitDimension.ToString() + "]";

        public static Dictionary<string, UnitPath> CachedPathes = new Dictionary<string, UnitPath>();

        private static bool enableUnitsCaching = true;
        public static bool EnableUnitsCaching
        {
            get
            {
                return enableUnitsCaching;
            }
            set
            {
                enableUnitsCaching = value;
                if (enableUnitsCaching) CachedPathes.Clear();
            }
        }

        /// <summary>
        /// Gets the path to the unit starting from current unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public UnitPath PathToUnit(Unit unit)
        {
            //because this method can be length method we try to check for cached pathes first.
            UnitPath cachedPath;
            if (EnableUnitsCaching)
            {
                if (CachedPathes.TryGetValue(UnitToUnitSymbol(this, unit), out cachedPath))
                {
                    return (UnitPath)cachedPath.Clone();   //<--- Clone
                    
                    //Why CLONE :D ??  because the unit path is a stack and I use Pop all the time 
                    // during the application, and there were hidden error that poping from unit path in the 
                    // cached store will not get them back again ;)
                    //  I MUST return cloned copy of the UnitPath.

                }
            }

            if (this.UnitDimension.IsDimensionless == true && unit.UnitDimension.IsDimensionless == true)
            {
                
            }
            else
            {
                //why I've tested dimensioless in begining??
                //   because I want special dimensionless quantities like angle and solid angle to be treated
                //   as normal dimensionless values

                if (this.UnitDimension.Equals(unit.UnitDimension) == false)
                {
                    throw new UnitsNotDimensionallyEqualException();
                }
            }

            //test if one of the units are not strongly typed
            //  because this needs special treatment. ;)
            if (this.IsStronglyTyped == false || unit.IsStronglyTyped == false)
            {
                //the unit is not strongly typed so we need to make conversion to get its conversion
                // Source unit ==> SI Base Units
                // target unit ==> SI BaseUnits

                UnitPath SourcePath = this.PathToSIBaseUnits();
                UnitPath TargetPath = unit.PathToSIBaseUnits();

                UnitPath Tito = new UnitPath();

                

                while (SourcePath.Count > 0)
                {
                    Tito.Push(SourcePath.Pop());
                }
                //we have to invert the target 
                while (TargetPath.Count > 0)
                {
                    UnitPathItem upi = TargetPath.Pop();
                    upi.Invert();

                    Tito.Push(upi);

                }

                //first location in cache look below for the second location.

                if (EnableUnitsCaching)
                {
                    CachedPathes.Add(UnitToUnitSymbol(this, unit), (UnitPath)Tito.Clone());
                }

                return Tito;
            }

            // 1- Get Path default unit to current unit.

            UnitPath FromMeToDefaultUnit = this.PathToDefaultUnit();

            // 2- Get Path From Default unit to the passed unit.

            UnitPath FromDefaultUnitToTargetUnit = unit.PathFromDefaultUnit();

            // 3- check if the two units are in the same unit system
            //  if the units share the same parent don't jump

            UnitPath SystemsPath = null;

            bool NoBoundaryCross = false;



            if (this.UnitSystem == unit.UnitSystem)
            {
                NoBoundaryCross = true;
            }
            else
            {
                //test for that units parents are the same

                string ThisParent = this.UnitSystem.IndexOf('.') > -1 ? 
                    this.UnitSystem.Substring(0,this.UnitSystem.IndexOf('.')) :
                    this.UnitSystem;

                string TargetParent = unit.UnitSystem.IndexOf('.') > -1 ?
                                    unit.UnitSystem.Substring(0, unit.UnitSystem.IndexOf('.')) :
                                    unit.UnitSystem;


                if (ThisParent == TargetParent) NoBoundaryCross = true;
                
            }




            if (NoBoundaryCross)
            {
                //no boundary cross should occur

                //if the two units starts with Metric then no need to cross boundaries because
                //they have common references in metric.
            }
            else
            {
                
                //then we must go out side the current unit system
                //all default units are pointing to the SIUnit system this is a must and not option.
                SystemsPath = new UnitPath();

                //get the default unit of target 


                // to cross the boundary
                // we should know the non SI system that we will cross it
                // we have two options

                // 1- FromMeToDefaultUnit if (Me unit is another system (not SI)
                //     in this case we will take the top unit to get its reference
                // 2- FromDefaultUnitToTargetUnit (where default unit is not SI)
                //     and in this case we will take the last bottom unit of stack and get its reference

                
                SystemsPath = new UnitPath();

                UnitPathItem DefaultPItem;
                UnitPathItem RefUPI;

                Unit SourceDefaultUnit = FromMeToDefaultUnit.Peek().Unit;
                Unit TargetDefaultUnit = FromDefaultUnitToTargetUnit.ElementAt(FromDefaultUnitToTargetUnit.Count - 1).Unit;

                if (SourceDefaultUnit.UnitSystem != "Metric.SI"
                    && SourceDefaultUnit.GetType() != typeof(Shared.Second)
                    )
                {
                    //from source default unit to the si
                    DefaultPItem = FromMeToDefaultUnit.Peek();
                    RefUPI = new UnitPathItem
                        {
                            Numerator = DefaultPItem.Unit.ReferenceUnitNumerator,
                            Denumenator = DefaultPItem.Unit.ReferenceUnitDenominator,
                            //Shift = DefaultPItem.Unit.ReferenceUnitShift,
                            Unit = DefaultPItem.Unit.ReferenceUnit
                        };                
                }
                else
                {
                    // from target default unit to si
                    DefaultPItem = FromDefaultUnitToTargetUnit.ElementAt(FromDefaultUnitToTargetUnit.Count - 1);
                    RefUPI = new UnitPathItem
                    {
                        //note the difference here 
                        //I made the opposite assignments because we are in reverse manner

                        Numerator = DefaultPItem.Unit.ReferenceUnitDenominator, // <=== opposite
                        Denumenator = DefaultPItem.Unit.ReferenceUnitNumerator, // <===
                        //Shift = 0-DefaultPItem.Unit.ReferenceUnitShift,
                        Unit = DefaultPItem.Unit.ReferenceUnit
                    };
                }


                if (RefUPI.Unit != null)  
                {
                    SystemsPath.Push(RefUPI);
                }
                else
                {
                    //both default units were SI units without references

                    //note:
                    // when define units in unit cloud for quantity
                    //  either make all units reference SI units without default unit
                    // or make one default unit and make the rest of units reference it.
                }
            }


            //combine the two paths
            UnitPath Total = new UnitPath();

            //we are building the conversion stairs
            // will end like a stack


            //begin from me unit to default unit
            for(int i=FromMeToDefaultUnit.Count-1;i>=0;i--)
            {
                Total.Push(FromMeToDefaultUnit.ElementAt(i));
            }

            //cross the system if we need to .
            if (SystemsPath != null)
            {
                for (int i = SystemsPath.Count - 1; i >= 0; i--)
                {
                    Total.Push(SystemsPath.ElementAt(i));
                }
            }
            
            // from default unit to target unit
            for (int i = FromDefaultUnitToTargetUnit.Count - 1; i >= 0; i--)
            {
                Total.Push(FromDefaultUnitToTargetUnit.ElementAt(i));
            }




            //another check if the units are inverted then 
            // go through all items in path and invert it also

            if (this.IsInverted && unit.IsInverted)
            {
                foreach (UnitPathItem upi in Total)
                    upi.Invert();
            }

            //Second location in cache  look above for the first one in the same function here :D
            if (EnableUnitsCaching)
            {
                CachedPathes.Add(UnitToUnitSymbol(this, unit), (UnitPath)Total.Clone());
            }

            return Total;
        }



        public UnitPath PathToSIBaseUnits()
        {
            if (this.IsStronglyTyped)
            {
                //get the corresponding unit in the SI System
                Type InnerUnitType = Unit.GetDefaultSIUnitTypeOf(this.QuantityType);

                if (InnerUnitType == null && this.QuantityType == typeof(RadiusLength<>))
                    InnerUnitType = Unit.GetDefaultSIUnitTypeOf(typeof(Length<>));

                if (InnerUnitType == null)
                {
                    //some quantities don't have strongly typed si units

                    //like knot unit there are no corresponding velocity unit in SI
                    //  we need to replace the knot unit with mixed unit to be able to do the conversion

                    // first we should reach default unit
                    UnitPath path = this.PathToDefaultUnit();

                    //then test the system of the current unit if it was other than Metric.SI
                    //    then we must jump to SI otherwise we are already in default SI
                    if (this.UnitSystem == "Metric.SI" && this.UnitExponent == 1)
                    {
                        
                        //because no unit in SI with exponent = 1 don't have direct unit type
                        throw new NotImplementedException("Impossible reach by logic");
                    

                    }
                    else
                    {
                        // We should cross the system boundary 
                        UnitPathItem DefaultPItem;
                        UnitPathItem RefUPI;

                        DefaultPItem = path.Peek();
                        if (DefaultPItem.Unit.ReferenceUnit != null)
                        {
                            RefUPI = new UnitPathItem
                                {
                                    Numerator = DefaultPItem.Unit.ReferenceUnitNumerator,
                                    Denumenator = DefaultPItem.Unit.ReferenceUnitDenominator,
                                    //Shift = DefaultPItem.Unit.ReferenceUnitShift,
                                    Unit = DefaultPItem.Unit.ReferenceUnit
                                };

                            path.Push(RefUPI);
                        }
                    }

                    
                    return path;
                    
                }
                else
                {

                    Unit SIUnit = (Unit)Activator.CreateInstance(InnerUnitType);
                    SIUnit.UnitExponent = this.UnitExponent;
                    SIUnit.UnitDimension = this.UnitDimension;

                    UnitPath up = this.PathToUnit(SIUnit);

                    if (!SIUnit.IsBaseUnit)
                    {
                        if (SIUnit.UnitDimension.IsDimensionless && SIUnit.IsStronglyTyped)
                        {
                            //for dimensionless units like radian, stradian
                            //do nothing.
                        }
                        else
                        {

                            //expand the unit 
                            Unit expandedUnit = ExpandMetricUnit((MetricUnit)SIUnit);
                            UnitPath expath = expandedUnit.PathToSIBaseUnits();

                            while (expath.Count > 0)
                                up.Push(expath.Pop());
                        }

                    }

                    return up;

                }



            }
            
        
            UnitPath Pathes = new UnitPath();
            foreach (Unit un in this.SubUnits)
            {
                UnitPath up = null;

                up = un.PathToSIBaseUnits();

                while (up.Count > 0)
                {


                    UnitPathItem upi = up.Pop();

                    if (un.IsInverted) upi.Invert();

                    Pathes.Push(upi);
                }

            }
            return Pathes;

        }

        
    }
}
