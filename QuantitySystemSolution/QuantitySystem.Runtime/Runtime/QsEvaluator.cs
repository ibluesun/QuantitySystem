﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using ParticleLexer;
using ParticleLexer.TokenTypes;
using Qs.RuntimeTypes;
using System.Reflection;
using System.IO;


namespace Qs.Runtime
{

    /// <summary>
    /// used to evaluate the quantity system expressions.
    /// </summary>
    public class QsEvaluator
    {

        public const string UnitExpression = @"^\s*<(.+?)>\s*$";

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

                bool nbi = false;

                {
                    Type ns = GetQsNameSpace(nameSpace);
                    if (ns != null)
                    {
                        var prop = ns.GetProperty(varName, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                        if (prop != null)
                        {
                            prop.SetValue(null, varValue, null);
                            nbi = true;
                        }
                    }
                }

                if (nbi == false)
                {

                    //get the namespace from the scope
                    object o;

                    ((ScopeStorage)Scope.Storage).TryGetValue(nameSpace, true, out o);

                    QsNameSpace ns = o as QsNameSpace;

                    if (ns == null)
                    {
                        //new name space so create it. and add it to the current scope
                        ns = new QsNameSpace(nameSpace);
                        ((ScopeStorage)Scope.Storage).SetValue(nameSpace, true, ns);
                    }

                    ns.SetName(varName, varValue);
                }
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
            //try to get sequence.
            QsSequence seqo = QsSequence.ParseSequence(this, line);

            //test for lambda expression 
            // f(x,y,z) = x + y + (z/2.04 * 32<kg>)
            QsFunction func = QsFunction.ParseFunction(this, line);

            if (seqo != null)
            {
                //store the expression for later use 
                ((ScopeStorage)Scope.Storage).SetValue(seqo.SequenceName, true, seqo);
                return seqo;
            }
            else if (func != null)
            {
               
                //store the expression for later use 
              
                SetVariable(func.FunctionNamespace, func.FunctionName, func);
           
                return func;
            }
            else
            {
                string varName = string.Empty;

                int AssignOperatorIndex = line.IndexOf('=');

                if (AssignOperatorIndex > 0)  //assign operator should always have something behind it.
                {
                    if (line[AssignOperatorIndex - 1] == ':') // test for named argument :=  
                    {
                        //ignore
                    }
                    else
                    {
                        //split to variable name that will be assigned 
                        varName = line.Substring(0, AssignOperatorIndex).Trim();

                        if (char.IsNumber(varName[0]))
                        {
                            throw (new QsInvalidInputException("Variable must start with a letter"));
                        }

                        line = line.Substring(AssignOperatorIndex + 1, line.Length - AssignOperatorIndex - 1);
                    }
                }


                if (QsVar.IsMatch(line))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(varName))
                        {
                            // May be variable or sequence element.
                            Token seq = Token.ParseText(varName);
                            seq = seq.MergeTokens(new SpaceToken());
                            seq = seq.RemoveSpaceTokens();
                            seq = seq.MergeTokens(new WordToken());
                            
                            seq = seq.MergeTokens(new ColonToken());
                            seq = seq.GroupBrackets();

                            seq = seq.MergeTokens(new NameSpaceToken()); //discover namespace tokens

                            bool IsSequence = false;
                            if (seq.Count >= 2)
                                if (seq[1].TokenType == typeof(SquareBracketGroupToken)) 
                                    IsSequence = true;

                            if (IsSequence)
                            {
                                // Sequence element assignation.

                                if (seq[1].TokenType == typeof(SquareBracketGroupToken))
                                {
                                    if (seq[1].Contains(typeof(ColonToken)))
                                    {
                                        //indexed sequence.
                                        //a[n:1] = n!    the form that we expect.
                                        string[] indexText = seq[1].TokenValue.Trim('[', ']').Split(':');
                                        int n = int.Parse(indexText[1]);
                                        string indexName = indexText[0];

                                        
                                        if (seq.Count == 3)
                                        {
                                            //Parameterized indexed Sequence
                                            
                                            string[] parameters = { }; //array with zero count :)
                                            if (seq[2].TokenType == typeof(ParenthesisGroupToken))
                                            {
                                                parameters = (from c in seq[2]
                                                              where c.TokenType == typeof(WordToken)
                                                              select c.TokenValue).ToArray();
                                            }
                                            else
                                            {
                                                throw new QsException("Expected Parenthesis for sequence element assignation");
                                            }

                                            string sqname = QsSequence.FormSequenceScopeName(seq[0].TokenValue, 1, parameters.Length);

                                            QsSequence sq = QsSequence.GetSequence(Scope, sqname);

                                            //replace the index name used with the real index of the sequence.
                                            string evline = line.Replace(indexName, sq.SequenceIndexName);


                                            //replace the parameter names with parameters with the real parameter names of sequence
                                            for (int i = 0; i < parameters.Length; i++)
                                            {
                                                evline = evline.Replace(parameters[i], sq.Parameters[i].Name);
                                            }

                                            sq[n] = QsSequenceElement.Parse(evline, this, sq);
                                        }
                                        else
                                        {
                                            // Parameterless indexed Sequence
                                            string sqname = QsSequence.FormSequenceScopeName(seq[0].TokenValue, 1, 0);
                                            QsSequence sq = QsSequence.GetSequence(Scope, sqname);

                                            //replace the index name used with the real index of the sequence.
                                            string evline = line.Replace(indexName, sq.SequenceIndexName);

                                            sq[n] = QsSequenceElement.Parse(evline, this, sq);
                                        }
                                    }
                                    else
                                    {
                                     
                                        //non indexed sequence
                                        //match for a[1]
                                        int n = int.Parse(seq[1].TokenValue.Trim('[', ']'));

                                        if (seq.Count == 3)
                                        {
                                            //Parametrized non indexed sequence

                                            string[] parameters = { }; //array with zero count :)
                                            if (seq[2].TokenType == typeof(ParenthesisGroupToken))
                                            {
                                                parameters = (from c in seq[2]
                                                              where c.TokenType == typeof(WordToken)
                                                              select c.TokenValue).ToArray();
                                            }
                                            else
                                            {
                                                throw new QsException("Expected Parenthesis for sequence element assignation");
                                            }

                                            string sqname = QsSequence.FormSequenceScopeName(seq[0].TokenValue, 1, parameters.Length);

                                            QsSequence sq = QsSequence.GetSequence(Scope, sqname);

                                            string evline = line;

                                            //replace the parameter names with parameters with the real parameter names of sequence
                                            for (int i = 0; i < parameters.Length; i++)
                                            {
                                                evline = evline.Replace(parameters[i], sq.Parameters[i].Name);
                                            }

                                            sq[n] = QsSequenceElement.Parse(evline, this, sq);

                                        }
                                        else
                                        {

                                            // Parameterless Sequence
                                            string sqname = QsSequence.FormSequenceScopeName(seq[0].TokenValue, 1, 0);

                                            QsSequence sq = QsSequence.GetSequence(Scope, sqname);

                                            sq[n] = QsSequenceElement.Parse(line, this, sq);

                                        }
                                    }

                                    return null;
                                }
                                else
                                {
                                    // not sequence.
                                    //do nothing
                                    return null;
                                }
                            }
                            else
                            {
                                //Normal Variable.
                                // get the after assign expression value
                                QsVar qv = new QsVar(this, line);


                                Token vnToken = Token.ParseText(varName);
                                vnToken = vnToken.MergeTokens(new SpaceToken());
                                vnToken = vnToken.RemoveSpaceTokens();
                                vnToken = vnToken.MergeTokens(new WordToken());
                                vnToken = vnToken.MergeTokens(new ColonToken());
                                vnToken = vnToken.MergeTokens(new NameSpaceToken());

                                if (vnToken.Contains(typeof(NameSpaceToken)))
                                {
                                    SetVariable(vnToken[0][0].TokenValue, vnToken[1].TokenValue, qv.Execute());

                                    var q = GetScopeQsValue(this.Scope, vnToken[0][0].TokenValue, vnToken[1].TokenValue);
                                    PrintQuantity(q);
                                    return q;
                                }
                                else
                                {
                                    //assign the variable
                                    SetVariable(varName, qv.Execute());

                                    var q = GetScopeQsValue(this.Scope, "", varName);
                                    PrintQuantity(q);
                                    return q;
                                }
                            }
                        }
                        else
                        {
                            // there is no assignation here  
                            // the code goes here when we don't find '=' equal sign.

                            QsVar qv = new QsVar(this, line);
                            //only print the result.
                            var q = qv.Execute();
                            if(q!= null) PrintQuantity(q);
                            return q;
                        }
                    }
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
                    catch (Exception e)
                    {
                        throw new QsException("Unhandled", e);
                    }

                }
                return null;
            }
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

                QsNameSpace ns = o as QsNameSpace;

                if (ns != null)
                {
                    return ns.GetName(name);
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
        public static QsValue GetScopeQsValue(Scope scope, string nameSpace, string name)
        {

            if (string.IsNullOrEmpty(nameSpace))
            {
                object q;

                ((ScopeStorage)scope.Storage).TryGetValue(name, true, out q);

                if (q != null)
                {
                    return (QsValue)q;
                }
                else
                {
                    throw new QsVariableNotFoundException("Variable '" + name + "' Not Found.");
                }
            }
            else
            {
                //get the variable from the name space
                object o;

                ((ScopeStorage)scope.Storage).TryGetValue(nameSpace, true, out o);

                QsNameSpace ns = o as QsNameSpace;

                QsValue r = null;

                if (ns != null)
                {
                    r = (QsValue)ns.GetName(name);
                }

                if (r == null)
                {
                    //try to get it from the Qs.Modules.QsModule property
                    try
                    {
                        var module = GetQsNameSpace(nameSpace);
                        var prop = module.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                        r = (QsValue)prop.GetValue(null, null);
                    }
                    catch (Exception e)
                    {
                        throw new QsVariableNotFoundException("Variable '" + name + "' Not Found In Namespace '" + nameSpace + "'", e);
                    }
                }

                return r;
            }

        }
        #endregion


        #region Helpers for getting qsnampespaces

        /// <summary>
        /// The namespace is a static C# class under Qs.Modules.*
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static Type GetQsNameSpace(string nameSpace)
        {
            string cls = "Qs.Modules." + nameSpace;


            //try the current assembly

            Type ns = Type.GetType(cls, false, true);

            if (ns == null)
            {
                // try  another search in the Qs*.dll dlls

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\Modules");
                var files = di.GetFiles("Qs*.dll");
                foreach (var file in files)
                {
                    var a = Assembly.LoadFrom(file.FullName);
                    ns = a.GetType(cls, false, true);//+ ", " + file.Name.TrimEnd('.', 'd', 'l', 'l'));
                    if (ns != null) break;  //found the break and pop out from the loop.
                }
            }

            return ns;

        }
        #endregion

    }

}
