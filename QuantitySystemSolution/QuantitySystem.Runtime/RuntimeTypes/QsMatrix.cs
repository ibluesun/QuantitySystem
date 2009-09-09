﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using System.Globalization;

namespace Qs.RuntimeTypes
{
    /// <summary>
    /// Matrix that hold quantities
    /// and the basic matrix calculations.
    /// </summary>
    public partial class QsMatrix: QsValue
    {
        List<QsVector> Rows = new List<QsVector>();

        public QsMatrix()
        {
        }

        /// <summary>
        /// Initiate matrix with a set of vectors
        /// </summary>
        /// <param name="vectors"></param>
        public QsMatrix(params QsVector[] vectors)
        {
            Rows.AddRange(vectors);
        }

        /// <summary>
        /// Increase the matrix with a vector values.
        /// </summary>
        /// <param name="vector"></param>
        public void AddVector(QsVector vector)
        {
            Rows.Add(vector);
        }

        /// <summary>
        /// Increase the matrix with a column vector values.
        /// </summary>
        /// <param name="vector"></param>
        public void AddColumnVector(QsVector vector)
        {
            AddColumn(vector.ToArray());
        }

        /// <summary>
        /// Increase the matrix with a set of vectors.
        /// </summary>
        /// <param name="vectors"></param>
        public void AddVectors(params QsVector[] vectors)
        {
            Rows.AddRange(vectors);
        }


