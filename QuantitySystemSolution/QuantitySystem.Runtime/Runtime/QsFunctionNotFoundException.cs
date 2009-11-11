using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Qs.Runtime
{
    [Serializable()]
    public class QsFunctionNotFoundException : QsException
    {
      public QsFunctionNotFoundException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QsFunctionNotFoundException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QsFunctionNotFoundException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
      protected QsFunctionNotFoundException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }    
    }
}
