using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Currency
{
    [DefaultUnit("$", typeof(Currency<>))]
    public sealed class Coin : Unit  
    {

        //public override bool IsDefaultUnit
        //{
        //    get
        //    {
                
        //        return true;
        //    }
        //}
    }


    //codes.Add("USD: United States Dollar");
    [Unit("USD", typeof(Currency<>))]
    [ReferenceUnit(1, UnitType = typeof(Coin))]
    public sealed class United_States_Dollar : Unit  
    {
    }

}
