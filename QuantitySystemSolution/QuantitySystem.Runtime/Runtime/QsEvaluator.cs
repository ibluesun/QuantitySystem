using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ParticleLexer;
using ParticleLexer.QsTokens;
using ParticleLexer.StandardTokens;
using Qs.Types;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using System.Text;
using System.Linq.Expressions;
using QsRoot;
using System.Globalization;


namespace Qs.Runtime
{

    /// <summary>
    /// used to evaluate the quantity system expressions.
    /// </summary>
    public class QsEvaluator
    {

        /// <summary>
        /// Match unit 
        /// </summary>
        public const string UnitExpression = @"^\s*<([\w\.\^\/!\$]+)>\s*$";

        /// <summary>
        /// Match unit to unit 
        /// </summary>
        public const string UnitToUnitExpression = @"^\s*<(.+)>\s*[tT][oO]\s*<(.+)>\s*$";

        public const string DoubleNumber = @"[-+]?\d+(\.\d+)*([eE][-+]?\d+)*?";

        public const string VariableQuantityExpression = @"^(\w+)\s*=\s*(" + DoubleNumber + @")\s*(\[(.+)\])";

#if WINRT
#else
        public const ConsoleColor BackgroundColor = ConsoleColor.White;
        public const ConsoleColor ForegroundColor = ConsoleColor.Black;
        public const ConsoleColor HelpColor = ConsoleColor.Blue;

        public const ConsoleColor ValuesColor = ConsoleColor.Green;
        public const ConsoleColor ExceptionColor = ConsoleColor.Red;
#endif

        public IEnumerable<string> VariablesKeys
        {
            get
            {
                if (Scope != null)
                {
                    var varo = from item in Scope.GetItems()
                               select item.Key;
                    return varo;
                }
                else
                {
                    throw new Exception("No Scope exist");
                }
            }
        }

        private QsScope _InternalScope = new QsScope();

        public QsScope Scope
        {
            get
            {
                return _InternalScope;
            }
        }



        public object GetVariable(string varName)
        {
            
            if (Scope != null)
            {
                object q;
                Scope.TryGetValue(varName, out q);
                return q;
            }
            else
            {
                throw new NotImplementedException("you should be running in DLR");
            }

        }

        public AnyQuantity<double> GetQuantity(string varName)
        {
            if (Scope != null)
            {
                object q;
                Scope.TryGetValue(varName, out q);
                return (AnyQuantity<double>)q;
            }
            else
            {
                throw new Exception("No Scope exist");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public void SetVariable(string varName, object varValue)
        {
            if (Scope != null)
            {
                var r = GetVariable(varName) as QsReference;
                if (r == null)
                    Scope.SetValue(varName, varValue);
                else
                    r.ContentValue = (QsValue)varValue;
            }
            else
            {
                throw new Exception("No Scope exist");
            }
        }

        public QsReference AddReference(string nameSpace, string varName, string referencedVariableName)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                QsReference qsr = new QsReference(referencedVariableName);
                SetVariable(varName, qsr);
                return qsr;
            }
            else
            {
                //try to set the property in the namespace

                QsNamespace ns = QsNamespace.GetNamespace(Scope, nameSpace, true);

                return ns.AddReference(varName, referencedVariableName);

            }            

        }

