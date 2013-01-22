using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Qs
{

    public class QsSyntaxErrorException : QsException
    {
              public QsSyntaxErrorException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QsSyntaxErrorException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QsSyntaxErrorException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
    }
}
