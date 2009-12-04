using System;
using System.Runtime.Serialization;

namespace Qs
{
    [Serializable()]
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
        protected QsException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
        {
         // Implement type-specific serialization constructor logic.
        }

        public string ExtraData { get; set; }
    }
}
