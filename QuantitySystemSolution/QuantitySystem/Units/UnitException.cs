using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace QuantitySystem.Units
{
    [Serializable()]
    public class UnitException : Exception
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
      protected UnitException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }    
    }
}
