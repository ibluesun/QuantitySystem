using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace Qs.QsTypes
{
    /// <summary>
    /// Gives the factorial of real numbers.
    /// should be complex number also but I will not include it
    /// ----------
    /// the implementation now is integer numbers.
    /// </summary>
    public static class Gamma
    {
        
        public static AnyQuantity<double> Factorial(AnyQuantity<double> Number)
        {
            
            int v = (int)Number.Value;

            if (v > 170) throw new NotImplementedException("Number Exceededs Factorial Limit > 170");

            var One = (AnyQuantity<double>)Number.Clone();

            One.Value = 1.0;

            AnyQuantity<double> num = (AnyQuantity<double>)Number.Clone();
            num.Value = Math.Floor(num.Value);

            AnyQuantity<double> Total = (AnyQuantity<double>)num.Clone();


            for (int i = v; i >1; i--)
            {
                num = num - One;
                Total = Total * num;
            }

            return Total;
        
        }

    }
}
