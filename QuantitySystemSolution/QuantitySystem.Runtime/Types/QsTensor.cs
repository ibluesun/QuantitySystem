using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Types
{

    /// <summary>
    /// Represeneted by &lt;&lt; list &gt;&gt;
    /// </summary>
    public partial class QsTensor : QsValue
    {

        /// <summary>
        /// Returns the current rank of this Tensor.
        /// </summary>
        public int Rank
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<QsMatrix> Layers = new List<QsMatrix>();

        public QsTensor()
        {
        }

        /// <summary>
        /// Form tensor from group of matrices.
        /// </summary>
        /// <param name="matrices"></param>
        public QsTensor(params QsMatrix[] matrices)
        {
            Layers.AddRange(matrices);
        }

        #region QsValue operations
        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        public override QsValue AddOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue DivideOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue PowerOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue ModuloOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue LeftShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }


        public override bool LessThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThan(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool LessThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool GreaterThanOrEqual(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool Equality(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override bool Inequality(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue DotProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue CrossProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue TensorProductOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue NormOperation()
        {
            throw new NotImplementedException();
        }

        public override QsValue AbsOperation()
        {
            throw new NotImplementedException();
        }

        #endregion



        /// <summary>
        /// Count of the face matrix rows {m}
        /// </summary>
        public int FaceRowsCount
        {
            get
            {
                return Layers[0].RowsCount;
            }
        }

        /// <summary>
        /// Count of the face matrix columns {n}
        /// </summary>
        public int FaceColumnsCount
        {
            get
            {
                return Layers[0].ColumnsCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qsMatrix"></param>
        public void AddMatrix(QsMatrix qsMatrix)
        {
            if (Layers.Count > 0)
            {
                if (qsMatrix.RowsCount == FaceRowsCount && qsMatrix.ColumnsCount == FaceColumnsCount)
                { }
                else
                {
                    throw new QsException("The matrix about to be added is not in the same dimension");
                }
            }

            Layers.Add(qsMatrix);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public QsScalar this[int row, int column, int z]
        {
            get
            {
                
                return Layers[z][row,column];
            }
            set
            {
                Layers[z][row, column] = value;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("QsTensor:");
            sb.AppendLine();

            foreach (var mat in Layers)
            {
                sb.Append(mat.MatrixText);
                sb.AppendLine();
            }
            return sb.ToString();
        }



        #region This region for rotation of tensor 
        // try to remember that rotation of tensor depend on the rank of the tensor
        //  I mean tensor of rank 0 (looks like scalar)  will have no dimention to rotate around
        //  tensor of 2nd rank will have two dimension to rotate around
        #endregion
    }
}
