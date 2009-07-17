using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using QuantitySystem;

namespace Qs
{
    public static class Qs
    {
        public static ScriptRuntime CreateRuntime()
        {
            string[] QsNames = { "QuantitySystem", "Qs" };
            string[] QsExtensions = { ".Qs" };
            string QsType = typeof(Runtime.QsContext).FullName + ", " + typeof(Runtime.QsContext).Assembly.FullName;

            LanguageSetup QsSetup = new LanguageSetup(QsType, "Quantity System Calculator", QsNames, QsExtensions);

            ScriptRuntimeSetup srs = new ScriptRuntimeSetup();

            srs.LanguageSetups.Add(QsSetup);

            ScriptRuntime sr = new ScriptRuntime(srs);

            return sr;


        }

        #region Extension methods

        /// <summary>
        /// Quantitize the double value into DimensionlessQuantity
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this double d)
        {
            Unit un = Unit.DiscoverUnit(QuantityDimension.Dimensionless);
            return un.GetThisUnitQuantity<double>(d);
        }

        /// <summary>
        /// Quantitize the double value into AnyQuantity with the help of selected unit.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this double d, string unit)
        {
            Unit un = Unit.Parse(unit);
            
            return un.GetThisUnitQuantity<double>(d);
        }

        /// <summary>
        /// Quantitize the string into quantity based on its input of units.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static AnyQuantity<double> ToQuantity(this string s)
        {
            return Unit.ParseQuantity(s);

        }
 
        #endregion
    }
}
