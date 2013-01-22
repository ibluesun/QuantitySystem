using System;

namespace QuantitySystem.Units
{
    public class UnitsNotDimensionallyEqualException : UnitException
    {
      public UnitsNotDimensionallyEqualException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public UnitsNotDimensionallyEqualException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public UnitsNotDimensionallyEqualException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
    

    }
}
