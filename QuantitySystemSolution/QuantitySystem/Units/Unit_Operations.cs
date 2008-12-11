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
        /// Get the path to the default unit.
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
                    RefTimesDen = RefUnit.ReferenceUnitDenumenator;

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
        /// Gets the Current unit path from default unit.
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
                    upi.Numerator = upi.Unit.ReferenceUnitDenumenator;  //invert the number
                    upi.Denumenator = upi.Unit.ReferenceUnitNumerator;
                }

                Backward.Push(upi);
            }

            return Backward;
        }


        public UnitPath PathFromUnit(Unit unit)
        {
            // 1- Get Path of Current Unit From Default

            UnitPath FromDefaultUnitToMe = this.PathFromDefaultUnit();

            // 2- Get Path of passed unit From Default unit

            UnitPath FromTargetUnitToDefaultUnit = unit.PathToDefaultUnit();

            // 3- check if the two units are in the same unit system

            if (this.UnitSystem == unit.UnitSystem)
            {
            }
            else
            {
                throw new NotImplementedException("Crossing boundary of unit system not yet supported.");
            }


            //combine the two paths
            UnitPath Total = new UnitPath();

            //begin from target unit
            while (FromTargetUnitToDefaultUnit.Count > 0)
            {
                Total.Push(FromTargetUnitToDefaultUnit.Pop());
            }


            while (FromDefaultUnitToMe.Count > 0)
            {
                Total.Push(FromDefaultUnitToMe.Pop());
            }

            return Total;
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
 
            }
            else
            {
                //then we must go out side the current unit system
                //all default units are pointing to the SIUnit system this is a must and not option.
                SystemsPath = new UnitPath();

                //get the default unit of target 

                throw new NotImplementedException("Crossing boundary of unit system not yet supported.");
            }


            //combine the two paths
            UnitPath Total = new UnitPath();

            //begin from me unit
            while (FromMeToDefaultUnit.Count > 0)
            {
                Total.Push(FromMeToDefaultUnit.Pop());
            }


            while (FromDefaultUnitToTargetUnit.Count > 0)
            {
                Total.Push(FromDefaultUnitToTargetUnit.Pop());
            }

            return Total;
        }

    }
}
