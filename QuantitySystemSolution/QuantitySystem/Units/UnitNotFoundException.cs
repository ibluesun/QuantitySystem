using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace QuantitySystem.Units
{
    [Serializable()]
    public class UnitNotFoundException : UnitException
    {
      public UnitNotFoundException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public UnitNotFoundException(string unit)
          : base(unit + " Not found")
      {
      }

      public UnitNotFoundException(string message, string unit)
          : base(unit + " " + message)
      {
          // Add any type-specific logic.
      }
      public UnitNotFoundException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
      protected UnitNotFoundException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }    
    }
}
