using System;

namespace QuantitySystem
{
    public class QuantityException : Exception
    {
      public QuantityException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QuantityException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QuantityException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
    }
}
