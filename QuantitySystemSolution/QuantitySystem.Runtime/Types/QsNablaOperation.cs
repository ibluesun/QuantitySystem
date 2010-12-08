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
        /// Returns vector.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue MultiplyOperation(QsValue value)
        {
            var fscalar = value as QsScalar;

            if (!Object.ReferenceEquals(fscalar, null))
            {
                if (!Object.ReferenceEquals(fscalar.FunctionQuantity, null))
                {
                    var f = fscalar.FunctionQuantity.Value;
                    
                    string[] prms = f.ParametersNames;
                    
                    SymbolicAlgebra.SymbolicVariable fsv = f.SymbolicBodyScalar.SymbolicQuantity.Value;

                    QsVector Gradient = new QsVector();

                    foreach (string prm in prms)
                    {
                        Gradient.AddComponent(fsv.Differentiate(prm).ToQuantity().ToScalar());
                    }

                    return Gradient;
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
