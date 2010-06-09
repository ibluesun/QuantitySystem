using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs.Types
{
    public partial class QsTensor : QsValue
    {

        #region QsValue operations
        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        public override QsValue AddOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var scalar = value as QsScalar;
                QsTensor NewTensor = new QsTensor();
                for (int il = 0; il < this.MatrixLayers.Count; il++)
                {
                    QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].AddOperation(scalar);
                    NewTensor.AddMatrix(ResultMatrix);
                }

                return NewTensor;
            }

            if (value is QsTensor)
            {
                var tensor = value as QsTensor;
                if (tensor.MatrixLayers.Count == this.MatrixLayers.Count)
                {
                    // the operation will succeed
                    if (tensor.FaceRowsCount == this.FaceRowsCount)
                    {
                        if (tensor.FaceColumnsCount == this.FaceColumnsCount)
                        {
                            QsTensor NewTensor = new QsTensor();
                            for(int il=0; il < this.MatrixLayers.Count; il++)
                            {
                                QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].AddOperation(tensor.MatrixLayers[il]);
                                NewTensor.AddMatrix(ResultMatrix);
                            }

                            return NewTensor;
                        }
                    }
                }
            }

            throw new QsException("Tensor Operation Failed");
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            if (value is QsScalar)
            {
                var scalar = value as QsScalar;
                QsTensor NewTensor = new QsTensor();
                for (int il = 0; il < this.MatrixLayers.Count; il++)
                {
                    QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].SubtractOperation(scalar);
                    NewTensor.AddMatrix(ResultMatrix);
                }

                return NewTensor;
            }

            if (value is QsTensor)
            {
                var tensor = value as QsTensor;
                if (tensor.MatrixLayers.Count == this.MatrixLayers.Count)
                {
                    // the operation will succeed
                    if (tensor.FaceRowsCount == this.FaceRowsCount)
                    {
                        if (tensor.FaceColumnsCount == this.FaceColumnsCount)
                        {
                            QsTensor NewTensor = new QsTensor();
                            for (int il = 0; il < this.MatrixLayers.Count; il++)
                            {
                                QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].SubtractOperation(tensor.MatrixLayers[il]);
                                NewTensor.AddMatrix(ResultMatrix);
                            }

                            return NewTensor;
                        }
                    }
                }
            }

            throw new QsException("Tensor Operation Failed");
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

        public override QsValue GetIndexedItem(int[] indices)
        {
            int dr = this.Rank - indices.Count();
            if (dr < 0)
            {
                throw new QsException("Indices count exceed the tensor rank, only specify indices to the same rank to get scalar, or less to get vectors to tensors");
            }
            else if (dr == 0)
            {
                return GetScalar(indices);
            }
            else if (dr == 1)
            {
                // return vector
                if (this.Rank == 2)
                {
                    return this[0][indices[0]];
                }
                else if (this.Rank == 3)
                {
                    return this[indices[0]][indices[1]];
                }
                else
                {
                    QsTensor t = this;
                    int ix = 0;
                    int ic = indices.Count();
                    while (ix < ic - 2)
                    {
                        t = t.InnerTensors[indices[ix]];
                        ix++;
                    }
                    return t[indices[ix]][indices[ix + 1]];  //ix was increased the latest time.
                }
            }
            else if (dr == 2)
            {
                // return matrix;
                if (this.Rank == 2)
                {
                    return this[indices[0]];
                }
                else
                {
                    // specify the tensor
                    QsTensor t = this;
                    int ix = 0;
                    int ic = indices.Count();

                    while (ix < ic - 1)
                    {
                        t = t.InnerTensors[indices[ix]];
                        ix++;
                    }

                    // then return the matrix.
                    return t[indices[ix]];  //ix was increased the latest time.
                }
            }
            else
            {
                // return tensor
                QsTensor t = this;
                int ix = 0;
                while (ix < indices.Count())
                {
                    t = t.InnerTensors[indices[ix]];
                    ix++;
                }
                return t;
            }
        }
        #endregion

    }
}
