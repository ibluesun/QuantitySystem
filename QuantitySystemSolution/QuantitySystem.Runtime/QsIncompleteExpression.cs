using System;
using System.Runtime.Serialization;

namespace Qs
{
    public class QsIncompleteExpression : QsException
    {
      public QsIncompleteExpression()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public QsIncompleteExpression(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public QsIncompleteExpression(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
  
    }
}
