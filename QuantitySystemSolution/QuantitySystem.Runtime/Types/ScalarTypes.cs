using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Types
{
    /// <summary>
    /// Enum of various types 
    /// </summary>
    public enum ScalarTypes
    {
        /// <summary>
        /// Default behaviour of scalar values
        /// </summary>
        NumericalQuantity = 0 ,

        /// <summary>
        /// Complex number storage type.
        /// </summary>
        ComplexNumberQuantity = 10,

        /// <summary>
        /// Quanternion number storage type.
        /// </summary>
        QuaternionNumberQuantity = 20,

        /// <summary>
        /// Symbolic variable 
        /// </summary>
        SymbolicQuantity = 100,


        /// <summary>
        /// Function as a variable.
        /// </summary>
        FunctionQuantity = 300,



        /// <summary>
        /// @
        /// </summary>
        QsOperation = 400
    }
}
