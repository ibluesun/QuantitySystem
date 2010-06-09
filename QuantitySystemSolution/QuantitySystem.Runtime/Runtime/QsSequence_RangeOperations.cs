using System;
using System.Collections.Generic;
using System.Globalization;
using Qs.Types;

namespace Qs.Runtime
{
    public partial class QsSequence : SortedList<int, QsSequenceElement>, IEnumerable<QsValue>
    {


        #region Higher Sequence Manipulation functions (Summation ++, Multiplication **, Range ..).

        /// <summary>
        /// Check if toIndex is greater than fromIndex and swap if not.
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        private void FixIndices(ref int fromIndex, ref int toIndex)
        {
            if (fromIndex > toIndex)
            {
                int f = fromIndex;
                fromIndex = toIndex;
                toIndex = f;
            }
       }

        /// <summary>
        /// Replace elements with operation and replace index variable with indexing number.
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private string JoinElementsWithOperation(int fromIndex, int toIndex, string operation)
        {
            string FunctionBody = string.Empty;
            int counterIndex = fromIndex;
            bool still = true;
            while (still)
            {
                var e = GetElement(counterIndex);

                string se_text = e.ElementDeclaration.Replace("$" + this.SequenceIndexName, "`");
                se_text = se_text.Replace(this.SequenceIndexName, counterIndex.ToString(CultureInfo.InvariantCulture));
                se_text = se_text.Replace("`", "$" + this.SequenceIndexName);

                if (!string.IsNullOrEmpty(SequenceRangeStartName))
                {
                    se_text = se_text.Replace("$" + SequenceRangeStartName, "`");
                    se_text = se_text.Replace(SequenceRangeStartName, StartIndex.ToString(CultureInfo.InvariantCulture));
                    se_text = se_text.Replace("`", "$" + SequenceRangeStartName);
                }

                if (!string.IsNullOrEmpty(SequenceRangeEndName))
                {
                    se_text = se_text.Replace("$" + SequenceRangeEndName, "`");
                    se_text = se_text.Replace(SequenceRangeEndName, EndIndex.ToString(CultureInfo.InvariantCulture));
                    se_text = se_text.Replace("`", "$" + SequenceRangeEndName);
                }

                FunctionBody += "(" + se_text + ")";

                if (fromIndex > toIndex)
                {
                    counterIndex--;
                    if (counterIndex < toIndex) still = false;
                }
                else
                {
                    counterIndex++;
                    if (counterIndex > toIndex) still = false;
                }
                if (still)
                    FunctionBody += " " + operation + " ";
            }
            return FunctionBody;
        }


        /// <summary>
        /// returns elements in array
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public QsSequenceElement[] GetElementsValuesInArray(int fromIndex, int toIndex)
        {
            List<QsSequenceElement> Elements = new List<QsSequenceElement>();

            int counterIndex = fromIndex;
            bool still = true;
            while (still)
            {
                Elements.Add(GetElement(counterIndex));

                if (fromIndex > toIndex)
                {
                    counterIndex--;
                    if (counterIndex < toIndex) still = false;
                }
                else
                {
                    counterIndex++;
                    if (counterIndex > toIndex) still = false;
                }
                if (still)
                {
                }
            }

            return Elements.ToArray();
        }