        /// <summary>
        /// Add a row of quantities to the matrix.
        /// </summary>
        /// <param name="row"></param>
        public void AddRow(params QsScalar[] row)
        {
            if (Rows.Count > 0) //test if the matrix initialized.
            {
                if (row.Length != ColumnsCount)
                {
                    throw new QsMatrixException("Added row columns [" + row.Length.ToString(CultureInfo.InvariantCulture) + "] is different than the matrix column count [" + ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
                }
            }

            QsVector vec = new QsVector();

            vec.AddComponents(row);

            Rows.Add(vec);
        }

        /// <summary>
        /// Add a column of quantities to the matrix.
        /// </summary>
        /// <param name="column"></param>
        public void AddColumn(params QsScalar[] column)
        {

            if (Rows.Count > 0) //test if the matrix initialized.
            {
                if (column.Length != RowsCount)
                {
                    throw new QsMatrixException("Added column rows [" + column.Length.ToString(CultureInfo.InvariantCulture) + "] is different than the matrix rows count [" + RowsCount.ToString(CultureInfo.InvariantCulture) + "]");
                }

                // vertically we will increase all columns of the inner array of every row.
                // this will be slow I think :)

                int newIndex = ColumnsCount+1;
                for (int IY = 0; IY < RowsCount; IY++)
                {
                    Rows[IY].AddComponent(column[IY]);
                }
            }
            else
            {

                foreach (var q in column)
                {
                    AddRow(q);
                }
            }
        }

        /// <summary>
        /// Count of the matrix rows {m}
        /// </summary>
        public int RowsCount
        {
            get
            {
                return Rows.Count;
            }
        }

        /// <summary>
        /// Count of the matrix columns {n}
        /// </summary>
        public int ColumnsCount
        {
            get
            {
               return Rows[0].Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public QsScalar this[int row, int column]
        {
            get
            {
                return Rows[row][column];
            }
            set
            {
                Rows[row][column] = value;
            }
        }


        #region Matrix Operations

        /// <summary>
        /// Matrix + Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix AddMatrix(QsMatrix matrix)
        {
            if (this.DimensionEquals(matrix))
            {
                QsMatrix Total = new QsMatrix();
                for (int IY = 0; IY < this.RowsCount; IY++)
                {
                    List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                    for (int IX = 0; IX < this.ColumnsCount; IX++)
                    {
                        row.Add(this[IY, IX] + matrix[IY, IX]);
                    }

                    Total.AddRow(row.ToArray());
                }
                return Total;
            }
            else
            {

                throw new QsMatrixException("Matrix 1 [" + this.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + this.ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                    "] is not dimensional equal with Materix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
            }
        }


        /// <summary>
        /// Element wise multiplcation of the matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix MultiplyMatrixByElements(QsMatrix matrix)
        {
            if (this.DimensionEquals(matrix))
            {
                QsMatrix Total = new QsMatrix();
                for (int IY = 0; IY < this.RowsCount; IY++)
                {
                    List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                    for (int IX = 0; IX < this.ColumnsCount; IX++)
                    {
                        row.Add(this[IY, IX] * matrix[IY, IX]);
                    }

                    Total.AddRow(row.ToArray());
                }
                return Total;
            }
            else
            {

                throw new QsMatrixException("Matrix 1 [" + this.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + this.ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                    "] is not dimensional equal with Materix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
            }
        }

        public QsMatrix DivideMatrixByElements(QsMatrix matrix)
        {
            if (this.DimensionEquals(matrix))
            {
                QsMatrix Total = new QsMatrix();
                for (int IY = 0; IY < this.RowsCount; IY++)
                {
                    List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                    for (int IX = 0; IX < this.ColumnsCount; IX++)
                    {
                        row.Add(this[IY, IX] / matrix[IY, IX]);
                    }

                    Total.AddRow(row.ToArray());
                }
                return Total;
            }
            else
            {

                throw new QsMatrixException("Matrix 1 [" + this.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + this.ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                    "] is not dimensional equal with Materix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
            }
        }


        /// <summary>
        /// Matrix - Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix SubtractMatrix(QsMatrix matrix)
        {
            if (this.DimensionEquals(matrix))
            {
                QsMatrix Total = new QsMatrix();
                for (int IY = 0; IY < this.RowsCount; IY++)
                {
                    List<QsScalar> row = new List<QsScalar>(ColumnsCount);

                    for (int IX = 0; IX < this.ColumnsCount; IX++)
                    {
                        row.Add(this[IY, IX] - matrix[IY, IX]);
                    }

                    Total.AddRow(row.ToArray());
                }
                return Total;
            }
            else
            {

                throw new QsMatrixException("Matrix 1 [" + this.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + this.ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                    "] is not dimensional equal with Matrix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                    + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
            }
        }


        /// <summary>
        /// Ordinary multiplicatinon of the matrix.
        /// Naive implementation :D  and I am proud :P
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix MultiplyMatrix(QsMatrix matrix)
        {
            if (this.ColumnsCount == matrix.RowsCount)
            {
                QsMatrix Total = new QsMatrix();

                //loop through my rows
                for (int iy = 0; iy < RowsCount; iy++)
                {
                    var vec = this.Rows[iy];

                    QsScalar[] tvec = new QsScalar[matrix.ColumnsCount]; //the target row in the Total Matrix.

                    //loop through all co vectors in the target matrix.
                    for (int tix = 0; tix < matrix.ColumnsCount; tix++)
                    {
                        var covec = matrix.GetColumnVectorMatrix(tix).ToArray();

                        //multiply vec*covec and store it at the Total matrix at iy,ix

                        QsScalar[] snum = new QsScalar[vec.Count];
                        for (int i = 0; i < vec.Count; i++)
                        {
                            snum[i] = vec[i] * covec[i];
                        }

                        QsScalar tnum = snum[0];

                        for (int i = 1; i < snum.Length; i++)
                        {
                            tnum = tnum + snum[i];
                        }

                        tvec[tix] = tnum;
                        
                    }

                    Total.AddRow(tvec);
                    
                }

                return Total;
            }
            else
            {
                throw new QsMatrixException("Width of the first matrix [" + this.ColumnsCount + "] not equal to the height of the second matrix [" + matrix.RowsCount + "]");
            }
        }

        /// <summary>
        /// Transfer the columns of the matrix into rows.
        /// </summary>
        /// <returns></returns>
        public QsMatrix Transpose()
        {
            QsMatrix m = new QsMatrix();
            for (int IX = 0; IX < ColumnsCount; IX++)
            {
                var vec = this.GetColumnVectorMatrix(IX);

                m.AddRow(vec.ToArray());
            }
            return m;
        }



        #endregion



        #region Manipulation
        public QsVector GetVector(int rowIndex)
        {
            if (rowIndex > RowsCount) throw new QsMatrixException("Index '" + rowIndex + "' Exceeds the rows limits '" + RowsCount + "'");

            return QsVector.CopyVector(Rows[rowIndex]);
        }


        public QsMatrix GetVectorMatrix(int rowIndex)
        {
            if (rowIndex > RowsCount) throw new QsMatrixException("Index '" + rowIndex + "' Exceeds the rows limits '" + RowsCount + "'");

            QsMatrix mat = new QsMatrix();

            mat.AddVector(QsVector.CopyVector(Rows[rowIndex]));

            return mat;
        }

        public QsVector GetColumnVector(int columnIndex)
        {
            if (columnIndex > ColumnsCount) throw new QsMatrixException("Index '" + columnIndex + "' Exceeds the columns limits '" + ColumnsCount + "'");


            QsScalar[] col = new QsScalar[this.RowsCount];
            int nc = 0;
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                col[nc] = this[IY, columnIndex];
                nc++;
            }
            QsVector vec = new QsVector(col);


            return vec;

        }

        public QsMatrix GetColumnVectorMatrix(int columnIndex)
        {
            QsMatrix mat = new QsMatrix();
            mat.AddColumnVector(GetColumnVector(columnIndex));
            return mat;
        }

        /// <summary>
        /// returns all quantities in the matrix in <see cref="AnyQuantity<double>"/> array.
        /// starting from left to right then by going down.
        /// </summary>
        /// <returns></returns>
        public QsScalar[] ToArray()
        {
            QsScalar[] values = new QsScalar[ColumnsCount * RowsCount];

            int valIndex=0;
            for (int IX = 0; IX < this.ColumnsCount; IX++)
            {
                for (int IY = 0; IY < this.RowsCount; IY++)
                {
                    values[valIndex] = this[IY, IX];
                    valIndex++;
                }
            }

            return values;

        }

        #endregion

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("QsMatrix:");
            sb.AppendLine();

            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                //sb.Append("\t");   
                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    string cell = this[IY,IX].Quantity.ToShortString();
                    
                    sb.Append(cell.PadLeft(13));
                    sb.Append(" ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        #region Matrix Attributes
        public bool IsVector
        {
            get
            {
                if (RowsCount == 1) return true;
                else return false;
            }
        }

        public bool IsCoVector
        {
            get
            {
                if (ColumnsCount == 1) return true;
                else return false;
            }
        }

        public bool IsScalar
        {
            get
            {
                if (RowsCount == 1 && ColumnsCount == 1) return true;
                else return false;
            }
        }

        public bool IsDeterminant
        {
            get
            {
                if (RowsCount == ColumnsCount) return true;
                else return false;
            }
        }

        /// <summary>
        /// Calculates the determinant and return it as a vector.
        /// </summary>
        /// <returns></returns>
        public QsVector Determinant()
        {
            if (IsDeterminant)
            {
                if(IsScalar)
                {
                    return new QsVector(this[0,0]);
                }
                else if (RowsCount == 2)
                {
                    QsVector vec = new QsVector(2);
                    vec.AddComponent(this[0, 0] * this[1, 1]);
                    vec.AddComponent(
                        new QsScalar
                        {
                            Quantity = "-1".ToQuantity() * (this[0, 1].Quantity * this[1, 0].Quantity)
                        }
                        );

                    return vec;
                }
                else if (RowsCount == 3)
                {
                    QsVector vec = new QsVector(3);

                    vec.AddComponent((this[1, 1] * this[2, 2]) - (this[1, 2] * this[2, 1]));

                    vec.AddComponent((this[1, 2] * this[2, 0]) - (this[1, 0] * this[2, 2]));

                    vec.AddComponent((this[1, 0] * this[2, 1]) - (this[1, 1] * this[2, 0]));

                    vec[0] = this[0, 0] * vec[0];

                    vec[1] = this[0, 1] * vec[1];

                    vec[2] = this[0, 2] * vec[2];

                    return vec;
                }
                else
                {
                    //I think this is the LU decomposition.
                    throw new QsMatrixException("Determinant of more than 3 elements not Implemented yet");
                }
                    
            }
            else throw new QsMatrixException("matrix is not determinant");
        }


        #endregion

        /// <summary>
        /// used to make sure that the two matrices are dimensionally equal
        /// have the same count of rows and columnss
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DimensionEquals(QsMatrix matrix)
        {
            if (matrix == null) return false;
            else
            {

                if (this.RowsCount == matrix.RowsCount)
                {
                    if (this.ColumnsCount == matrix.ColumnsCount)
                    {

                        return true;   //just for now.
                    }
                }
                //dimensions not equal.
                return false;
            }
        }


    }
}
