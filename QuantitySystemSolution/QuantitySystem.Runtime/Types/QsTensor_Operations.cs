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

        public override QsValue AddOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = value as QsScalar;

                QsTensor NewTensor = new QsTensor();
                if (this.Order > 3)
                {
                    foreach (var iTensor in this.InnerTensors)
                    {
                        NewTensor.AddInnerTensor((QsTensor)iTensor.AddOperation(value));
                    }
                }
                else
                {
                    for (int il = 0; il < this.MatrixLayers.Count; il++)
                    {
                        QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].AddOperation(scalar);
                        NewTensor.AddMatrix(ResultMatrix);
                    }
                }
                return NewTensor;
            }

            if (value is QsTensor)
            {
                var tensor = value as QsTensor;
                if (this.Order != (tensor.Order)) throw new QsException("Adding two different ranked tensors are not supported");

                if (this.Order > 3)
                {
                    QsTensor NewTensor = new QsTensor();
                    for (int i=0; i< this.InnerTensors.Count(); i++)
                    {
                        var iTensor = this.InnerTensors[i];
                        NewTensor.AddInnerTensor((QsTensor)
                            iTensor.AddOperation(tensor.InnerTensors[i]));
                    }
                    return NewTensor;
                }
                else
                {
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
                                    QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].AddOperation(tensor.MatrixLayers[il]);
                                    NewTensor.AddMatrix(ResultMatrix);
                                }
                                return NewTensor;
                            }
                        }
                    }
                }
            }

            throw new QsException("Summing Operation with " + value.GetType().Name + " Failed");
        }

        public override QsValue SubtractOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = value as QsScalar;

                QsTensor NewTensor = new QsTensor();
                if (this.Order > 3)
                {
                    foreach (var iTensor in this.InnerTensors)
                    {
                        NewTensor.SubtractOperation((QsTensor)iTensor.AddOperation(value));
                    }
                }
                else
                {
                    for (int il = 0; il < this.MatrixLayers.Count; il++)
                    {
                        QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].SubtractOperation(scalar);
                        NewTensor.AddMatrix(ResultMatrix);
                    }
                }
                return NewTensor;
            }

            if (value is QsTensor)
            {
                var tensor = value as QsTensor;
                if (this.Order != (tensor.Order)) throw new QsException("Adding two different ranked tensors are not supported");

                if (this.Order > 3)
                {
                    QsTensor NewTensor = new QsTensor();
                    for (int i = 0; i < this.InnerTensors.Count(); i++)
                    {
                        var iTensor = this.InnerTensors[i];
                        NewTensor.AddInnerTensor((QsTensor)
                            iTensor.SubtractOperation(tensor.InnerTensors[i]));
                    }
                    return NewTensor;
                }
                else
                {
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
            }

            throw new QsException("Tensor Subtract Operation with " + value.GetType().Name + " Failed");
        }

        public override QsValue MultiplyOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = (QsScalar)value;

                QsTensor NewTensor = new QsTensor();
                if (this.Order > 3)
                {
                    foreach (var iTensor in this.InnerTensors)
                    {
                        NewTensor.AddInnerTensor((QsTensor)iTensor.MultiplyOperation(value));
                    }
                }
                else
                {
                    for (int il = 0; il < this.MatrixLayers.Count; il++)
                    {
                        QsMatrix ResultMatrix = (QsMatrix)this.MatrixLayers[il].MultiplyOperation(scalar);
                        NewTensor.AddMatrix(ResultMatrix);
                    }
                }

                return NewTensor;
            }

            if (value is QsTensor)
            {
                var tensor = (QsTensor) value;
                if (this.Order == 1 && tensor.Order == 1)
                {
                    var thisVec = this[0][0];
                    var thatVec = tensor[0][0];
                    var result = (QsMatrix)thisVec.TensorProductOperation(thatVec);
                    return new QsTensor(result);
                }
                if (this.Order == 2 && tensor.Order <= 2)
                {
                    //tenosrial product of two matrices will result in another matrix also.
                    QsMatrix result = (QsMatrix)this.MatrixLayers[0].TensorProductOperation(tensor.MatrixLayers[0]);

                    return new QsTensor(result);
                }
                else
                {
                    throw new QsException("", new NotImplementedException());
                }
            }

            throw new QsException("Tensor Multiplication Operation with " + value.GetType().Name + " Failed");
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

        public override QsValue DotProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            
            QsTensor tensor;

            if (value is QsTensor) tensor = (QsTensor)value;
            else if (value is QsMatrix) tensor = new QsTensor((QsMatrix)value);
            else if (value is QsVector) tensor = new QsTensor(new QsMatrix((QsVector)value));
            else if (value is QsScalar) tensor = new QsTensor(new QsMatrix(new QsVector((QsScalar)value)));
            else throw new QsException("Tensor Operation with non mathematical type is not permitted");
            
            if (tensor == null) throw new QsException("Must be a tensor for scalar product");

            if (this.Order > 3)
            {
                throw new NotImplementedException();
            }
            else if (this.Order == 1)
            {
                if (tensor.Order == 1)
                {
                    var thisVec = this[0][0];
                    var thatVec = tensor[0][0];
                    var result = (QsScalar)thisVec.DotProductOperation(thatVec);
                    return new QsTensor(new QsMatrix(new QsVector(result)));
                }
            }
            else if (this.Order == 2)
            {
                // only for tensors that looks like matrix.

                // reference:  http://people.rit.edu/pnveme/EMEM851n/constitutive/tensors_rect.html
                // the scalar product called there as double dot  ':'  and I don't know if this is right or wrong
                // 
                /* 
                 
                if (tensor.Order == 2)
                {
                    var matrix = tensor.MatrixLayers[0];
                    var thisMatrix = this.MatrixLayers[0];
                    if (thisMatrix.RowsCount == matrix.RowsCount && thisMatrix.ColumnsCount == matrix.ColumnsCount)
                    {
                        QsScalar Total = default(QsScalar);

                        for (int i = 0; i < thisMatrix.RowsCount; i++)
                        {
                            for (int j = 0; j < thisMatrix.ColumnsCount; j++)
                            {
                                if (Total == null) Total = thisMatrix[i, j] * matrix[j, i];
                                else Total = Total + thisMatrix[i, j] * matrix[j, i];
                            }
                        }
                        return Total;
                    }
                    else
                    {
                        throw new QsException("The two matrices should be equal in rows and columns");
                    }
                }
                */
            }
            
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

        public override QsValue GetIndexedItem(QsParameter[] allIndices)
        {
            int[] indices = new int[allIndices.Length];

            for (int ix = 0; ix < indices.Length; ix++) indices[ix] = (int)((QsScalar)allIndices[ix].QsNativeValue).NumericalQuantity.Value;                

            int dr = this.Order - indices.Count();
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
                if (this.Order == 2)
                {
                    return this[0][indices[0]];
                }
                else if (this.Order == 3)
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
                if (this.Order == 2)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue DifferentiateOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var t = new QsTensor();
                foreach (var m in t.MatrixLayers)
                {
                    t.AddMatrix((QsMatrix)m.DifferentiateOperation(value));
                }
                return t;
            }
            else
            {
                return base.DifferentiateOperation(value);
            }
        }
        #endregion

    }
}
