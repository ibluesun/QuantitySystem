using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;
using ParticleLexer.TokenTypes;
using QuantitySystem.Units;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting;
using System.Globalization;

namespace Qs.Runtime
{
    public partial class QsSequence : Dictionary<int, QsSequenceElement>
    {
        private string sequenceDeclaration;
        public string SequenceDeclaration 
        {
            get
            {
                //go through all elements and print their index: text
                string gg = "";
                foreach (var v in base.Keys)
                {
                    gg += v.ToString(CultureInfo.InvariantCulture).Trim() + ": " + base[v].ElementDeclaration + ";";
                }
                return sequenceDeclaration + " ..> " + gg;
            }
            set
            {
                sequenceDeclaration = value;
            }
        }

        public QsParamInfo[] Parameters { get; private set; }
        public string SequenceIndexName { get; set; }

        private string sequenceName;
        public string SequenceName
        {
            get
            {
                //% indexes number.
                //# parameters number.
                return FormSequenceScopeName(sequenceName, 1, Parameters.Length);
            }
            private set
            {
                sequenceName = value;
            }
        }

        /// <summary>
        /// The default index name in the sequence without explicitly declared index name.
        /// </summary>
        public const string DefaultIndexName = "_1";

        public QsSequence(string indexName, string[] parameters)
        {
            if (!string.IsNullOrEmpty(indexName))
                SequenceIndexName = indexName;
            else
                SequenceIndexName = DefaultIndexName;

            this.Parameters = (from v in parameters select new QsParamInfo { Name = v }).ToArray();

            if (parameters.Length == 0) CachingEnabled = true;  //Allow caching for parameterless sequence.


        }



