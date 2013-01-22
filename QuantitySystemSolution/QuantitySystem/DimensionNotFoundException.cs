using System;

namespace QuantitySystem
{
    public class DimensionNotFoundException : QuantityException
    {
      public DimensionNotFoundException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public DimensionNotFoundException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public DimensionNotFoundException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
    }
}