        /// <summary>
        /// The method sum all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i++k]
        /// </summary>
        public QsValue SumElements(int fromIndex, int toIndex)
        {
            

            if (this.Parameters.Length > 0)
            {
                // this is a call to form symbolic element
                // like g[n](x) ..> x^n
                // and calling g[0++2] 
                //  the output should be x^0 + x^1 + x^2
                //  and be parsed into function  (QsFunction)


                string porma = string.Empty;  // the parameters separated by comma ','
                foreach (var prm in this.Parameters)
                {
                    porma += prm.Name + ", ";
                }
                porma = porma.TrimEnd(',', ' ');

                string FunctionBody = JoinElementsWithOperation(fromIndex, toIndex, "+");

                string FunctionDeclaration = "_(" + porma + ") = " + FunctionBody;


                QsFunction qs = QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, FunctionDeclaration);
                
                return qs;
            }
            else
            {
                FixIndices(ref fromIndex, ref toIndex);

                QsValue Total = GetElementValue(fromIndex);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total = Total + GetElementValue(i);
                }

                
                return Total;
            }
        }

        #region SumElements Functions
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0);
            }
            
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1);
            }
            
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2);
            }
            
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3);
            }
            
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4);
            }
            
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }
            
            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
            
            return Total;
        }
        #endregion



        /// <summary>
        /// The method multiply all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i**k]
        /// </summary>
        public QsValue MulElements(int fromIndex, int toIndex)
        {
            

            if (this.Parameters.Length > 0)
            {
                // this is a call to form symbolic element
                // like g[n](x) ..> x^n
                // and calling g[0++2] 
                //  the output should be x^0 + x^1 + x^2
                //  and be parsed into function  (QsFunction)


                string porma = string.Empty;  // the parameters separated by comma ','
                foreach (var prm in this.Parameters)
                {
                    porma += prm.Name + ", ";
                }
                porma = porma.TrimEnd(',', ' ');

                string FunctionBody = JoinElementsWithOperation(fromIndex, toIndex, "*");

                string FunctionDeclaration = "_(" + porma + ") = " + FunctionBody;

                QsFunction qs = QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, FunctionDeclaration);
                
                return qs;
            }
            else
            {
                FixIndices(ref fromIndex, ref toIndex);

                QsValue Total = (QsValue)GetElementValue(fromIndex);

                for (int i = fromIndex + 1; i <= toIndex; i++)
                {
                    Total = Total * (QsValue)GetElementValue(i);
                }

                
                return Total;
            }
        }

        #region MulElements Functions
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0);
            }
            
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1);
            }
            
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2);
            }
            
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3);
            }
            
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4);
            }
            
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }
            
            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            

            FixIndices(ref fromIndex, ref toIndex);

            QsValue Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (int i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }
            
            return Total;
        }
        #endregion



        /// <summary>
        /// This is a tricky functions
        /// it returns Vector if components are Scalars.
        /// Matrix if components are Vectors
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public QsValue QsValueElements(int fromIndex, int toIndex)
        {
            if (this.Parameters.Length > 0)
            {
                List<string> ProcessedElements = new List<string>();

                #region symbolic representation

                if (fromIndex > toIndex)
                {
                    for (int e_ix = fromIndex; e_ix >= toIndex; e_ix--)
                    {
                        var se = GetElement(e_ix);
                        
                        // s[n](x) ..> $x*x^n+$n     # symbolic variables shouldn't be changed ($x, $n) we should take care.

                        // first preserve the symbolic variable with the same index name that we are going to change.
                        string se_text = se.ElementDeclaration.Replace("$" + this.SequenceIndexName, "`");

                        // replace the index name with the 
                        se_text = se_text.Replace(this.SequenceIndexName, e_ix.ToString(CultureInfo.InvariantCulture));

                        // get back the symbolic 
                        se_text = se_text.Replace("`", "$" + this.SequenceIndexName);

                        if (!string.IsNullOrEmpty(SequenceRangeStartName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeStartName, "`");
                            se_text = se_text.Replace(SequenceRangeStartName, StartIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeStartName);
                        }

                        if (!string.IsNullOrEmpty(SequenceRangeEndName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeEndName, "`");
                            se_text = se_text.Replace(SequenceRangeEndName, EndIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeEndName);
                        }


                        // replace the parameters in declaration with the same 
                        foreach (var param in this.Parameters)
                        {
                            se_text = se_text.Replace("$" + param.Name, "`");
                            
                            se_text = se_text.Replace(param.Name, "$" + param.Name);

                            se_text = se_text.Replace("`" , "$" + param.Name);

                        }

                        ProcessedElements.Add(se_text);
                    }
                }
                else
                {
                    for (int e_ix = fromIndex; e_ix <= toIndex; e_ix++)
                    {
                        var se = GetElement(e_ix);

                        string se_text = se.ElementDeclaration.Replace("$" + this.SequenceIndexName, "`");
                        se_text = se_text.Replace(this.SequenceIndexName, e_ix.ToString(CultureInfo.InvariantCulture));
                        se_text = se_text.Replace("`", "$" + this.SequenceIndexName);
                        if (!string.IsNullOrEmpty(SequenceRangeStartName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeStartName, "`");
                            se_text = se_text.Replace(SequenceRangeStartName, StartIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeStartName);
                        }

                        if (!string.IsNullOrEmpty(SequenceRangeEndName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeEndName, "`");
                            se_text = se_text.Replace(SequenceRangeEndName, EndIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeEndName);
                        }


                        // replace the parameters with names
                        foreach (var param in this.Parameters)
                        {
                            se_text = se_text.Replace("$" + param.Name, "`");

                            se_text = se_text.Replace(param.Name, "$" + param.Name);

                            se_text = se_text.Replace("`", "$" + param.Name);

                        }

                        ProcessedElements.Add(se_text);
                    }
                }
                var ee = QsEvaluator.CurrentEvaluator.SilentEvaluate(ProcessedElements[0]);
                QsValue Total;

                if (ee is QsScalar)
                {
                    Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);
                    foreach (string pel in ProcessedElements)
                        ((QsVector)Total).AddComponent((QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(pel));
                }
                else if (ee is QsVector)
                {
                    Total = new QsMatrix();
                    foreach (string pel in ProcessedElements)
                        ((QsMatrix)Total).AddVector((QsVector)QsEvaluator.CurrentEvaluator.SilentEvaluate(pel));
                }
                else if (ee is QsMatrix)
                {
                    Total = new QsTensor();
                    foreach (string pel in ProcessedElements)
                        ((QsTensor)Total).AddMatrix((QsMatrix)QsEvaluator.CurrentEvaluator.SilentEvaluate(pel));
                }
                else
                {
                    
                    throw new QsException("This is enough, no more than matrix values please.");
                }

                

                #endregion

                
                return Total;
            }

            QsValue firstElement = (QsValue)GetElementValue(fromIndex);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);
  
                
                #region Numerical Representation
                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i));
                    }
                }
                #endregion

                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i));
                    }
                }

                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }

        #region QsValueElements Functions
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0)
        {
            

            QsValue firstElement = GetElementValue(fromIndex, arg0);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0));
                    }
                }

                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0));
                    }
                }

                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1)
        {
            

            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }

        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2)
        {
            

            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3)
        {
            

            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }

                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }

        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4)
        {
            

            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }

        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5)
        {
            

            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsValue arg0, QsValue arg1, QsValue arg2, QsValue arg3, QsValue arg4, QsValue arg5, QsValue arg6)
        {
            

            QsValue firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            if (firstElement is QsScalar)
            {
                //return vector
                QsVector Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsVector)
            {
                //return vector
                QsMatrix Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (int i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                else
                {
                    for (int i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                
                return Total;
            }
            else if (firstElement is QsMatrix)
            {
                
                throw new NotImplementedException();
            }
            else
            {
                
                throw new NotSupportedException();
            }
        }


        #endregion

        #endregion

    }
}
