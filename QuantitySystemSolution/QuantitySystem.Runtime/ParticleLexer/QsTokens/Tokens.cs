using ParticleLexer;
using ParticleLexer.StandardTokens;
using System;
using System.Linq;

namespace ParticleLexer.QsTokens
{

    /// <summary>
    /// unit token form &lt;unit&gt;
    /// </summary>
    [TokenPattern(
        RegexPattern = "<(°?\\w+!?(\\^\\d+)?\\.?)+(/(°?\\w+!?(\\^\\d+)?\\.?)+)?>"
        , ShouldBeginWith = "<", ShouldEndWith = ">"
        )
    ]
    public class UnitToken : TokenClass { }

    /// <summary>
    /// Unitized number is
    /// 90, 90.9
    /// 90.9e2, 90.9e+2, 90.9e-2
    /// 90.09&lt;m&gt;, 90.2e+2&lt;m&gt;, etc.
    /// </summary>
    [TokenPattern(
        RegexPattern = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?"                       //floating number
        + "(\\s*<(°?\\w+!?(\\^\\d+)?\\.?)+(/(°?\\w+!?(\\^\\d+)?\\.?)+)?>)?"          // unit itself.
        )
    ]
    public class UnitizedNumberToken : TokenClass
    {

    }


    #region Sequence tokens

