using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using System.Globalization;

namespace Qs.Runtime
{
    /// <summary>
    /// Matrix that hold quantities
    /// and the basic matrix calculations.
    /// </summary>
    public partial class QsMatrix
    {
        List<AnyQuantity<double>[]> Rows = new List<AnyQuantity<double>[]>();

        public void AddRow(params AnyQuantity<double>[] row)
        {
            if (Rows.Count > 0) //test if the matrix initialized.
            {
                if (row.Length != ColumnsCount)
                {
                    throw new QsMatrixException("Added row columns [" + row.Length.ToString(CultureInfo.InvariantCulture) + "] is different than the matrix column count [" + ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
                }
            }

            Rows.Add(row);
        }

        public void AddColumn(params AnyQuantity<double>[] column)
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
                    var cols = Rows[IY];
                    Array.Resize<AnyQuantity<double>>(ref cols, newIndex);

                    
                    cols[newIndex-1] = column[IY];

                    Rows[IY] = cols;

                }
            }
            else
            {

                foreach (AnyQuantity<double> q in column)
                {
                    AddRow(q);
                }
            }
        }

        public int RowsCount
        {
            get
            {
                return Rows.Count;
            }
        }

        public int ColumnsCount
        {
            get
            {
               return Rows[0].Length;
            }
        }

        public AnyQuantity<double> this[int row, int column]
        {
            get
            {
                return Rows[row][column];
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
                    List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

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
                    List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

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
        /// Naive implementation :D  and I am proud :P
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

                    AnyQuantity<double>[] tvec = new AnyQuantity<double>[matrix.ColumnsCount]; //the target row in the Total Matrix.

                    //loop through all co vectors in the target matrix.
                    for (int tix = 0; tix < matrix.ColumnsCount; tix++)
                    {
                        var covec = matrix.GetCoVector(tix).GetRawQuantities();

                        //multiply vec*covec and store it at the Total matrix at iy,ix

                        AnyQuantity<double>[] snum = new AnyQuantity<double>[vec.Length];
                        for (int i = 0; i < vec.Length; i++)
                        {
                            snum[i] = vec[i] * covec[i];
                        }

                        AnyQuantity<double> tnum = snum[0];

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


        public QsMatrix Transpose()
        {
            QsMatrix m = new QsMatrix();
            for (int IX = 0; IX < ColumnsCount; IX++)
            {
                var vec = this.GetCoVector(IX);

                m.AddRow(vec.GetRawQuantities());
            }
            return m;
        }



        #endregion


        #region Scalar operations

        /// <summary>
        /// Matrix + scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix AddMatrixToScalar(AnyQuantity<double> scalarQuantity)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] + scalarQuantity);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;

        }


        /// <summary>
        /// Scalar + matrix
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix AddScalarToMatrix(AnyQuantity<double> scalarQuantity)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(scalarQuantity + this[IY, IX] );
                }

                Total.AddRow(row.ToArray());
            }
            return Total;

        }


        /// <summary>
        /// Matrix * scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix MultiplyMatrixByScalar(AnyQuantity<double> scalarQuantity)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] * scalarQuantity);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;

        }
        
        /// <summary>
        /// Matrix / scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix DivideMatrixByScalar(AnyQuantity<double> scalarQuantity)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] / scalarQuantity);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;

        }


        /// <summary>
        /// Scalar / Matrix
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix DivideScalarByMatrix(AnyQuantity<double> scalarQuantity)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(scalarQuantity / this[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;

        }

        /// <summary>
        /// Matrix - scalar
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix SubtractMatrixFromScalar(AnyQuantity<double> scalarQuantity)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] - scalarQuantity);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        /// <summary>
        /// Scalar - Matrix.
        /// </summary>
        /// <param name="scalarQuantity"></param>
        /// <returns></returns>
        public QsMatrix SubtractScalarFromMatrix(AnyQuantity<double> scalarQuantity)
        {
            QsMatrix Total = new QsMatrix();
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                List<AnyQuantity<double>> row = new List<AnyQuantity<double>>(ColumnsCount);

                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    row.Add(scalarQuantity - this[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }
        
        #endregion

        #region Manipulation
        public QsMatrix GetVector(int rowIndex)
        {
            if (rowIndex > RowsCount) throw new QsMatrixException("Index '" + rowIndex + "' Exceeds the rows limits '" + RowsCount + "'");
            QsMatrix rt = new QsMatrix();

            rt.AddRow(this.Rows[rowIndex]);

            return rt;

        }

        public QsMatrix GetCoVector(int columnIndex)
        {
            if (columnIndex > ColumnsCount) throw new QsMatrixException("Index '" + columnIndex + "' Exceeds the columns limits '" + ColumnsCount + "'");
            QsMatrix rt = new QsMatrix();

            AnyQuantity<double>[] col = new AnyQuantity<double>[this.RowsCount];
            int nc=0;
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                col[nc] = this[IY, columnIndex];
                nc++;
            }

            rt.AddColumn(col);


            return rt;
        }

        /// <summary>
        /// returns all quantities in the matrix in <see cref="AnyQuantity<double>"/> array.
        /// starting from left to right then by going down.
        /// </summary>
        /// <returns></returns>
        public AnyQuantity<double>[] GetRawQuantities()
        {
            AnyQuantity<double>[] values = new AnyQuantity<double>[ColumnsCount * RowsCount];

            int valIndex=0;
            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                for (int IX = 0; IX < this.ColumnsCount; IX++)
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

            for (int IY = 0; IY < this.RowsCount; IY++)
            {
                
                for (int IX = 0; IX < this.ColumnsCount; IX++)
                {
                    string cell = this[IY,IX].ToShortString();
                    sb.Append(cell);
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
