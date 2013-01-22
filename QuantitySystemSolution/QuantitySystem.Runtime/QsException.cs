using System;
using System.Runtime.Serialization;

namespace Qs
{
    public class QsException : Exception
    {
        public QsException()
        {
         // Add any type-specific logic, and supply the default message.
        }

        public QsException(string message): base(message) 
        {
         // Add any type-specific logic.
        }
        public QsException(string message, Exception innerException): 
         base (message, innerException)
        {
         // Add any type-specific logic for inner exceptions.
        }


        public string ExtraData { get; set; }
    }
}