    /// <summary>
    /// -> Token
    /// </summary>
    [TokenPattern(RegexPattern="->", ExactWord=true)]
    public class PointerOperatorToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = "\\.\\.", ExactWord = true, ShouldBeginWith = ".")]
    public class VectorRangeToken : TokenClass
    {
    }

    /// <summary>
    /// ..>  token
    /// </summary>
    [TokenPattern(RegexPattern = "\\.\\.>", ExactWord = true, ShouldBeginWith = ".")]
    public class PositiveSequenceToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "<\\.\\.", ExactWord = true, ShouldBeginWith = "<")]
    public class NegativeSequenceToken : TokenClass
    {
    }

    /// <summary>
    /// a=S[u SequenceRangeToken ]   like  a=S[2++5]
    /// The operator range that appears in the sequence calling square brackets n..m
    /// where .. is
    ///         ++ Series:                  Sum elements                    returns Scalar
    ///         ** Product:                 Multiply elements               returns Scalar
    ///         !! Average:                 Get the Mean of elements.       returns Scalar.
    ///         !% Standard Deviation       
    ///         .. Range                                                    returns Vector if components are scalars, Matrix if components are vectors
    /// </summary>
    [TokenPattern(RegexPattern = "((\\+\\+)?|(!!)?|(\\*\\*)?|(!%)?|(\\.\\.)?)")]
    public class SequenceRangeToken : TokenClass
    {
    }

    /// <summary>
    /// fifo[i1, i2, ..., in](p1, p2, ..., p3)    whole token
    /// </summary>
    public class SequenceCallToken : TokenClass
    {
    }

    /// <summary>
    /// [i1, i2, sequenceIndexToken, ..., in]  i(s) inside parenthesis
    /// </summary>
    public class SequenceIndexToken : TokenClass
    {
    }

    /// <summary>
    /// S[] ..> elementToken; elementToken; elementToken.
    /// </summary>
    public class SequenceElementToken : TokenClass
    {
    }

    #endregion


    /// <summary>
    /// Reference token on the form WordToken, ColonToken  x:  oh:  D80: may contain in its end letters
    /// 
    /// May begin with ':'
    /// Must Begin with alphabet [a-zA-Z]
    /// Follwed by any number of letter  \w*
    /// may contain ':' in the first and must contain ':' at the end
    /// </summary>
    [TokenPattern(RegexPattern = @"(:*[a-zA-Z]\w*:)+", ContinueTestAfterSuccess = true)]   // when merging tokens if a success happen then continue merge until a failure happen or consume success as much as you can
    public class NamespaceToken : TokenClass
    {
    }

    /// <summary>
    /// Reference Namespace with its value  x:r   x:u  xd:Abs  x:r:t y:t:@e etc...   
    /// NamespaceToken
    /// </summary>
    [TokenPattern(RegexPattern = @"(:*[a-zA-Z]\w*:)+@?[a-zA-Z]\w*", ContinousToken = true)]
    public class NameSpaceAndVariableToken : TokenClass
    {
    }

    
    /// <summary>
    /// Adding '@' before function name like @f(x)  return the function body
    /// </summary>
    [TokenPattern(RegexPattern = @"(@\w+|@\w+\([\sa-zA-Z0-9,]*\)|@\w+:\w+|@\w+:\w+\([\sa-zA-Z0-9,]*\))")]
    public class FunctionValueToken : TokenClass
    {
    }

    /// <summary>
    /// Function Value followed by unit
    /// No Regex Token
    /// </summary>
    public class FunctionQuantityToken : TokenClass
    {
    }


    /// <summary>
    /// Left token of absolute group
    /// </summary>
    [TokenPattern(RegexPattern = @"_\|", ExactWord = true)]
    public class LeftAbsoluteToken : TokenClass
    {
    }

    /// <summary>
    /// right token of absolute group
    /// </summary>
    [TokenPattern(RegexPattern = @"\|_", ExactWord = true)]
    public class RightAbsoluteToken : TokenClass
    {
    }

    /// <summary>
    /// left token of norm group
    /// </summary>
    [TokenPattern(RegexPattern = @"_\|\|", ExactWord = true)]
    public class LeftNormToken : TokenClass
    {
    }

    /// <summary>
    /// right token of norm group
    /// </summary>
    [TokenPattern(RegexPattern = @"\|\|_", ExactWord = true)]
    public class RightNormToken : TokenClass
    {
    }



    /// <summary>
    /// Double Vertical Bar
    /// </summary>
    [TokenPattern(RegexPattern = @"\|\|", ExactWord = true)]
    public class DoubleVerticalBarToken : TokenClass
    {
    }


    /// <summary>
    /// \/ nabla operator :)
    /// </summary>
    [TokenPattern(RegexPattern = @"\\.*\/")]
    public class Nabla : TokenClass
    {
    }



    /// <summary>
    /// When
    /// </summary>
    [TokenPattern(RegexPattern = "when", ExactWord = true)]
    public class WhenStatementToken : TokenClass
    {
    }

    /// <summary>
    /// Otherwise
    /// </summary>
    [TokenPattern(RegexPattern = "otherwise", ExactWord = true)]
    public class OtherwiseStatementToken : TokenClass
    {
    }



    [TokenPattern(RegexPattern = @"\^\.", ExactWord = true)]
    public class PowerDotToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\^x", ExactWord = true)]
    public class PowerCrossToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\(\*\)", ExactWord = true)]
    public class TensorProductToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = "!=", ExactWord = true)]
    public class InEqualityToken : TokenClass
    {
    }

    /// <summary>
    /// Equality == operator
    /// </summary>
    [TokenPattern(RegexPattern = "==", ExactWord = true)]
    public class EqualityToken : TokenClass
    {
    }

    /// <summary>
    /// AND operator keyword
    /// </summary>
    [TokenPattern(RegexPattern = "and", ExactWord = true)]
    public class AndStatementToken : TokenClass
    {
    }

    /// <summary>
    /// OR operator keyword
    /// </summary>
    [TokenPattern(RegexPattern = "or", ExactWord = true)]
    public class OrStatementToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = "<<", ExactWord = true)]
    public class LeftShiftToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = ">>", ExactWord = true)]
    public class RightShiftToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = @"<\|", ExactWord = true)]
    public class LeftTensorToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\|>", ExactWord = true)]
    public class RightTensorToken : TokenClass
    {
    }


    /// <summary>
    /// Dollar Sign followed by word token. $x or $y  $ROI 
    /// also can be used for ${x*x*y}   any text between brackets will be parsed by the symbolicvariable praser
    /// </summary>
    [TokenPattern(RegexPattern = @"\$\{.+\}", ShouldBeginWith = "$", ShouldEndWith = "}")]
    public class SymbolicToken : TokenClass
    {

    }

    /// <summary>
    /// $symbol with unit  :) i.e.  $x&lt;kg&gt;
    /// </summary>
    public class SymbolicQuantityToken : TokenClass
    {
    }


    /// <summary>
    /// C{3 4}  
    /// </summary>
    [TokenPattern(RegexPattern = @"C\{.+\}", ShouldBeginWith = "C", ShouldEndWith = "}")]
    public class ComplexNumberToken : TokenClass
    {
    }

    /// <summary>
    /// @{(t) = t^2}
    /// </summary>
    [TokenPattern(RegexPattern = @"@\{.+\}", ShouldBeginWith = "@", ShouldEndWith = "}")]
    public class FunctionLambdaToken : TokenClass
    {
    }

    /// <summary>
    /// Complex number with unit C{2 1}&lt;kg&gt;
    /// </summary>
    public class ComplexQuantityToken : TokenClass
    {
    }

    /// <summary>
    /// H{3 4 2 1}
    /// </summary>
    [TokenPattern(RegexPattern = @"H\{.+\}", ShouldBeginWith = "H", ShouldEndWith = "}")]
    public class QuaternionNumberToken : TokenClass
    {
    }

    /// <summary>
    /// Quaternion number with unit H{2 1 5 4}&lt;kg&gt;
    /// </summary>
    public class QuaternionQuantityToken : TokenClass
    {
    }


    /// <summary>
    /// Q{1 2}
    /// </summary>
    [TokenPattern(RegexPattern = @"Q\{.+\}", ShouldBeginWith = "Q", ShouldEndWith = "}")]
    public class RationalNumberToken : TokenClass
    {
    }

    /// <summary>
    /// Rational number with unit Q{1 2}&lt;kg&gt;
    /// </summary>
    public class RationalQuantityToken : TokenClass
    {
    }


    /// <summary>
    /// Express groups of '&lt;| a b c |>'
    /// </summary>
    public class TensorGroupToken : GroupTokenClass
    {
        public TensorGroupToken()
            : base(new LeftTensorToken(), new RightTensorToken())
        {
        }
    }

    public class AbsoluteGroupToken : GroupTokenClass
    {
        public AbsoluteGroupToken()
            : base(new LeftAbsoluteToken(), new RightAbsoluteToken())
        {
        }
    }

    public class NormGroupToken : GroupTokenClass
    {
        public NormGroupToken()
            : base(new LeftNormToken(), new RightNormToken())
        {
        }
    }


    /// <summary>
    /// Mathches \"    
    /// </summary>
    [TokenPattern(RegexPattern = @"\\""", ExactWord = true)]
    public class QuotationMarkEscapeToken : TokenClass
    {
        
    }

    /// <summary>
    /// Text between two single qutation.
    /// </summary>
    public class TextToken : TokenClass
    {
    }


    /// <summary>
    /// This token type is for internal use only.
    /// </summary>
    internal class MergedToken : TokenClass
    {
    }

    public static class TokenExtensions
    {
        /// <summary>
        /// Should be used after merging by word
        /// and grouping brackets
        /// The function tries to find Word followed by complete parenthesis group or square brackets group.
        /// And can parse this example:
        ///            fn(434,fn((434+434)+8/4,50,fifo(5)))
        ///            
        ///            S[5,F[4,2,1],G](4, 3, 2,R[2], p(30*o(9)))
        ///            -              - Parameters              -
        ///            -    Indexes   -
        ///            -              Sequence Call             -
        /// </summary>
        /// <param name="ignoreWords">list of words that should be ignored when discovering calls </param>
        /// <returns></returns>
        public static Token DiscoverQsCalls(this Token token, StringComparer stringComparer, params string[] ignoreWords)
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;

            while (ci < token.Count)
            {
                var c = token[ci];

                if (c.Contains(typeof(ParenthesisGroupToken)) | c.Contains(typeof(SquareBracketsGroupToken)))
                {
                    //recursive call if the token have inside groups
                    current.AppendSubToken(c.DiscoverQsCalls(stringComparer, ignoreWords));
                }
                else
                {
                    //sub groups then test this token 
                    if (
                        (
                              c.TokenClassType == typeof(WordToken)               // word token
                           || c.TokenClassType == typeof(NameSpaceAndVariableToken)  // or namespace:value token
                        )
                        && ignoreWords.Contains(c.TokenValue, stringComparer) == false         // and the whole value is not in  the ignore words
                        )
                    {
                        //check if the next token is group
                        if (ci < token.Count - 1)
                        {
                            Token cnext = token[ci + 1];

                            #region Parenthesis group discovery
                            if (cnext.TokenClassType == typeof(ParenthesisGroupToken))
                            {
                                // so this is a function
                                //take the current token with the next token and make it as functionToken

                                Token functionCallToken = new Token();
                                functionCallToken.TokenClassType = typeof(ParenthesisCallToken);
                                functionCallToken.AppendSubToken(c);



                                if (cnext.Contains((typeof(ParenthesisGroupToken))) | cnext.Contains(typeof(SquareBracketsGroupToken)))
                                {
                                    cnext = cnext.DiscoverQsCalls(stringComparer, ignoreWords);
                                }


                                cnext = token.SplitParamerers(cnext, new CommaToken());


                                functionCallToken.AppendSubToken(cnext);

                                current.AppendSubToken(functionCallToken);

                                ci += 2;
                                continue;
                            }
                            #endregion

                            #region Square Brackets discovery
                            if (cnext.TokenClassType == typeof(SquareBracketsGroupToken))
                            {
                                // so this is a sequence
                                //take the current token with the next token and make it as sequenceToken

                                Token sequenceCallToken = new Token();
                                sequenceCallToken.TokenClassType = typeof(SequenceCallToken);
                                sequenceCallToken.AppendSubToken(c);

                                if (cnext.Contains((typeof(SquareBracketsGroupToken))) | cnext.Contains((typeof(ParenthesisGroupToken))))
                                {
                                    cnext = cnext.DiscoverQsCalls(stringComparer, ignoreWords);
                                }

                                cnext = token.SplitParamerers(cnext, new CommaToken());

                                sequenceCallToken.AppendSubToken(cnext);

                                if (token.Count > ci + 2)
                                {
                                    //check if we have a Parenthesis parameters after Square Brackets
                                    Token cnextnext = token[ci + 2];
                                    if (cnextnext.TokenClassType == typeof(ParenthesisGroupToken))
                                    {
                                        //then this is a sequence call with parameters.
                                        if ((cnextnext.Contains((typeof(SquareBracketsGroupToken))) | cnextnext.Contains((typeof(ParenthesisGroupToken)))))
                                        {
                                            cnextnext = cnextnext.DiscoverQsCalls(stringComparer, ignoreWords);
                                        }

                                        cnextnext = token.SplitParamerers(cnextnext, new CommaToken());

                                        sequenceCallToken.AppendSubToken(cnextnext);

                                        ci += 3;
                                    }
                                    else
                                    {
                                        ci += 2;
                                    }
                                }
                                else
                                {
                                    ci += 2;
                                }
                                current.AppendSubToken(sequenceCallToken);
                                continue;
                            }

                            #endregion

                        }
                    }

                    // if all conditions failed we put the token and resume to another one
                    current.AppendSubToken(token[ci]);
                }

                ci++;
            }
            first.TokenClassType = token.TokenClassType;

            return Token.Zabbat(first);
        }




        /// <summary>
        /// Parse text between " TEXT "
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static Token DiscoverQsTextTokens(this Token tokens)
        {

            // merge \" to be one charachter after this
            
            tokens = tokens.MergeTokens<QuotationMarkEscapeToken>();

            Token root = new Token();

            Token runner = root;

            //add every token until you encounter '


            int ix = 0;
            bool TextMode = false;
            while (ix < tokens.Count)
            {
                if (tokens[ix].TokenClassType == typeof(QuotationMarkToken))
                {
                    TextMode = !TextMode;

                    if (TextMode)
                    {
                        //true create the token
                        runner = new Token();
                        runner.TokenClassType = typeof(TextToken);
                        root.AppendSubToken(runner);

                        runner.AppendSubToken(tokens[ix]);

                    }
                    else
                    {
                        //false: return to root tokens
                        runner.AppendSubToken(tokens[ix]);

                        runner = root;
                    }
                }
                else
                {
                    runner.AppendSubToken(tokens[ix]);
                }


                ix++;
            
            }


            return root;
        }
    }

}
