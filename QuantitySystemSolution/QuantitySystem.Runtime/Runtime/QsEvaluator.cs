using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using ParticleLexer;
using ParticleLexer.StandardTokens;
using Qs.Types;
using System.Reflection;
using System.IO;
using ParticleConsole.QsTokens;


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
        public const string UnitExpression = @"^\s*<([\w\.\^\/!]+)>\s*$";

        /// <summary>
        /// Match unit to unit 
        /// </summary>
        public const string UnitToUnitExpression = @"^\s*<(.+)>\s*[tT][oO]\s*<(.+)>\s*$";

        public const string DoubleNumber = @"[-+]?\d+(\.\d+)*([eE][-+]?\d+)*?";

        public const string VariableQuantityExpression = @"^(\w+)\s*=\s*(" + DoubleNumber + @")\s*(\[(.+)\])";



        public IEnumerable<string> VariablesKeys
        {
            get
            {
                if (Scope != null)
                {
                    var varo = from item in ((ScopeStorage)Scope.Storage).GetItems()
                               select item.Key;
                    return varo;
                }
                else
                {
                    throw new Exception("No Scope exist");
                }
            }
        }

        public Scope Scope { get; set; }


        public object GetVariable(string varName)
        {
            if (Scope != null)
            {
                object q;
                ((ScopeStorage)Scope.Storage).TryGetValue(varName, true, out q);
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
                ((ScopeStorage)Scope.Storage).TryGetValue(varName, true, out q);
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
                ((ScopeStorage)Scope.Storage).SetValue(varName, true, varValue);
            }
            else
            {
                throw new Exception("No Scope exist");
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


        public bool SilentOutput = false;

        /// <summary>
        /// Never put any thing on the output screen.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public object SilentEvaluate(string line)
        {
            SilentOutput = true;
            var r = Evaluate(line);
            SilentOutput = false;
            return r;
        }

        public object Evaluate(string expr)
        {

            if (string.IsNullOrEmpty(expr)) return null;
            Match m = null;

            #region Match <unit> to <unit>

            //match unit to unit
            m = Regex.Match(expr, UnitToUnitExpression);
            if (m.Success)
            {
                //evaluate unit

                try
                {
                    Unit u1 = Unit.Parse(m.Groups[1].Value);
                    Unit u2 = Unit.Parse(m.Groups[2].Value);
                    //PrintUnitInfo(u);
                    UnitPath up = u1.PathToUnit(u2);

                    Console.ForegroundColor = ConsoleColor.Gray;

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

                    string cf = "    Conversion Factor => " + ConversionFactor.ToString();

                    foreach (UnitPathItem upi in up) Console.WriteLine("    -> {0}", upi);

                    string dashes = "    ".PadRight(cf.Length, '-');

                    Console.WriteLine(dashes);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(cf);
                    Console.ForegroundColor = ConsoleColor.White;

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
            m = Regex.Match(expr, UnitExpression);
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
            m = Regex.Match(expr, VariableQuantityExpression);
            if (m.Success)
            {
                //get the variable name
                varName = m.Groups[1].Value;
                double varVal = double.Parse(m.Groups[2].Value);

                QsScalar qty = null;

                if (!string.IsNullOrEmpty(m.Groups[5].Value))
                {
                    try
                    {
                        //get the quantity

                        qty = new QsScalar { Quantity = AnyQuantity<double>.Parse(m.Groups[6].Value) };


                        //get the quantity

                        qty.Quantity.Unit = Unit.DiscoverUnit(qty.Quantity); 

                    }
                    catch (QuantityNotFoundException)
                    {
                        Console.Error.WriteLine("Quantity Not Found");
                        return null;
                    }

                }

                qty.Quantity.Value = varVal;

                SetVariable(varName, qty);
                
                PrintQuantity(qty);
                return qty;
            }
            #endregion


            #region expression

            //check if the line has '='
            return ExtraEvaluate(expr);


            #endregion

        }


        internal object ExtraEvaluate(string line)
        {
            // f(x,y,z) = x + y + (z/2.04 * 32<kg>)
            QsFunction func = QsFunction.ParseFunction(this, line);

            if (func != null)
            {
               
                //store the expression for later use 
                //SetVariable(func.FunctionNamespace, func.FunctionSymbolicName, func);

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

            // check of existance of = inside bracket
            Token tt = Token.ParseText(line);
            tt = tt.MergeTokens(new WordToken());
            tt = tt.MergeTokens(new MultipleSpaceToken());
            tt = tt.MergeTokensInGroups(new ParenthesisGroupToken(), new SquareBracketsGroupToken());
            tt = tt.DiscoverCalls();
            


            int AssignOperatorIndex = -1;
            foreach (var t in tt)
            {
                if (t.TokenValue == "=")
                {
                    AssignOperatorIndex = t.IndexInText;
                    break;
                }
            }

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
                    // May be variable or sequence element.
                    Token seq = Token.ParseText(varName);
                    seq = seq.MergeTokens(new MultipleSpaceToken());
                    seq = seq.RemoveSpaceTokens();
                    seq = seq.MergeTokens(new WordToken());

                    seq = seq.MergeTokens(new ColonToken());
                    seq = seq.MergeTokensInGroups(new ParenthesisGroupToken(), new SquareBracketsGroupToken());

                    seq = seq.MergeTokens(new NameSpaceToken()); //discover namespace tokens

                    string seqNamespace = string.Empty;
                    int nsidx = 0;
                    if (seq[0].TokenClassType == typeof(NameSpaceToken))
                    {
                        nsidx = 1; //the function begin with namespace.
                        seqNamespace = seq[0][0].TokenValue;
                    }


                    bool IsSequence = false;
                    if (seq.Count >= nsidx + 2)
                    {
                        if (seq[nsidx + 1].TokenClassType == typeof(SquareBracketsGroupToken))
                        {
                            IsSequence = true;
                        }
                    }

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
                                int n = int.Parse(seq[nsidx + 1].TokenValue.Trim('[', ']'));

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

                                    #region Parameterless Sequence
                                    string sqname = QsSequence.FormSequenceSymbolicName(seq[nsidx + 0].TokenValue, 1, 0);

                                    QsSequence sq = QsSequence.GetSequence(Scope, seqNamespace, sqname);

                                    sq[n] = QsSequenceElement.Parse(line, this, sq);
                                    #endregion

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


                        Token vnToken = Token.ParseText(varName);
                        vnToken = vnToken.MergeTokens(new MultipleSpaceToken());
                        vnToken = vnToken.RemoveSpaceTokens();
                        vnToken = vnToken.MergeTokens(new WordToken());
                        vnToken = vnToken.MergeTokens(new ColonToken());
                        vnToken = vnToken.MergeTokens(new NameSpaceToken());

                        if (vnToken.Contains(typeof(NameSpaceToken)))
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
                                SetVariable(vnToken[0][0].TokenValue, vnToken[1].TokenValue, qvResult);

                                var q = GetScopeQsValue(this.Scope, vnToken[0][0].TokenValue, vnToken[1].TokenValue);
                                PrintQuantity(q);
                                return q;
                            }
                        }
                        else
                        {
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
                                SetVariable(varName, qvResult);

                                var q = GetScopeQsValue(this.Scope, "", varName);
                                PrintQuantity(q);

                                return q;
                            }
                        }
                        #endregion
                    }
                        
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
                throw new QsException("Unhandled", e);
            }
            #endregion

            #endregion
        }


        #region print information helpers

        public void PrintUnitInfo(Unit unit)
        {

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("    Unit:        {0}", unit.ToString());
            Console.WriteLine("    Quantity:    {0}", unit.QuantityType.Name);
            Console.WriteLine("    Dimension:   {0}", unit.UnitDimension);
            Console.WriteLine("    Unit System: {0}", unit.UnitSystem);

            if (unit.IsOverflowed)
            {
                Console.WriteLine("    Unit overflow: {0}", unit.GetUnitOverflow());
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintQuantity(object qty)
        {
            if (!SilentOutput)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("    {0}", qty.ToString());

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        #endregion






        #region Scope Helper methods.

        public static object GetScopeVariable(Scope scope, string nameSpace, string name)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                object q;
                ((ScopeStorage)scope.Storage).TryGetValue(name, true, out q);
                return q;
            }
            else
            {
                object o;

                ((ScopeStorage)scope.Storage).TryGetValue(nameSpace, true, out o);

                QsNamespace ns = o as QsNamespace;

                if (ns != null)
                {
                    return ns.GetValue(name);
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
        public static QsValue GetScopeQsValue(Scope scope, string qsNamespace, string name)
        {

            if (string.IsNullOrEmpty(qsNamespace))
            {
                object q;

                ((ScopeStorage)scope.Storage).TryGetValue(name, true, out q);

                if (q != null)
                {
                    return (QsValue)q;
                }
                else
                {
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
        #endregion



        #region  singleton pattern
        private QsEvaluator()
        {
        }

        private static QsEvaluator _CurrentEvaluator;
        public static QsEvaluator CurrentEvaluator
        {
            get
            {
                if (_CurrentEvaluator == null)
                    _CurrentEvaluator = new QsEvaluator();
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
