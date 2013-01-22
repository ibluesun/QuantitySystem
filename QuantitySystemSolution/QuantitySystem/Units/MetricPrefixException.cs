using System;

namespace QuantitySystem.Units
{
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


      public double WrongExponent { get; set; }
      public MetricPrefix CorrectPrefix { get; set; }
      public double OverflowExponent { get; set; }
    }
}
