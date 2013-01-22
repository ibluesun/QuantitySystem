using System;

namespace QuantitySystem
{
    public class QuantityNotFoundException : QuantityException
    {
      public QuantityNotFoundException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QuantityNotFoundException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QuantityNotFoundException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
    }
}
