using System;
using System.Runtime.Serialization;

namespace Qs
{
    public class QsInvalidInputException : QsException
    {
      public QsInvalidInputException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QsInvalidInputException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QsInvalidInputException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
 
    }
}
