using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace QuantitySystem
{
    [Serializable()]
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
      protected DimensionNotFoundException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }    
    }
}
