using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;
using SymbolicAlgebra;

namespace Qs.Types.Operators
{
    /// <summary>
    /// \/ is a very smart operator
    /// When \/ * Function  the operator will execute nabla based on the parameters of the function
    /// But if you del * vector  the nabla should be defined for this vector as \x y z/  or \r theta z/ etc.
    /// </summary>
    public class QsNablaOperation : QsOperation
    {
        public readonly string[] Coordinates;

        public QsNablaOperation(params string[] coordinates)
        {
            Coordinates = coordinates;
        }

        /// <summary>
        /// Returns the differential form of del vector based on coordinates specified.
        /// \x y z/  produce {@|$x @|$y @|$z}
        /// </summary>
        public QsVector DelVector
        {
            get
            {
                QsVector v = new QsVector(Coordinates.Length);

                foreach (var x in Coordinates)
                {
                    QsDifferentialOperation dpo = new QsDifferentialOperation();
                    
                    var dx = (QsScalar)dpo.DifferentiateOperation(new SymbolicVariable(x).ToQuantity().ToScalar());
                    if(Power>1)
                        for (int p = 1; p < Power; p++)
                        {
                            dx = (QsScalar)dx.DifferentiateOperation(new SymbolicVariable(x).ToQuantity().ToScalar());
                        }

                    v.AddComponent(dx);
                }

                return v;
            }
        }


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
                // Here we will multiply the nabla \/ *  with @function 
                if (!Object.ReferenceEquals(fscalar.FunctionQuantity, null))
                {
                    var f = fscalar.FunctionQuantity.Value;
                    
                    string[] prms = f.ParametersNames;

                    SymbolicVariable fsv = f.ToSymbolicVariable();

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
            if (value is QsVector)
            {
                return this.DelVector.MultiplyVector((QsVector)value);
            }
            
            throw new NotImplementedException(@"Multiplication of \/ * " + value.GetType().Name +" Not implemented yet");
            
        }


        /// <summary>
        /// \/ . F  where F is a vector field and the return value should be scalar.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue DotProductOperation(QsValue value)
        {
            if (value is QsVector)
            {
                return this.DelVector.DotProductOperation((QsVector)value);
            }

            throw new NotImplementedException(@"\/ . " + value.GetType().Name + " not implemented");
        }


        /// <summary>
        /// \/  x  F  is called curl of vector
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue CrossProductOperation(QsValue value)
        {
            if (value is QsVector)
            {
                return this.DelVector.CrossProductOperation((QsVector)value);
            }

            throw new NotImplementedException(@"\/ x " + value.GetType().Name + " not implemented");
        }


        private int Power = 1;
        public override QsValue PowerOperation(QsValue value)
        {
            int p = (int)((QsScalar)value).NumericalQuantity.Value;
            Power = p;
            return this;
        }

        public override string ToShortString()
        {
            string nb = @"\";

            string ccs = string.Empty;

            foreach (var s in Coordinates) ccs += s + " ";

            ccs = ccs.Trim();

            return nb + ccs + "/";
        }

        public override string ToString()
        {
            return ToShortString();
        }

    }
}
