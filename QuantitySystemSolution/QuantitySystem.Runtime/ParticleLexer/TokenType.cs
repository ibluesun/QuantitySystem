using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ParticleLexer.TokenTypes
{
    public abstract class TokenType
    {

        public Regex Regex
        {
            get; 
            private set;
        }

        //cache token regexes
        static Dictionary<Type, Regex> regexes = new Dictionary<Type, Regex>();

        /*
         * worth mentioned note that when I cached the regexes in this part the console calculations went very fast
         * I couldn't imagine that the reflection here make a lot of slow.
         */

        public TokenType()
        {

            Type tclass = this.GetType();
            
            Regex j;
            if (regexes.TryGetValue(tclass, out j))
            {
                Regex = j;
            }
            else
            {
                TokenPatternAttribute TPA = this.GetType().GetCustomAttributes(false)[0] as TokenPatternAttribute;

                if (TPA != null)
                {
                    Regex = new Regex("^" + TPA.RegexPattern + "$", RegexOptions.Compiled);
                }

                regexes.Add(tclass, Regex);
            }
        }
    }


    /// <summary>
    /// Any charachter
    /// </summary>
    public class CharToken : TokenType
    {

    }

    /// <summary>
    /// () groups
    /// </summary>
    public class ParenthesisGroupToken : TokenType
    {
        
    }

    [TokenPattern(RegexPattern = "(")]
    public class LeftParenthesisToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = ")")]
    public class RightParenthesisToken : TokenType
    {
    }


    /// <summary>
    /// [] Groups
    /// </summary>
    public class SquareBracketGroupToken : TokenType
    {

    }

    [TokenPattern(RegexPattern = "[")]
    public class LeftSquareBracketToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "]")]
    public class RightSquareBracketToken : TokenType
    {
    }

    /// <summary>
    /// {} Groups
    /// </summary>
    public class CurlyBracketGroupToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "{")]
    public class LeftCurlyBracketToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "}")]
    public class RightCurlyBracketToken : TokenType
    {
    }

    /// <summary>
    /// || Groups
    /// </summary>
    public class MatrixGroupToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "|")]
    public class MatrixBracketToken : TokenType
    {
    }


    [TokenPattern(RegexPattern="\\w+")]
    public class WordToken : TokenType
    {
    }



    [TokenPattern(RegexPattern = @"\|[^\|]+?\|")]
    public class AbsoluteToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = @"\|\|[^\|]+\|\|")]
    public class MagnitudeToken : TokenType
    {
    }


    [TokenPattern(RegexPattern=@"\d+(\.|\.\d+)?([eE][-+]?\d+)?")]
    public class NumberToken : TokenType
    {
    }

    [TokenPattern(
        RegexPattern = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?"                       //floating number
        + "\\s*<(°?\\w+!?(\\^\\d+)?\\.?)+(/(°?\\w+!?(\\^\\d+)?\\.?)+)?>"          // unit itself.
        )
    ]
    public class UnitizedNumberToken : TokenType
    {

    }


    [TokenPattern(RegexPattern = "=")]
    public class AssignmentOperatorToken : TokenType
    {
    }



    [TokenPattern(RegexPattern = ",")]
    public class CommaToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = ";")]
    public class SemiColonToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = ":")]
    public class ColonToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = @"\s+")]
    public class SpaceToken : TokenType
    {
    }


    /// <summary>
    /// fifo(p1, p2, ..., pn)    whole token
    /// </summary>
    public class FunctionCallToken : TokenType
    {
    }

    /// <summary>
    /// (p1, p2, ..., pn)  p(s) inside parenthesis
    /// </summary>
    public class FunctionParameterToken : TokenType
    {
    }



    #region Sequence tokens
    [TokenPattern(RegexPattern = "..>")]
    public class PositiveSequenceToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "<..")]
    public class NegativeSequenceToken : TokenType
    {
    }
    
    /// <summary>
    /// a=S[u SequenceRangeToken ]   like  a=S[2++5]
    /// The operator range that appears in the sequence calling square brackets n..m
    /// where .. is
    ///         ++ Series:                  Sum elements                    returns Scalar
    ///         ** Product:                 Multiply elements               returns Scalar
    ///         !! Average:                 Get the Mean of elements.       returns Scalar.
    ///         ## Standard Deviation       
    ///         .. Range                                                    returns Vector if components are scalars, Matrix if components are vectors
    /// </summary>
    [TokenPattern(RegexPattern = "((\\+\\+)?|(!!)?|(\\*\\*)?|(\\#\\#)?|(\\.\\.)?)")]
    public class SequenceRangeToken : TokenType
    {
    }

    /// <summary>
    /// fifo[i1, i2, ..., in](p1, p2, ..., p3)    whole token
    /// </summary>
    public class SequenceCallToken : TokenType
    {
    }

    /// <summary>
    /// [i1, i2, sequenceIndexToken, ..., in]  i(s) inside parenthesis
    /// </summary>
    public class SequenceIndexToken : TokenType
    {
    }

    /// <summary>
    /// S[] ..> elementToken; elementToken; elementToken.
    /// </summary>
    public class SequenceElementToken : TokenType
    {
    }

    #endregion



    #region Matlab operators
    [TokenPattern(RegexPattern = @"\.\*")]
    public class MatlabArrayMultilply : TokenType
    {
    }

    [TokenPattern(RegexPattern = @"\.\/")]
    public class MatlabArrayDivide : TokenType
    {
    }

    [TokenPattern(RegexPattern = @"\.\^")]
    public class MatlabArrayPower : TokenType
    {
    }



    #endregion

}
