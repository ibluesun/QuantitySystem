using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem
{
    public static class DynamicQuantitySystem
    {
        /// <summary>
        /// Holds number of arbitary functions that give the conversion factor to specific unit types.
        /// has been created for the currency conversion.
        /// </summary>
        internal static Dictionary<string, Func<string, double>> DynamicSourceFunctions = new Dictionary<string, Func<string, double>>();
        public static string[] SourceFunctionNames
        {
            get
            {
                return DynamicSourceFunctions.Keys.ToArray();
            }
        }

        public static void AddDynamicUnitConverterFunction(string name, Func<string, double> converter)
        {
            DynamicSourceFunctions[name] = converter;
        }


    }
}
