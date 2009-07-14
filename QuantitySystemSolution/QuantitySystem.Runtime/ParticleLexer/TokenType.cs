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


    public class CharToken : TokenType
    {

    }

    public class GroupToken : TokenType
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

    [TokenPattern(RegexPattern="\\w+")]
    public class WordToken : TokenType
    {
    }


    [TokenPattern(RegexPattern=@"\d+(\.|\.\d+)?([eE][-+]?\d+)?")]
    public class NumberToken : TokenType
    {
    }

    [TokenPattern(
        RegexPattern = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?" 
        + "\\s*<(\\w+!?(\\^\\d+)?\\.?)+(/(\\w+!?(\\^\\d+)?\\.?)+)?>"
        )
    ]
    public class UnitizedNumberToken : TokenType
    {

    }


    [TokenPattern(RegexPattern="=>")]
    public class LambdaOperatorToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "=")]
    public class AssignmentOperator : TokenType
    {
    }



    [TokenPattern(RegexPattern = ",")]
    public class CommaOperatorToken : TokenType
    {
    }


    public class FunctionCallToken : TokenType
    {
    }

    public class FunctionParameterToken : TokenType
    {
    }

    [TokenPattern(RegexPattern = "..>")]
    public class ForwardSeries : TokenType
    {
    }

    [TokenPattern(RegexPattern = "<..")]
    public class BackwardSeries : TokenType
    {
    }

}
