using System;
using System.Runtime.Serialization;

namespace Qs
{
    public class QsInvalidOperationException : QsException
    {
      public QsInvalidOperationException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QsInvalidOperationException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QsInvalidOperationException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
  
    }
}
