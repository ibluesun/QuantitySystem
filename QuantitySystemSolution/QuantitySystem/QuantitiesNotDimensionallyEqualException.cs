using System;

namespace QuantitySystem
{
    public class QuantitiesNotDimensionallyEqualException : QuantityException
    {
        public QuantitiesNotDimensionallyEqualException()
        {
            // Add any type-specific logic, and supply the default message.
        }

        public QuantitiesNotDimensionallyEqualException(string message): base(message) 
        {
            // Add any type-specific logic.
        }

        public QuantitiesNotDimensionallyEqualException(string message, Exception innerException): 
            base (message, innerException)
        {
            // Add any type-specific logic for inner exceptions.
        }
    }
}
