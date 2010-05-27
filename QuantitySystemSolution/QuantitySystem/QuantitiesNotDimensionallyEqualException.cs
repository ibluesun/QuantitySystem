using System;
using System.Runtime.Serialization;

namespace QuantitySystem
{
    [Serializable()]
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

        protected QuantitiesNotDimensionallyEqualException(SerializationInfo info, 
            StreamingContext context) : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}
