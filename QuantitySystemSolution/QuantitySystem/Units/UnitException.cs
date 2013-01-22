using System;

namespace QuantitySystem.Units
{
    public class UnitException : QuantityException
    {
      public UnitException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public UnitException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public UnitException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
     
    }
}
