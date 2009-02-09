using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

using System.Reflection;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units
{
    public partial class Unit
    {

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
        /// Creat units path from the current unit instance to the default unit of the current unit system in the current quantity dimension.
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

                // do the iteration until we reach the default unit.
                while (RefUnit.DefaultUnit == false)
                {

                    path.Push(
                        new UnitPathItem
                        {
                            Unit = RefUnit,
                            Numerator = RefTimesNum,
                            Denumenator = RefTimesDen
                        }
                        );

                    RefTimesNum = RefUnit.ReferenceUnitNumerator;  //get the value before changing the RefUnit
                    RefTimesDen = RefUnit.ReferenceUnitDenominator;

                    RefUnit = RefUnit.ReferenceUnit;
                }

                path.Push(
                    new UnitPathItem
                    {
                        Unit = RefUnit,
                        Numerator = RefTimesNum,
                        Denumenator = RefTimesDen
                    }
                    );
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

                if (upi.Unit.DefaultUnit)
                {
                    upi.Numerator = 1;
                    upi.Denumenator = 1;
                }
                else
                {
                    upi.Numerator = upi.Unit.ReferenceUnitDenominator;  //invert the number
                    upi.Denumenator = upi.Unit.ReferenceUnitNumerator;
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



        /// <summary>
        /// Gets the path to the unit starting from current unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public UnitPath PathToUnit(Unit unit)
        {
            // 1- Get Path default unit to current unit.

            UnitPath FromMeToDefaultUnit = this.PathToDefaultUnit();

            // 2- Get Path From Default unit to the passed unit.

            UnitPath FromDefaultUnitToTargetUnit = unit.PathFromDefaultUnit();

            // 3- check if the two units are in the same unit system
            UnitPath SystemsPath = null;
            if (this.UnitSystem == unit.UnitSystem)
            {
                //no boundary cross should occur
            }
            else
            {
                //then we must go out side the current unit system
                //all default units are pointing to the SIUnit system this is a must and not option.
                SystemsPath = new UnitPath();

                //get the default unit of target 

                // throw new NotImplementedException("Crossing boundary of unit system not yet supported.");

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
                if (FromMeToDefaultUnit.Peek().Unit.UnitSystem.ToUpper() != "SI")
                {
                    DefaultPItem = FromMeToDefaultUnit.Peek();
                    RefUPI = new UnitPathItem
                        {
                            Numerator = DefaultPItem.Unit.ReferenceUnitNumerator,
                            Denumenator = DefaultPItem.Unit.ReferenceUnitDenominator,
                            Unit = DefaultPItem.Unit.ReferenceUnit
                        };                
                }
                else
                {
                    DefaultPItem = FromDefaultUnitToTargetUnit.ElementAt(FromDefaultUnitToTargetUnit.Count - 1);
                    RefUPI = new UnitPathItem
                    {
                        //note the difference here 
                        //I made the opposite assignments because we are in reverse manner

                        Numerator = DefaultPItem.Unit.ReferenceUnitDenominator,
                        Denumenator = DefaultPItem.Unit.ReferenceUnitNumerator,
                        Unit = DefaultPItem.Unit.ReferenceUnit
                    };                

                }



                SystemsPath.Push(RefUPI);

                
            }


            //combine the two paths
            UnitPath Total = new UnitPath();

            //we are building the conversion stairs
            // will end like a stack



            //begin from me unit
            for(int i=FromMeToDefaultUnit.Count-1;i>=0;i--)
            {
                Total.Push(FromMeToDefaultUnit.ElementAt(i));
            }

            if (SystemsPath != null)
            {
                for (int i = SystemsPath.Count - 1; i >= 0; i--)
                {
                    Total.Push(SystemsPath.ElementAt(i));
                }
            }

            for (int i = FromDefaultUnitToTargetUnit.Count - 1; i >= 0; i--)
            {
                Total.Push(FromDefaultUnitToTargetUnit.ElementAt(i));
            }




            return Total;
        }

    }
}