        /// <summary>
        /// Set the variable in the name space.
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public void SetVariable(string nameSpace, string varName, object varValue)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                SetVariable(varName, varValue);
            }
            else
            {
                //try to set the property in the namespace

                QsNamespace ns = QsNamespace.GetNamespace(Scope, nameSpace, true);

                ns.SetValue(varName, varValue);
                
            }            
        }

        
        /// <summary>
        /// Stack that reserve the SilentEvaluation during successive calls to the SilentEvaluate
        /// </summary>
        private Stack<bool> SilentStack = new Stack<bool>();

        /// <summary>
        /// Determine if we should echo the output on the screen or not.
        /// </summary>
        private bool SilentOutput
        {
            get
            {
                if (SilentStack.Count > 0) return true;
                else return false;
            }
        }


        /// <summary>
        /// Never put any thing on the output screen.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public object SilentEvaluate(string line)
        {
            
            SilentStack.Push(true);
            try
            {
                var r = Evaluate(line);
                return r;
            }
            finally
            {   
                SilentStack.Pop();
            }
        }



        static Regex UnitToUnitRegex = new Regex(UnitToUnitExpression);
        static Regex UnitRegex = new Regex(UnitExpression);
        static Regex VariableQuantityRegex = new Regex(VariableQuantityExpression);
        public object Evaluate(string expr)
        {

#if WINRT
#else
            if (string.IsNullOrEmpty(expr)) return null;
            Match m = null;

            #region Match <unit> to <unit>

            //match unit to unit
            m = UnitToUnitRegex.Match(expr);
            if (m.Success)
            {
                //evaluate unit

                try
                {
                    Unit u1 = Unit.Parse(m.Groups[1].Value);
                    Unit u2 = Unit.Parse(m.Groups[2].Value);
                    //PrintUnitInfo(u);
                    UnitPathStack up = u1.PathToUnit(u2);

                    Console.ForegroundColor = ForegroundColor;

                    Console.WriteLine();
                    double ConversionFactor = up.ConversionFactor;
                    if (u1.IsOverflowed)
                    {
                        var uof = u1.GetUnitOverflow();
                        Console.WriteLine("    Overflow in first unit {0}: {1}", u1.Symbol, uof);

                        ConversionFactor *= uof;
                    }

                    if (u2.IsOverflowed)
                    {
                        var uof = u2.GetUnitOverflow();
                        Console.WriteLine("    Overflow in second unit {0}: {1}", u2.Symbol, uof);
                        ConversionFactor /= uof;
                    }

                    string cf = "    Conversion Factor => " + ConversionFactor.ToString(CultureInfo.InvariantCulture);

                    foreach (UnitPathItem upi in up) Console.WriteLine("    -> {0}", upi);

                    string dashes = "    ".PadRight(cf.Length, '-');

                    Console.WriteLine(dashes);
                    Console.ForegroundColor = ValuesColor;
                    Console.WriteLine(cf);
                    Console.ForegroundColor = ForegroundColor;

                    return up.ConversionFactor;
                }
                catch (UnitNotFoundException e)
                {
                    throw new QsException("Unit Not Found", e);
                }
                catch (UnitsNotDimensionallyEqualException e)
                {
                    throw new QsException("Units not dimensionally equal", e);
                }
            }
            #endregion

            #region Match Unit <unit> like  <kn>, <m>, <kW> etc.
            //match one unit first
            m = UnitRegex.Match(expr);
            if (m.Success)
            {
                //evaluate unit

                try
                {
                    Unit u = Unit.Parse(m.Groups[1].Value);
                    PrintUnitInfo(u);
                    return u;
                }
                catch (UnitNotFoundException e)
                {
                    throw new QsException("Unit Not Found", e);
                }
                
            }
            #endregion

            string varName = string.Empty;

            #region Match variable Assignation with quantity "a=40[Acceleration]"
            //match variable assignation with quantity
            m = VariableQuantityRegex.Match(expr);
            
            if (m.Success)
            {
                //get the variable name
                varName = m.Groups[1].Value;
                double varVal = double.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);

                QsScalar qty = null;

                if (!string.IsNullOrEmpty(m.Groups[5].Value))
                {
                    try
                    {
                        //get the quantity

                        qty = new QsScalar { NumericalQuantity = AnyQuantity<double>.Parse(m.Groups[6].Value) };


                        //get the quantity

                        qty.NumericalQuantity.Unit = Unit.DiscoverUnit(qty.NumericalQuantity); 

                    }
                    catch (QuantityNotFoundException)
                    {
                        Console.Error.WriteLine("Quantity Not Found");
                        return null;
                    }

                }

                qty.NumericalQuantity.Value = varVal;

                SetVariable(varName, qty);
                
                PrintQuantity(qty);
                return qty;
            }
            #endregion

