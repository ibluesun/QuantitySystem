using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Qs.Runtime
{
    [Serializable()]
    public class QsVariableNotFoundException : QsException
    {
      public QsVariableNotFoundException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QsVariableNotFoundException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QsVariableNotFoundException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
      protected QsVariableNotFoundException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }



      public string Namespace { get; set; }
      public string Variable { get; set; }
    }



}
