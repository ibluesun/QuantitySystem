using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Qs.Types
{
    public class QsMatrixException : QsException
    {
      public QsMatrixException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QsMatrixException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QsMatrixException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
 
    }
}
