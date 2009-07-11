namespace QuantitySystem.DimensionDescriptors
{

    public interface IDimensionDescriptor<TDimensionDescriptor>
    {
        float Exponent { get; set; }

        TDimensionDescriptor Add(TDimensionDescriptor dimensionDescriptor);
        
        TDimensionDescriptor Subtract(TDimensionDescriptor dimensionDescriptor);

        TDimensionDescriptor Multiply(float exponent);

        TDimensionDescriptor Invert();

    }


}
