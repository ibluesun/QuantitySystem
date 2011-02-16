using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;

namespace Qs.Types.Operators
{
    /// <summary>
    /// \/ is a very smart operator, it only applies to functions 
    /// and it only works on the 
    /// </summary>
    public class QsNablaOperation : QsOperation
    {
        /// <summary>
        /// \/ * value  operation is called gradient 
        ///     gradient over scalar field generate a vector 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue MultiplyOperation(QsValue value)
        {
            var fscalar = value as QsScalar;

            if (!Object.ReferenceEquals(fscalar, null))
            {

                // Here we will multiply the nabla \/*  with @function 
                if (!Object.ReferenceEquals(fscalar.FunctionQuantity, null))
                {
                    var f = fscalar.FunctionQuantity.Value;
                    

                    string[] prms = f.ParametersNames;
                    
                    SymbolicAlgebra.SymbolicVariable fsv = f.SymbolicBodyScalar.SymbolicQuantity.Value;

                    QsVector GradientResult = new QsVector();

                    // we loop through the symbolic body and differentiate it with respect to the function parameters.
                    // then accumulate the 
                    foreach (string prm in prms)
                    {
                        GradientResult.AddComponent(fsv.Differentiate(prm).ToQuantity().ToScalar());
                    }

                    return GradientResult;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// \/ . F  where F is a vector field and the return value should be scalar.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue DotProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return @"\/";
        }

        public override string  ToShortString()
        {
 	        return @"\/";
        }
    }
}
