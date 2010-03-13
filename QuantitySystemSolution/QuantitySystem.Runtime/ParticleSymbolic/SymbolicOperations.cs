﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;
using ParticleLexer.StandardTokens;
using ParticleConsole.QsTokens;
using System.Globalization;
using Qs;

namespace ParticleSymbolic
{
    public static class SymbolicOperations
    {

        private class DifferentiableExpression
        {
            public DifferentiableExpression Next;

            public Token DifferentialGroupToken;
            public string DifferentialResult;
            public Token Operation;
        }


        /// <summary>
        /// Differentiate the expression based on the required variable.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static  string Diff(string equation, string variable)
        {
            Token tokens = TokenizeExpression(equation);

            //now all premitive are tokenized 

            // to differentiate we should separate + and - 

            int ix = 0;                 //this is the index in the discovered tokens

            DifferentiableExpression RootDE = new DifferentiableExpression();
            DifferentiableExpression CurrentDE = RootDE;

            Token Group = new Token();

            while (ix < tokens.Count)
            {
                if (
                    tokens[ix].TokenClassType != typeof(PlusToken) &&
                    tokens[ix].TokenClassType != typeof(MinusToken)
                    )
                {
                    Group.AppendSubToken(tokens[ix]);
                }
                else
                {
                    //grouping done, put this group in the list.
                    CurrentDE.DifferentialGroupToken = Group;
                    CurrentDE.DifferentialResult = DiffPart(Group, variable);

                    if (ix < tokens.Count)
                    {
                        CurrentDE.Operation = tokens[ix]; //the current positive or negative token

                        CurrentDE.Next = new DifferentiableExpression();

                        CurrentDE = CurrentDE.Next;

                        Group = new Token();
                    }
                }

                ix++;
            }

            CurrentDE.DifferentialGroupToken = Group;
            CurrentDE.DifferentialResult = DiffPart(Group, variable);


            // make another pass to form the result.
            CurrentDE = RootDE;

            string result = string.Empty;


            while (CurrentDE != null)
            {
                if (string.IsNullOrEmpty(CurrentDE.DifferentialResult))
                {
                    result += CurrentDE.DifferentialGroupToken.TokenValue;
                }
                else
                {
                    result += CurrentDE.DifferentialResult;
                }

                if (CurrentDE.Operation != null) result += CurrentDE.Operation.TokenValue;

                CurrentDE = CurrentDE.Next;
            }




            return result;
        }



        /// <summary>
        /// Take token {which doesn't contain any + or -}
        /// and then differentiate it.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        private static string DiffPart(Token part, string variable)
        {
            //the code will go back and forward to adjust the derivation.

            int ix = 0;
            while (ix < part.Count)
            {
                if (part[ix].TokenValue.Equals(variable, StringComparison.OrdinalIgnoreCase))
                {
                    //check if it has ^ token after it 

                    string PowerPart = string.Empty;
                    if (ix < part.Count)
                    {
                        //there is still tokens to consume
                        if (part[ix + 1].TokenClassType == typeof(CaretToken))
                        {
                            PowerPart = part[ix + 2].TokenValue;
                        }

                        //check the value before the variable
                        string CoeffecientPart = string.Empty;
                        if (ix > 1)
                        {
                            CoeffecientPart = part[ix - 2].TokenValue;
                        }

                        double NumericalPowerPart;
                        double NumericalCoeffecientPart;

                        if (double.TryParse(PowerPart, out NumericalPowerPart))
                        {
                            //succeed 
                            if (double.TryParse(CoeffecientPart, out NumericalCoeffecientPart))
                            {
                                // a*x^b

                                // a=a*b
                                double NewNumericalCoeffecient = NumericalPowerPart * NumericalCoeffecientPart;

                                // b=b-1
                                double NewNumericalPowerPart = NumericalPowerPart - 1;

                                // replace a*x^b with a*b*x^b-1

                                string result = NewNumericalCoeffecient.ToString(CultureInfo.InvariantCulture);
                                result += part[ix - 1].TokenValue;
                                result += part[ix].TokenValue;
                                if (NewNumericalPowerPart > 1)
                                {
                                    result += part[ix + 1].TokenValue;
                                    result += NewNumericalPowerPart.ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    //omit the zero power.
                                }

                                return result;
                            }
                        }

                    }
                    else
                    {
                        //no there are not

                    }

                }
                ix++;

            }

            
            return string.Empty;
        }




        public static string compute(string expression)
        {
            Token tokens = TokenizeExpression(expression);

            //suppose that we have x*x 
            //   then the output is x^2
            //  suppose that we have  3*x*x*x
            //    then the output is 3*x^3
            // suppose we have 4*a*3*x^4+5*sin(x)-2*(x^4-3*x^2)
            //    then the ouput is 12*a*x^4  +5*sin(x)-2*(x^4-3*x^2)
            // this means LEAVE the brackets.

            // make the multiplications and divisions first
            // leave the brackets as it is 
            // in every term
            //      1- inspect every word
            //      2- inspect every number
            // make the same as the calculation of numbers but instead
            //  make the expression tree to mix numbers and words.

            throw new Exception();

        }


        private static Token TokenizeExpression(string expression)
        {
            var tokens = Token.ParseText(expression);

            tokens = tokens.MergeTokens(new MultipleSpaceToken());

            #region Conditions
            tokens = tokens.MergeTokens(new WhenStatementToken());

            tokens = tokens.MergeTokens(new OtherwiseStatementToken());

            tokens = tokens.MergeTokens(new AndStatementToken());

            tokens = tokens.MergeTokens(new OrStatementToken());

            tokens = tokens.MergeTokens(new EqualityToken());

            tokens = tokens.MergeTokens(new InEqualityToken());

            tokens = tokens.MergeTokens(new LessThanOrEqualToken());

            tokens = tokens.MergeTokens(new GreaterThanOrEqualToken());

            #endregion

            tokens = tokens.MergeTokens(new WordToken());                 //discover words
            tokens = tokens.MergeTokens(new NumberToken());               //discover the numbers
            //tokens = tokens.MergeTokens<UnitToken>();
            //tokens = tokens.MergeTokens(new UnitizedNumberToken());   //discover the unitized numbers

            tokens = tokens.MergeTokens<TensorProductToken>();

            tokens = tokens.MergeTokens(new NameSpaceToken());
            tokens = tokens.MergeTokens(new NameSpaceAndValueToken());

            tokens = tokens.MergeTokens<FunctionValueToken>();

            tokens = tokens.MergeTokensInGroups(
                new ParenthesisGroupToken(),                // group (--()-) parenthesis
                new SquareBracketsGroupToken(),             // [[][][]]
                new CurlyBracketGroupToken()                //  {{}}{}
                );


            tokens = tokens.MergeTokens<MagnitudeToken>();

            tokens = tokens.MergeTokens<AbsoluteToken>();

            tokens = tokens.RemoveSpaceTokens();                           //remove all spaces

            tokens = tokens.DiscoverQsCalls(StringComparer.OrdinalIgnoreCase,
                new string[] { "When", "Otherwise", "And", "Or" }
                );

            return tokens;
        }

    }
}
