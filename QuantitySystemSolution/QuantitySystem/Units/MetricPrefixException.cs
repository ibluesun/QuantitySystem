using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace QuantitySystem.Units
{
    [Serializable()]
    public class MetricPrefixException : UnitException
    {
      public MetricPrefixException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public MetricPrefixException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public MetricPrefixException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
      protected MetricPrefixException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }

      public double WrongExponent { get; set; }
      public MetricPrefix CorrectPrefix { get; set; }
      public double OverflowExponent { get; set; }
    }
}