        /// <summary>
        /// Parse the give sequence text and return <see cref="QsSequence"/> if succeeded 
        /// otherwise return null reference.
        /// </summary>
        /// <param name="qse"></param>
        /// <param name="sequence">Sequence Text on the form S[i,j,k, ...]() ..> 40;50&lt;kg>; ..  </param>
        /// <returns></returns>
        public static QsSequence ParseSequence(QsEvaluator qse, string sequence)
        {
            Token t = Token.ParseText(sequence);


            t = t.MergeTokens(new SpaceToken());

            t = t.MergeTokens(new PositiveSequenceToken());  //    ..>  start from 0 index to +ve
            t = t.MergeTokens(new NegativeSequenceToken());  //    <..  start from -1 index to -ve

            if (t.IndexOf(typeof(PositiveSequenceToken)) > -1)
            {
                t = t.RemoveTokenUntil(typeof(SpaceToken), typeof(PositiveSequenceToken));
            }
            else if (t.IndexOf(typeof(NegativeSequenceToken)) > -1)
            {
                t = t.RemoveTokenUntil(typeof(SpaceToken), typeof(PositiveSequenceToken));
            }
            else
            {
                return null;
            }            

            t = t.MergeTokens(new WordToken());
            t = t.MergeTokens(new NumberToken());
            t = t.MergeTokens(new UnitizedNumberToken());
            t = t.GroupBrackets();



            t = t.MergeTokens(new SemiColonToken());



            if (t[0].TokenType == typeof(WordToken)
                && (t.Count > 1 ? t[1].TokenType == typeof(SquareBracketGroupToken) : false)     // test for second tokek to be [] group
                )
            {

                Type SequenceTokenType = null;

                int shift = 0;

                if (t.Count > 2)
                {
                    //check for sequence operator
                    if (t[2].TokenType == typeof(PositiveSequenceToken) || t[2].TokenType == typeof(NegativeSequenceToken))
                    {
                        SequenceTokenType = t[2].TokenType;
                    }
                    else if (t.Count > 4)
                    {
                        if (t[3].TokenType == typeof(PositiveSequenceToken) || t[3].TokenType == typeof(NegativeSequenceToken))
                        {
                            SequenceTokenType = t[3].TokenType;
                            shift = 1;
                        }
                        else return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }


                // s[]   found
                string sequenceName = t[0].TokenValue;

                // get indexes
                string[] indexes = (from c in t[1]
                                    where c.TokenType == typeof(WordToken)
                                    select c.TokenValue).ToArray();


                if (indexes.Length > 1) throw new QsException("Sequences with more than one index are not supported now");


                // get parameters
                string[] parameters = {}; //array with zero count :)
                if (t[2].TokenType == typeof(ParenthesisGroupToken))
                {
                    parameters = (from c in t[2]
                                  where c.TokenType == typeof(WordToken)
                                  select c.TokenValue).ToArray();
                }



                //make all things between ';' be a whole word.

                t = t.MergeAllBut(3 + shift, typeof(SequenceElementToken), new SemiColonToken());

                QsSequence seqo = GetSequence(qse.Scope, FormSequenceScopeName(sequenceName, indexes.Length, parameters.Length));


                if (seqo == null)
                {
                    if (SequenceTokenType == typeof(NegativeSequenceToken))
                    {
                        throw new QsException("You can't initialize negative sequence elements without inititialize positive sequence element(s) first");
                    }

                    seqo = new QsSequence(indexes.Length > 0 ? indexes[0] : string.Empty, parameters)
                    {
                        SequenceName = sequenceName,
                        SequenceDeclaration = t[0].TokenValue + t[1].TokenValue + (shift == 1 ? t[2].TokenValue : "")

                    };
                }
                else
                {
                    //sequence exist 
                    if (SequenceTokenType == typeof(PositiveSequenceToken))
                    {
                        //it meanse I am defining the sequence again and overwrite the previous one

                        seqo = new QsSequence(indexes.Length > 0 ? indexes[0] : string.Empty, parameters)
                        {
                            SequenceName = sequenceName,
                            SequenceDeclaration = t[0].TokenValue + t[1].TokenValue + (shift == 1 ? t[2].TokenValue : "")

                        };

                    }
                    else
                    {
                        seqo.CachedValues.Clear();  //clear all cache because we are defining extra elements.
                    }
                }

                //beginElement is zero index element in positive sequence and -1 index element in negative sequence.

                QsSequenceElement beginElement = QsSequenceElement.Parse(t[3 + shift].TokenValue, qse, seqo);

                if (SequenceTokenType == typeof(PositiveSequenceToken))
                    seqo[0] = beginElement;
                else
                    seqo[-1] = beginElement;



                // take the right side arguments to be added into the sequence.

                int seqoIndex = 5 + shift; // the argument with index 1
                
                int ix = 1;   //index of sequence.

                if (SequenceTokenType == typeof(NegativeSequenceToken))
                {
                    ix = -2;
                }

                while (seqoIndex < t.Count)
                {
                    if (t[seqoIndex].TokenType != typeof(SemiColonToken))
                    {
                        //assuming for now all entered values are quantities.
                        QsSequenceElement seqoElement = QsSequenceElement.Parse(t[seqoIndex].TokenValue, qse, seqo);
                        
                        seqo[ix] = seqoElement;  //-5 bacause I am starting from 1 index

                        if (SequenceTokenType == typeof(NegativeSequenceToken))
                            ix--;
                        else
                            ix++;
                    }

                    seqoIndex++;

                }

                return seqo;

            }
            else
            {
                return null;
            }
        }



        public override string ToString()
        {
            return SequenceDeclaration;
        }


        public static QsSequence GetSequence(Scope scope, string snName)
        {
            object q;


            scope.TryGetName(SymbolTable.StringToCaseInsensitiveId(snName), out q);


            return (QsSequence)q;

        }

        public static string FormSequenceScopeName(string name, int indexesCount, int parametersCount)
        {
            if (indexesCount == 0) indexesCount = 1;
            return name + "%" + indexesCount.ToString(CultureInfo.InvariantCulture) + "#" + parametersCount.ToString(CultureInfo.InvariantCulture);

        }
    }
}
