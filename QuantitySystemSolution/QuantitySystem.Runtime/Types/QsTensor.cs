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

        protected List<QsMatrix> MatrixLayers = new List<QsMatrix>();


        /// <summary>
        /// As was indicated in wikipedia and some physics forms
        /// It is good to visualize the 4th Tensor as a hyper cube 
        /// Hyper cube is a structure with four indices 
        /// and it could be accessed by Einstein indexing method.
        /// ------------------------------
        /// However in the property we can store Sub Tensors which in turn can store sub tensors and etc.
        /// this will permit us to store more tensor ranks as we want 
        /// </summary>
        protected List<QsTensor> InnerTensors;


        /// <summary>
        /// Returns the current rank of this Tensor.
        /// </summary>
        public int Rank
        {
            get
            {
                if (InnerTensors == null)
                {

                    if (MatrixLayers.Count > 1) return 3;
                    else if (MatrixLayers.Count == 1)
                    {
                        var matrix = MatrixLayers[0];
                        if (matrix.RowsCount > 1) return 2;
                        else
                        {
                            // only one row then it is vector
                            var vector = matrix.Rows[0];
                            if (vector.Count > 1) return 1;
                            else return 0;   //scalar

                        }
                    }
                    else
                    {
                        throw new QsException("Tensor is not initialized yet");
                    }
                }
                else
                {
                    // innder tensors do exist
                    // make recursive call to obtain the tensor rank

                    int rank = this.InnerTensors[0].Rank;

                    return rank + 1;

                }
            }
        }

        public QsTensor()
        {
        }

        /// <summary>
        /// Form tensor from group of matrices.
        /// </summary>
        /// <param name="matrices"></param>
        public QsTensor(params QsMatrix[] matrices)
        {
            MatrixLayers.AddRange(matrices);
        }


        /// <summary>
        /// Count of the face matrix rows {m}
        /// </summary>
        public int FaceRowsCount
        {
            get
            {
                return MatrixLayers[0].RowsCount;
            }
        }

        /// <summary>
        /// Count of the face matrix columns {n}
        /// </summary>
        public int FaceColumnsCount
        {
            get
            {
                return MatrixLayers[0].ColumnsCount;
            }
        }


        /// <summary>
        /// Initiate adding tensor in current tensor 
        /// and increase the rank of the tensor
        /// most probably inserting tensor as a sub tensor will increase rank starting from 4th rank.
        /// </summary>
        /// <param name="qsTensor"></param>
        public void AddInnerTensor(QsTensor qsTensor)
        {
            if (qsTensor.Rank == 0)
            {
                if (MatrixLayers.Count == 0)
                {
                    var matrix = new QsMatrix(new QsVector(qsTensor.MatrixLayers[0][0, 0]));

                    MatrixLayers.Add(matrix);
                }
                else
                {
                    // successive insertion of zero rank tensor will be put into the vecor in the matrix in the tensor.
                    MatrixLayers[0].Rows[0].AddComponent(qsTensor.MatrixLayers[0][0, 0]);
                }
            }
            else if (qsTensor.Rank == 1)
            {
                if (MatrixLayers.Count == 0)
                {
                    var matrix = new QsMatrix(qsTensor.MatrixLayers[0].Rows[0]);
                    MatrixLayers.Add(matrix);
                }
                else
                {
                    // successive insertion of first rank tensor insert them in the matrix.

                    var v = qsTensor.MatrixLayers[0].Rows[0];

                    if (MatrixLayers.Count > 0)
                    {
                        if (v.Count != this.FaceColumnsCount)
                            throw new QsInvalidInputException("Adding first rank tensor with different number of columns");

                    }
                    MatrixLayers[0].AddVector(v);
                }
            }
            else if (qsTensor.Rank == 2)
            {
                var matrix = QsMatrix.CopyMatrix(qsTensor.MatrixLayers[0]);

                if (MatrixLayers.Count > 0)
                {
                    if (matrix.RowsCount == this.FaceRowsCount)
                    {
                        if (matrix.ColumnsCount == this.FaceColumnsCount)
                        {

                        }
                        else
                        {
                            throw new QsInvalidOperationException("Adding Second Rank tensor with different number of columns");
                        }
                    }
                    else
                    {
                        throw new QsInvalidOperationException("Adding Second Rank tensor with different number of rows");
                    }
                }
                MatrixLayers.Add(matrix);
            }
            else
            {
                // Third rank tensor or more 
                if (InnerTensors == null) InnerTensors = new List<QsTensor>();

                InnerTensors.Add(qsTensor);
            }

        }

        /// <summary>
        /// Add Matrix face to the current tensor
        /// </summary>
        /// <param name="qsMatrix"></param>
        public void AddMatrix(QsMatrix qsMatrix)
        {
            if (MatrixLayers.Count > 0)
            {
                if (qsMatrix.RowsCount == FaceRowsCount && qsMatrix.ColumnsCount == FaceColumnsCount)
                { }
                else
                {
                    throw new QsException("The matrix about to be added is not in the same dimension");
                }
            }

            MatrixLayers.Add(qsMatrix);
        }


        /// <summary>
        /// Only Applied for 3rd rank tensor
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public QsScalar this[int row, int column, int z]
        {
            get
            {
                
                return MatrixLayers[z][row,column];
            }
            set
            {
                MatrixLayers[z][row, column] = value;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string rankText = Rank.ToString();
            if (Rank == 1) rankText += "st";
            if (Rank == 2) rankText += "nd";
            if (Rank == 3) rankText += "rd";
            if (Rank > 3) rankText += "th";

            sb.Append("QsTensor: " + rankText + " Rank");
            sb.AppendLine();

            foreach (var mat in MatrixLayers)
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
