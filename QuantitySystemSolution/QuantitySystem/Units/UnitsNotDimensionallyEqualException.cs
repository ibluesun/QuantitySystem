using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace QuantitySystem.Units
{
    [Serializable()]
    public class UnitsNotDimensionallyEqualException : UnitException
    {
      public UnitsNotDimensionallyEqualException()
      {
         // Add any type-specific logic, and supply the default message.
      }

      public UnitsNotDimensionallyEqualException(string message): base(message) 
      {
         // Add any type-specific logic.
      }
      public UnitsNotDimensionallyEqualException(string message, Exception innerException): 
         base (message, innerException)
      {
         // Add any type-specific logic for inner exceptions.
      }
      protected UnitsNotDimensionallyEqualException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
      {
         // Implement type-specific serialization constructor logic.
      }        

    }
}
