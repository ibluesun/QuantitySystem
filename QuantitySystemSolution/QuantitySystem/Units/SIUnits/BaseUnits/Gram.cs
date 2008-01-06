using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.UnitSystems;

namespace QuantitySystem.Units.SIUnits.BaseUnits
{
    public class Gram : SIUnit<Mass>
    {

        public Gram() : base(SIPrefix.Kilo) { }

        public override string Symbol
        {
            get
            {
                return base.Symbol + "g";
            }
        }

        public override bool IsBaseUnit
        {
            get
            {
                return true;
            }
        }



        public override double GetAbsoluteValue(double relativeValue)
        {
            //because this unit base is in KiloGram as SI standard
            // its absolute is always with kg
            // 1000000   /  1000  == 1000 
            // relative: 5 MegaGram = 5 * 1000; 5000 KiloGram

            double factor = Prefix.Factor / SIPrefix.Kilo.Factor;
            return relativeValue * factor;
        }

        public override double GetRelativeValue(double absoluteValue)
        {

            double factor = Prefix.Factor / SIPrefix.Kilo.Factor;

            return absoluteValue / factor;
        }
    }
}
