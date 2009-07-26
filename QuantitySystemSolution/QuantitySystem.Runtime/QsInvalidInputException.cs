﻿using System;
using System.Runtime.Serialization;

namespace Qs
{
    [Serializable()]
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
      protected QsInvalidInputException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }    
    }
}