#endif
            //check if the line has '='
            return ExtraEvaluate(expr);

        }


        /// <summary>
        /// Obtains the location of equal sign, that is not inside string or inside function  parenthese
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int GetAssignOperatorIndex(string m)
        {
            int ix = 0;
            int sl = m.Length;
            int scs = 0;
            int bcs = 0;
            int rcs = 0;

            bool InText = false;
            while (ix < sl)
            {
                if (m[ix] == '(') scs++;
                if (m[ix] == ')') scs--;
                if (m[ix] == '[') bcs++;
                if (m[ix] == ']') bcs--;

                if (m[ix] == '{') rcs++;
                if (m[ix] == '}') rcs--;

                if (m[ix] == '"')
                {
                    if (ix > 0)
                    {
                        if (m[ix - 1] != '\\') // not escape character for qoutation mark
                            InText = !InText;
                    }
                    else
                        InText = !InText;
                }

                if (m[ix] == '=' && scs == 0 && rcs == 0 && bcs == 0 && InText == false) return ix;

                ix++;
            }
            return -1;
        }

        /// <summary>
        /// Test the variable name if it resemble a sequence variable like c[x] or something.
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="seq"></param>
        /// <param name="seqNamespace"></param>
        /// <param name="nsidx"></param>
        /// <returns></returns>
        public static bool IsSequence(string varName, out Token seq, out string seqNamespace, out int nsidx)
        {
            bool IsSequence = false;
            seq = null;
            nsidx = 0;
            seqNamespace = string.Empty;
            if (varName.Contains("["))
            {
                // May be sequence element.
                seq = Token.ParseText(varName);
                seq = seq.MergeTokens<MultipleSpaceToken>();
                seq = seq.RemoveSpaceTokens();
                seq = seq.MergeTokens<WordToken>();
                seq = seq.MergeTokens<ColonToken>();
                seq = seq.MergeTokensInGroups(new ParenthesisGroupToken(), new SquareBracketsGroupToken());
                seq = seq.MergeTokens<NamespaceToken>(); //discover namespace tokens

                if (seq[0].TokenClassType == typeof(NamespaceToken))
                {
                    nsidx = 1; //the function begin with namespace.
                    seqNamespace = seq[0][0].TokenValue;
                }

                if (seq.Count >= nsidx + 2)
                {
                    if (seq[nsidx + 1].TokenClassType == typeof(SquareBracketsGroupToken))
                    {
                        IsSequence = true;
                    }
                }
            }


            return IsSequence;
        }

        

        internal object ExtraEvaluate(string line)
        {
            // f(x,y,z) = x + y + (z/2.04 * 32<kg>)
            QsFunction func = QsFunction.ParseFunction(this, line);

            if (!System.Object.ReferenceEquals(func, null))
            {
                //store the expression for later use 
                StoreFunction(func);
                return func;
            }


            //try to get sequence.
            QsSequence seqo = QsSequence.ParseSequence(this, line);
            if (seqo != null)
            {
                //store the expression for later use 
                SetVariable(seqo.SequenceNamespace, seqo.SequenceSymbolicName, seqo);
                
                return seqo;
            }
            
            #region expression evaluation with assignation or no assignation

            string varName = string.Empty;

            int AssignOperatorIndex = GetAssignOperatorIndex(line);

            if (AssignOperatorIndex > 0)  //assign operator should always have something behind it.
            {
                if (   
                       line[AssignOperatorIndex - 1] == ':' // test for named argument :=  
                    || line[AssignOperatorIndex - 1] == '>'  // >=
                    || line[AssignOperatorIndex - 1] == '<'  // <=
                    || line[AssignOperatorIndex - 1] == '!'  // !=
                    || line[AssignOperatorIndex + 1] == '='  // ==
                    
                    ) 
                {
                    //ignore
                }
                else
                {
                    //split to variable name that will be assigned 
                    varName = line.Substring(0, AssignOperatorIndex).Trim();

                    if (char.IsNumber(varName[0]))
                    {
                        throw (new QsSyntaxErrorException("Variable must start with a letter"));
                    }

                    if (varName.Contains(" ") || varName.Contains("\t"))
                        throw (new QsSyntaxErrorException("Variable shouldn't contain spaces"));

                    line = line.Substring(AssignOperatorIndex + 1, line.Length - AssignOperatorIndex - 1);
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(varName))
                {
                    #region Processing the variable name region
                    Token seq;
                    int nsidx;
                    string seqNamespace;
                    bool IsSequence = QsEvaluator.IsSequence(varName, out seq, out seqNamespace, out nsidx);
                    if (IsSequence)
                    {
                        #region Sequence element operation
                        // Sequence element assignation.

                        if (seq[nsidx + 1].TokenClassType == typeof(SquareBracketsGroupToken))
                        {
                            if (seq[nsidx + 1].Contains(typeof(ColonToken)))
                            {
                                #region indexed sequence.
                                //a[n:1] = n!    the form that we expect.
                                string[] indexText = seq[nsidx + 1].TokenValue.Trim('[', ']').Split(':');
                                int n = int.Parse(indexText[1]);
                                string indexName = indexText[0];

                                if (seq.Count == nsidx + 3)
                                {
                                    #region Parameterized indexed Sequence

                                    string[] parameters = { }; //array with zero count :)
                                    if (seq[nsidx + 2].TokenClassType == typeof(ParenthesisGroupToken))
                                    {
                                        parameters = (from c in seq[nsidx + 2]
                                                      where c.TokenClassType == typeof(WordToken)
                                                      select c.TokenValue).ToArray();
                                    }
                                    else
                                    {
                                        throw new QsException("Expected Parenthesis for sequence element assignation");
                                    }

                                    string sqname = QsSequence.FormSequenceSymbolicName(seq[nsidx + 0].TokenValue, 1, parameters.Length);

                                    QsSequence sq = QsSequence.GetSequence(Scope, seqNamespace, sqname);

                                    //replace the index name used with the real index of the sequence.
                                    string evline = line.Replace(indexName, sq.SequenceIndexName);


                                    //replace the parameter names with parameters with the real parameter names of sequence
                                    for (int i = 0; i < parameters.Length; i++)
                                    {
                                        evline = evline.Replace(parameters[i], sq.Parameters[i].Name);
                                    }

                                    sq[n] = QsSequenceElement.Parse(evline, this, sq);
                                    #endregion
                                }
                                else
                                {
                                    #region Parameterless indexed Sequence
                                    string sqname = QsSequence.FormSequenceSymbolicName(seq[nsidx + 0].TokenValue, 1, 0);
                                    QsSequence sq = QsSequence.GetSequence(Scope, seqNamespace, sqname);

                                    //replace the index name used with the real index of the sequence.
                                    string evline = line.Replace(indexName, sq.SequenceIndexName);

                                    sq[n] = QsSequenceElement.Parse(evline, this, sq);
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {

                                #region Non indexed sequence

                                //match for a[1]

                                string SequenceParameter = seq[nsidx + 1].TokenValue.Trim('[', ']');
                                int n;
                                if (int.TryParse(SequenceParameter, out n))
                                {
                                    if (seq.Count == nsidx + 3)
                                    {
                                        #region Parametrized non indexed sequence

                                        string[] parameters = { }; //array with zero count :)
                                        if (seq[nsidx + 2].TokenClassType == typeof(ParenthesisGroupToken))
                                        {
                                            parameters = (from c in seq[nsidx + 2]
                                                          where c.TokenClassType == typeof(WordToken)
                                                          select c.TokenValue).ToArray();
                                        }
                                        else
                                        {
                                            throw new QsException("Expected Parenthesis for sequence element assignation");
                                        }

                                        string sqname = QsSequence.FormSequenceSymbolicName(seq[nsidx + 0].TokenValue, 1, parameters.Length);

                                        QsSequence sq = QsSequence.GetSequence(Scope, seqNamespace, sqname);

                                        string evline = line;

                                        //replace the parameter names with parameters with the real parameter names of sequence
                                        for (int i = 0; i < parameters.Length; i++)
                                        {
                                            evline = evline.Replace(parameters[i], sq.Parameters[i].Name);
                                        }

                                        sq[n] = QsSequenceElement.Parse(evline, this, sq);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Parameterless Sequence or an object
                                        
                                        // it doesn't really have to be a sequence name.
                                        // the judgment here is to get the object 
                                        // if it was a QsObject then we deal with its indexer.

                                        string oname = seq[nsidx + 0].TokenValue;

                                        var _obj = QsEvaluator.GetScopeValueOrNull(Scope, seqNamespace, oname) as QsObject;

                                        if (_obj == null)
                                        {
                                            string sqname = QsSequence.FormSequenceSymbolicName(seq[nsidx + 0].TokenValue, 1, 0);

                                            QsSequence sq = QsSequence.GetSequence(Scope, seqNamespace, sqname);

                                            sq[n] = QsSequenceElement.Parse(line, this, sq);
                                        }
                                        else
                                        {
                                            QsVar qv = new QsVar(this, line);

                                            _obj.SetIndexedItem(
                                                new QsParameter[] 
                                                { 
                                                    QsParameter.MakeParameter(n.ToQuantity().ToScalar(), SequenceParameter) 
                                                }
                                                , (QsValue)qv.Execute()
                                                );
                                        }
                                        
                                        #endregion
                                    }
                                }
                                else
                                {
                                    // not a number may be  then it is a text :) :)
                                    SequenceParameter = SequenceParameter.Trim('"');  // remove 
                                    // get the name of this variable
                                    
                                    string oname = seq[nsidx + 0].TokenValue;
                                    var oinstance = GetVariable(oname) as QsValue;
                                    QsVar qv = new QsVar(this, line);

                                    oinstance.SetIndexedItem(
                                        new QsParameter[] { QsParameter.MakeParameter(new QsText(SequenceParameter), SequenceParameter) }
                                        , (QsValue)qv.Execute()
                                        );

                                }

                                #endregion
                            }

                            return null;
                        }
                        else
                        {
                            // not sequence.
                            //do nothing
                            return null;
                        }
                        #endregion
                    }
                    else
                    {
                        
                        #region Normal Variable

                        //Normal Variable.
                        // get the after assign expression value
                        QsVar qv = new QsVar(this, line);

                        if (varName.Contains(":"))
                        {
                            #region namespace in variable
                            Token vnToken = Token.ParseText(varName);
                            vnToken = vnToken.MergeTokens<MultipleSpaceToken>();
                            vnToken = vnToken.RemoveSpaceTokens();
                            vnToken = vnToken.MergeTokens<WordToken>();
                            vnToken = vnToken.MergeTokens<NumberToken>();
                            vnToken = vnToken.MergeTokens<ColonToken>();
                            vnToken = vnToken.MergeTokens<NamespaceToken>();
                            vnToken = vnToken.MergeSequenceTokens<WordToken>(typeof(AtSignToken), typeof(WordToken));

                            if (vnToken.Contains(typeof(NamespaceToken)))
                            {
                                var qvResult = qv.Execute();
                                if (qvResult is QsFunction)
                                {
                                    var qf = (QsFunction)qvResult;
                                    qf.FunctionNamespace = vnToken[0][0].TokenValue;
                                    qf.FunctionName = vnToken[1].TokenValue;

                                    StoreFunction(qf);
                                    return qvResult;
                                }
                                else
                                {
                                    if (vnToken[vnToken.Count - 1].TokenValue.StartsWith("@") && qvResult.GetType() == typeof(QsScalar))
                                    {
                                        var fsc = (QsScalar)qvResult;
                                        if (fsc.ScalarType == ScalarTypes.FunctionQuantity)
                                        {
                                            // @f = @function which means we should assign f to qsfunction f
                                            
                                            var qf = (QsFunction)((QsScalar)qvResult).FunctionQuantity.Value.Clone();

                                            qf.FunctionNamespace = vnToken[0][0].TokenValue;
                                            qf.FunctionName = vnToken[1][1].TokenValue;

                                            StoreFunction(qf);
                                            return qf;
                                        }
                                        else if (fsc.ScalarType == ScalarTypes.SymbolicQuantity)
                                        {
                                            var fh = "_(";
                                            foreach (var p in fsc.SymbolicQuantity.Value.InvolvedSymbols) fh += p + ", ";
                                            fh = fh.TrimEnd(',', ' ') + ") = ";

                                            var qf = QsFunction.ParseFunction(this, fh + fsc.SymbolicQuantity.Value.ToString());

                                            qf.FunctionNamespace = vnToken.TokenValue.TrimEnd(vnToken[vnToken.Count - 1].TokenValue.ToCharArray()).TrimEnd(':');
                                            qf.FunctionName = vnToken[vnToken.Count - 1].TokenValue.TrimStart('@');

                                            StoreFunction(qf);
                                            return qf;
                                        }
                                        else
                                        {
                                            throw new QsException("Cannot store scalar " + fsc.ScalarType.ToString() + " into function variable");
                                        }
                                    }
                                    else if (vnToken[vnToken.Count - 2].TokenValue.StartsWith("&") && qvResult is QsValue)
                                    {
                                        // store the reference of the value to the same name here
                                        //  &b = a   it means that b and a will point to the same variable.
                                        var existValue = GetVariable(line.Trim());
                                        var ns = vnToken.TrimTokens(0,2).TokenValue.TrimEnd(':');
                                        var nm = vnToken[vnToken.Count - 1].TokenValue.TrimStart('&');
                                        var q = AddReference(ns, nm, line.Trim());
                                        PrintQuantity(q);
                                        return q;
                                    }
                                    else
                                    {
                                        
                                        SetVariable(
                                            vnToken.TokenValue.TrimEnd(vnToken[vnToken.Count - 1].TokenValue.ToCharArray()).TrimEnd(':')
                                            , vnToken[vnToken.Count - 1].TokenValue
                                            , qvResult
                                            );

                                       
                                        var q = GetScopeQsValue(this.Scope
                                            , vnToken.TokenValue.TrimEnd(vnToken[vnToken.Count - 1].TokenValue.ToCharArray()).TrimEnd(':')
                                            , vnToken[vnToken.Count - 1].TokenValue
                                            );
                                        PrintQuantity(q);
                                        return q;
                                    }
                                }
                            }
                            else
                            {
                                throw new QsInvalidInputException();
                            }
                            #endregion
                        }
                        else
                        {
                            #region bare variable name
                            //assign the variable
                            var qvResult = qv.Execute();
                            if (qvResult is QsFunction)
                            {
                                var qf = (QsFunction)qvResult;
                                qf.FunctionName = varName;
                                StoreFunction(qf);
                                return qvResult;
                            }
                            else
                            {
                                if (varName.StartsWith("@") && qvResult.GetType() == typeof(QsScalar))
                                {
                                    #region Variable starts with @ and the result is scalar function or symbolic quantity
                                    var fsc = (QsScalar)qvResult;
                                    varName = varName.Substring(1); // remove @
                                    if (fsc.ScalarType == ScalarTypes.FunctionQuantity)
                                    {
                                        // @f = @function which means we should assign f to qsfunction f

                                        var qf = (QsFunction)((QsScalar)qvResult).FunctionQuantity.Value.Clone();
                                        qf.FunctionName = varName;
                                        StoreFunction(qf);
                                        return qf;
                                    }
                                    else if (fsc.ScalarType == ScalarTypes.SymbolicQuantity)
                                    {
                                        var fh = "_(";
                                        foreach (var p in fsc.SymbolicQuantity.Value.InvolvedSymbols) fh += p + ", ";
                                        fh = fh.TrimEnd(',', ' ') + ") = ";

                                        var qf = QsFunction.ParseFunction(this, fh + fsc.SymbolicQuantity.Value.ToString());
                                        qf.FunctionName = varName;

                                        StoreFunction(qf);
                                        return qf;
                                    }
                                    else
                                    {
                                        
                                        throw new QsException("Cannot store scalar " + fsc.ScalarType.ToString() + " into function variable");

                                    }
                                    #endregion
                                }
                                else if(varName.StartsWith("@") && (qvResult.GetType()==typeof(QsVector) || qvResult.GetType()==typeof(QsMatrix)||qvResult.GetType()==typeof(QsTensor)))
                                {
                                    #region Variable starts with @ and result is complex qs type vector, matrix, or tensor to be converted into the corresponding field i.e. vector field or matrix field
                                    varName = varName.Substring(1); // remove @

                                    // vector or matrix or tensor
                                    // the parsing to function should know the symbolic variables in the expression
                                    //  and get the parameters required arranged by alphabet ofcourse

                                    QsScalar[] AllComponents = null;
                                    Type qvType = qvResult.GetType();

                                    if(qvType == typeof(QsVector))
                                        AllComponents = ((QsVector)qvResult).ToArray();
                                    if(qvType == typeof(QsMatrix))
                                        AllComponents = ((QsMatrix)qvResult).ToArray();

                                     if(qvType == typeof(QsTensor))
                                        throw new QsException("Tensor parsing to function is not supported yet");

  
                                    List<string> vparameters = new List<string>();
                                    foreach(var c in AllComponents)
                                        if(c.ScalarType ==  ScalarTypes.SymbolicQuantity)
                                        {
                                            foreach (var sym in c.SymbolicQuantity.Value.InvolvedSymbols)
                                            {
                                                if (!vparameters.Contains(sym)) vparameters.Add(sym);
                                            }
                                        }


                                    var fh = "_(";
                                        foreach (var p in vparameters) fh += p + ", ";
                                        fh = fh.TrimEnd(',', ' ') + ") = ";
                                    if(qvType == typeof(QsVector))
                                    {
                                        StringBuilder fhb = new StringBuilder();
                                        fhb.Append('{');
                                        foreach(var c in AllComponents)
                                        {
                                            fhb.Append(c.ToParsableValuedString());
                                            fhb.Append(' ');
                                        }
                                        fhb.Append('}');

                                        var qf = QsFunction.ParseFunction(this, fh + fhb.ToString());
                                        qf.FunctionName = varName;

                                        StoreFunction(qf);
                                        return qf;
                                    }
                                    else if (qvType == typeof(QsMatrix))
                                    {
                                        QsMatrix m = (QsMatrix)qvResult;
                                        StringBuilder fhb = new StringBuilder();
                                        fhb.Append('[');
                                        foreach (var vv in m)
                                        {
                                            fhb.Append(' ');
                                            foreach (var c in vv)
                                            {
                                                fhb.Append(c.ToParsableValuedString());
                                                fhb.Append(' ');
                                            }
                                            fhb.Append("; ");
                                        }

                                        var all = fh + fhb.ToString().Trim(';', ' ') + "]";


                                        var qf = QsFunction.ParseFunction(this, all);
                                        qf.FunctionName = varName;

                                        StoreFunction(qf);
                                        return qf;

                                    }
                                    else
                                    {
                                        throw new QsException("QsType " + qvType.Name + " is not supported to be converted into function");
                                    }
                                    #endregion
                                }
                                else if (varName.StartsWith("&") && qvResult is QsValue)
                                {
                                    var existValue = GetVariable(line.Trim());
                                    var q = AddReference(string.Empty, varName.Substring(1), line.Trim());
                                    PrintQuantity(q);
                                    return q;
                                }
                                else
                                {
                                    var varNameTokens = QsVar.TokenzieExpression(varName);
                                    int v_index = 0;
                                    QsValue parent_object = null;
                                    PropertyInfo ParentPropertyInfo = null;
                                    int? ParentPropertyInfoIndexer = null;
                                    Dictionary<PropertyInfo, bool> PropertyInfoIndexed = new Dictionary<PropertyInfo, bool>();

                                    while (v_index < varNameTokens.Count && varNameTokens.Count > 1)
                                    {
                                        if (varNameTokens[v_index].TokenClassType == typeof(WordToken))
                                        {
                                            // set parent_object value
                                            if (v_index == 0)
                                            {
                                                parent_object = GetScopeQsValue(this.Scope, "", varNameTokens[v_index].TokenValue);
                                            }
                                            else
                                            {
                                                // get property from parent_object
                                                if (varNameTokens[v_index - 1].TokenClassType == typeof(PointerOperatorToken))
                                                {
                                                    // we are accessing property here
                                                    QsObject parent = (QsObject)parent_object;
                                                    if (ParentPropertyInfo == null)
                                                    {
                                                        ParentPropertyInfo = parent.GetPropertyInfo(varNameTokens[v_index].TokenValue);
                                                    }
                                                    else
                                                    {
                                                        if (ParentPropertyInfo.PropertyType.IsArray == true || ParentPropertyInfo.GetIndexParameters().Length > 0)
                                                        {
                                                            parent_object = parent.GetIndexedProperty(ParentPropertyInfo.Name, ParentPropertyInfoIndexer.Value);
                                                            parent = (QsObject)parent_object;
                                                            ParentPropertyInfo = parent.GetPropertyInfo(varNameTokens[v_index][0].TokenValue);
                                                        }
                                                        else
                                                        {
                                                            // we had previous unresolved property info
                                                            // so we get it as an object 
                                                            //   set this object as the parent
                                                            //  then get the sub property metadata again.
                                                            parent_object = parent.GetProperty(ParentPropertyInfo.Name);
                                                            parent = (QsObject)parent_object;
                                                            ParentPropertyInfo = parent.GetPropertyInfo(varNameTokens[v_index].TokenValue);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // previous operator not known
                                                    throw new NotImplementedException("Operator " + varNameTokens[v_index - 1].TokenValue + " not known");
                                                }
                                            }
                                        }
                                        else if (varNameTokens[v_index].TokenClassType == typeof(SequenceCallToken))
                                        {
                                            if (v_index == 0)
                                            {
                                                // variable name beginning with indexer :  G[0] for example is handled before
                                                throw new NotImplementedException("Review the code above ,.., this case of sequence calling at the beginning of expression is handled there");
                                            }
                                            else
                                            {
                                                if (varNameTokens[v_index - 1].TokenClassType == typeof(PointerOperatorToken))
                                                {
                                                    // then we are referring to object with a property in its properties that is an array.
                                                    // we need to get this property
                                                    QsObject parent = (QsObject)parent_object;
                                                    if (ParentPropertyInfo == null)
                                                    {
                                                        ParentPropertyInfo = parent.GetPropertyInfo(varNameTokens[v_index][0].TokenValue);
                                                        PropertyInfoIndexed[ParentPropertyInfo] = true;
                                                    }
                                                    else
                                                    {
                                                        // if the previous parent property has an indexers then we need to get it as an index
                                                        // if not then it was a normal variable
                                                        if (ParentPropertyInfo.PropertyType.IsArray == true || ParentPropertyInfo.GetIndexParameters().Length > 0)
                                                        {
                                                            parent_object = parent.GetIndexedProperty(ParentPropertyInfo.Name, ParentPropertyInfoIndexer.Value);
                                                            parent = (QsObject)parent_object;
                                                            ParentPropertyInfo = parent.GetPropertyInfo(varNameTokens[v_index][0].TokenValue);
                                                        }
                                                        else
                                                        {
                                                            // get the previous property and make it parent
                                                            parent_object = parent.GetProperty(ParentPropertyInfo.Name);
                                                            parent = (QsObject)parent_object;
                                                            ParentPropertyInfo = parent.GetPropertyInfo(varNameTokens[v_index][0].TokenValue);
                                                        }

                                                    }

                                                    // and get the indexing number
                                                    ParentPropertyInfoIndexer = int.Parse(varNameTokens[v_index][1][1].TokenValue);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //nothing
                                        }
                                        v_index++;
                                    }

                                    if (parent_object!=null && ParentPropertyInfo!=null)
                                    {
                                        if (ParentPropertyInfo.PropertyType.IsArray == true || ParentPropertyInfo.GetIndexParameters().Length > 0)
                                        {
                                            // set the value to the indexed item.
                                            QsObject parent = (QsObject)parent_object;
                                            ParentPropertyInfo.SetValue(parent.ThisObject, qvResult, new object[] { ParentPropertyInfoIndexer.Value });
                                            var q = parent.GetProperty(ParentPropertyInfo.Name, ParentPropertyInfoIndexer.Value);
                                            PrintQuantity(q);
                                            return q;
                                        }
                                        else if (PropertyInfoIndexed.ContainsKey(ParentPropertyInfo) && PropertyInfoIndexed[ParentPropertyInfo]) // indexer in object
                                        {
                                            // we are dealing with indexer
                                            // indexer should be called by the index
                                            
                                            QsObject parent = (QsObject)parent_object;

                                            var InsidePropertyAsObject = ParentPropertyInfo.GetValue(parent.ThisObject, null);
                                            Type InsidePropertyObjectType = InsidePropertyAsObject.GetType();
                                            PropertyInfo IndexerProperty = InsidePropertyObjectType.GetProperty("Item");

                                            var nativeValue = Root.QsToNativeConvert(IndexerProperty.PropertyType, qvResult);
                                            IndexerProperty.SetValue(InsidePropertyAsObject
                                                , nativeValue
                                                , new object[] { ParentPropertyInfoIndexer.Value }
                                                );

                                            var q = IndexerProperty.GetValue(InsidePropertyAsObject, new object[] { ParentPropertyInfoIndexer.Value });
                                            PrintQuantity(q);
                                            return q;
                                        }
                                        else
                                        {
                                            QsObject parent = (QsObject)parent_object;
                                            parent.SetProperty(ParentPropertyInfo.Name, qvResult);
                                            var q = parent.GetProperty(ParentPropertyInfo.Name);
                                            PrintQuantity(q);
                                            return q;
                                        }
                                    }
                                    else
                                    {
                                        // bare variable name without enclosed properties
                                        SetVariable(varName, qvResult);
                                        var q = GetScopeQsValue(this.Scope, "", varName);
                                        PrintQuantity(q);
                                        return q;
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion

                    }

                    #endregion
                }
                else
                {
                    // there is no assignation here  
                    // the code goes here when we don't find '=' equal sign.

                    QsVar qv = new QsVar(this, line);
                    //only print the result.
                    var q = qv.Execute();
                    if (q != null) PrintQuantity(q);
                    return q;
                }
            }
            #region Catch region
            catch (NullReferenceException e)
            {
                throw new QsException(e.Message, e);
            }
            catch (QuantitiesNotDimensionallyEqualException e)
            {
                throw new QsException("Quantities Not Dimensionally Equal", e);
            }
            catch (UnitNotFoundException e)
            {
                throw new QsException("Unit Not Found", e);
            }
            catch (QuantityException qe)
            {
                throw new QsException(qe.Message, qe);
            }
            catch (OverflowException e)
            {
                throw new QsException("Overflow", e);
            }
            catch (QsException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException)
                {
                    throw new QsException("Unhandled", e.InnerException);
                }
                else
                {
                    throw new QsException("Unhandled", e);
                }
            }
            #endregion

            #endregion
        }


        #region print information helpers

        public void PrintUnitInfo(Unit unit)
        {

#if WINRT
#else
            Console.ForegroundColor = HelpColor;

            Console.WriteLine("    Unit:        {0}", unit.ToString());
            Console.WriteLine("    Quantity:    {0}", unit.QuantityType.Name);
            Console.WriteLine("    Dimension:   {0}", unit.UnitDimension);
            Console.WriteLine("    Unit System: {0}", unit.UnitSystem);

            if (unit.IsOverflowed)
            {
                Console.WriteLine("    Unit overflow: {0}", unit.GetUnitOverflow());
            }

            Console.ForegroundColor = ForegroundColor;
#endif
        }

        public void PrintQuantity(object qty)
        {
#if WINRT
#else
            if (!SilentOutput)
            {
                Console.ForegroundColor = ValuesColor;

                Console.WriteLine("    {0}", qty.ToString());

                Console.ForegroundColor = ForegroundColor;
            }
#endif
        }

        #endregion


        #region Scope Helper methods.

        /// <summary>
        /// Get the value or null
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="nameSpace"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetScopeValueOrNull(QsScope scope, string nameSpace, string name)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                object q;
                scope.TryGetValue(name, out q);
                return q;
            }
            else
            {
                object o;

                scope.TryGetValue(nameSpace, out o);

                QsNamespace ns = o as QsNamespace;

                if (ns != null)
                {
                    return ns.GetValueOrNull(name);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Used by reflection.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="nameSpace"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static QsValue GetScopeQsValue(QsScope scope, string qsNamespace, string name)
        {

            if (string.IsNullOrEmpty(qsNamespace))
            {
                object q;

                scope.TryGetValue(name, out q);

                if (q != null)
                {
                    return (QsValue)q;
                }
                else
                {
                    // look for enum in the QsRoot namespace
                    var t = Type.GetType("QsRoot." + name);
                    if (t != null)
                    {
                        if (t.IsEnum)
                        {
                            // convert enum into Flowing Tuple and return it.
                            
                            return QsRoot.Root.NativeToQsConvert(t);
                        }
                    }

                    throw new QsVariableNotFoundException("Variable '" + name + "' Not Found.")
                        {
                            Namespace = qsNamespace,
                            Variable = name
                        };
                }
            }
            else
            {
                //get the variable from the name space

                var module = QsNamespace.GetNamespace(scope, qsNamespace);

                return (QsValue)module.GetValue(name);
                    
            }

        }

        public QsValue DeleteQsValue(string qsNamespace, string name)
        {

            if (string.IsNullOrEmpty(qsNamespace))
            {
                if (!this.Scope.DeleteValue(name))
                    throw new QsException("Can't delete the variable");
                else
                {
                    GC.Collect();
                }
            }
            return null;
        }

        #endregion


        #region  singleton pattern
        private QsEvaluator()
        {
        }

        private static Assembly _CallingAssembly; // this will hold the calling assembly that called the current evaluator
        public static Assembly CallingAssembly
        {
            get
            {
                return _CallingAssembly;
            }
        }

        private static QsEvaluator _CurrentEvaluator;
        public static QsEvaluator CurrentEvaluator
        {
            get
            {
                if (_CurrentEvaluator == null)
                {
                    _CurrentEvaluator = new QsEvaluator();
                    _CallingAssembly = Assembly.GetCallingAssembly();
                }
                return _CurrentEvaluator;
            }
        }
         
        #endregion


        #region Function Storage 


        /// <summary>
        /// Store the function in the scope by choosing the suitable symbolic name for the function.
        /// </summary>
        /// <param name="function"></param>
        public void StoreFunction(QsFunction qsFunction)
        {
            
            // 1- find the default function of this name.
            // 2- if default function exist declare non default function
            // 3- if default function does'nt exist declare default function.
            // Default function: is function declared without specifying its parameters in its name  f#2 f#4  are default functions.

            QsFunction DefaultFunction = QsFunction.GetDefaultFunction(this.Scope, qsFunction.FunctionNamespace, 
                qsFunction.FunctionName, qsFunction.Parameters.Length);

            if (DefaultFunction == null)
            {
                // then store the function
                SetVariable(qsFunction.FunctionNamespace, 
                    QsFunction.FormFunctionSymbolicName(qsFunction.FunctionName, qsFunction.Parameters.Length),
                    qsFunction);
            }
            else
            {
                // 1- find if function with the same parameters and name exist 
                // 2- overwrite this function otherwise create new one.

                var OverLoadedFunction = QsFunction.GetExactFunctionWithParameters(this.Scope,
                    qsFunction.FunctionNamespace,
                    qsFunction.FunctionName,
                    qsFunction.ParametersNames);

                if (OverLoadedFunction == null)
                {
                    // store new overloaded function with parameters.
                    SetVariable(qsFunction.FunctionNamespace,
                        QsFunction.FormFunctionSymbolicName(qsFunction.FunctionName, qsFunction.ParametersNames),
                        qsFunction);
                }
                else
                {
                    //overwrite  the old function.

                    if (OverLoadedFunction.FunctionDeclaration.Equals(DefaultFunction.FunctionDeclaration))
                    {
                        //overwrite default function.

                        SetVariable(DefaultFunction.FunctionNamespace,
                            QsFunction.FormFunctionSymbolicName(qsFunction.FunctionName, DefaultFunction.ParametersNames.Length),
                            qsFunction);
                    }
                    else
                    {
                        // overwrite overloaded function

                        SetVariable(DefaultFunction.FunctionNamespace,
                            QsFunction.FormFunctionSymbolicName(qsFunction.FunctionName, OverLoadedFunction.ParametersNames),
                            qsFunction);
                    }

                }
            }

        }

        #endregion

    }

}
