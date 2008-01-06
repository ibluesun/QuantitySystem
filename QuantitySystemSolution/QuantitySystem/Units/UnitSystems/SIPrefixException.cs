using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace QuantitySystem.Units.UnitSystems
{
    [Serializable()]
    public class SIPrefixException : Exception
    {
      public SIPrefixException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public SIPrefixException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public SIPrefixException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
      protected SIPrefixException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }  
    }
}
