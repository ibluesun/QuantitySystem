namespace QuantitySystem.DimensionDescriptors
{

    public interface IDimensionDescriptor<TDimensionDescriptor>
    {
        int Exponent { get; set; }

        TDimensionDescriptor Add(TDimensionDescriptor dimensionDescriptor);
        
        TDimensionDescriptor Subtract(TDimensionDescriptor dimensionDescriptor);

        TDimensionDescriptor Multiply(int exponent);

    }


}
