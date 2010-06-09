using QuantitySystem.Attributes;
using QuantitySystem.Quantities;


namespace QuantitySystem.Units.Metric
{
    /// <summary>
    /// Base unit for Hectare and Decare
    /// Hectare: by adding Hecto to Are
    /// Decare: by addin Deka to Are
    /// </summary>
    [MetricUnit("are", typeof(Area<>))]
    [ReferenceUnit(100)]
    public sealed class Are : MetricUnit
    {

    }

    /// <summary>
    /// Hectare by adding Hecto to Are
    /// </summary>
    [MetricUnit("ha", typeof(Area<>))]
    [ReferenceUnit(100, UnitType=typeof(Are))]
    public sealed class Hectare : MetricUnit
    {

    }

    /// <summary>
    /// Decare by addin Deka to Are
    /// </summary>
    [MetricUnit("decare", typeof(Area<>))]
    [ReferenceUnit(10, UnitType = typeof(Are))]
    public sealed class Decare : MetricUnit
    {

    }


    [MetricUnit("b", typeof(Area<>))]
    [ReferenceUnit(1E-28)]
    public sealed class Barn : MetricUnit
    {

    }


}