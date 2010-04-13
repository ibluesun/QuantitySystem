using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;
using ParticleLexer.StandardTokens;
using QuantitySystem.Units;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting;
using System.Globalization;
using ParticleLexer.QsTokens;

namespace Qs.Runtime
{
    public partial class QsSequence : SortedList<int, QsSequenceElement>
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
                    gg += v.ToString(CultureInfo.InvariantCulture).Trim() + ": " + base[v].ElementDeclaration + "; ";
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
        public string SequenceSymbolicName
        {
            get
            {
                //% indexes number.
                //# parameters number.
                return FormSequenceSymbolicName(sequenceName, 1, Parameters.Length);
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

            if (sequence.IndexOf("..>") < 0)
            {
                // no forward operator
                if (sequence.IndexOf("<..") < 0)
                {
                    // no backward operator.
                    return null;    //fast check because sequence have
                }
            }

            Token t = Token.ParseText(sequence);


            t = t.MergeTokens(new MultipleSpaceToken());

            t = t.MergeTokens(new PositiveSequenceToken());  //    ..>  start from 0 index to +ve
            t = t.MergeTokens(new NegativeSequenceToken());  //    <..  start from -1 index to -ve


            if (t.IndexOf(typeof(PositiveSequenceToken)) > -1)
            {
                t = t.RemoveTokenUntil(typeof(MultipleSpaceToken), typeof(PositiveSequenceToken));
            }
            else if (t.IndexOf(typeof(NegativeSequenceToken)) > -1)
            {
                t = t.RemoveTokenUntil(typeof(MultipleSpaceToken), typeof(PositiveSequenceToken));
            }
            else
            {
                return null;
            }            

            t = t.MergeTokens(new WordToken());
            t = t.MergeTokens(new NumberToken());
            t = t.MergeTokens(new UnitizedNumberToken());

            t = t.MergeTokens(new NameSpaceToken());

            t = t.MergeTokensInGroups(new ParenthesisGroupToken(), new SquareBracketsGroupToken());


            int nsidx = 0; // surve as a base for indexing token if there is namespace it will be 1 otherwise remain 0

            string declaredNamespace = string.Empty;
            if (t[0].TokenClassType == typeof(NameSpaceToken))
            {
                nsidx = 1; //the function begin with namespace.
                declaredNamespace = t[0][0].TokenValue;
            }


            if (t[nsidx + 0].TokenClassType == typeof(WordToken)
                && (t.Count > 1 ? t[nsidx + 1].TokenClassType == typeof(SquareBracketsGroupToken) : false)     // test for second tokek to be [] group
                )
            {

                Type SequenceTokenType = null;

                int shift = 0;

                if ((nsidx + t.Count) > 2)
                {
                    //check for sequence operator
                    if (t[nsidx + 2].TokenClassType == typeof(PositiveSequenceToken) || t[nsidx + 2].TokenClassType == typeof(NegativeSequenceToken))
                    {
                        //reaching here means the sequence doesn't have parameters only indexers.
                        SequenceTokenType = t[nsidx + 2].TokenClassType;
                    }
                    else if ((nsidx + t.Count) > 4)
                    {
                        if (t[nsidx + 3].TokenClassType == typeof(PositiveSequenceToken) || t[nsidx + 3].TokenClassType == typeof(NegativeSequenceToken))
                        {
                            //reaching here means the sequence has parameterized arguments.
                            SequenceTokenType = t[nsidx + 3].TokenClassType;
                            shift = nsidx + 1;
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
                string sequenceName = t[nsidx + 0].TokenValue;

                // get indexes
                string[] indexes = (from c in t[nsidx + 1]
                                    where c.TokenClassType == typeof(WordToken)
                                    select c.TokenValue).ToArray();


                if (indexes.Length > 1) throw new QsException("Sequences with more than one index are not supported now");


                // get parameters
                string[] parameters = {}; //array with zero count :)
                if (t[nsidx + 2].TokenClassType == typeof(ParenthesisGroupToken))
                {
                    parameters = (from c in t[nsidx + 2]
                                  where c.TokenClassType == typeof(WordToken)
                                  select c.TokenValue).ToArray();
                }



                //make all things between ';' be a whole word.

                t = t.MergeAllBut(nsidx + 3 + shift, typeof(SequenceElementToken), new SemiColonToken());

                QsSequence seqo = GetSequence(qse.Scope, declaredNamespace, FormSequenceSymbolicName(sequenceName, indexes.Length, parameters.Length));


                if (seqo == null)
                {
                    if (SequenceTokenType == typeof(NegativeSequenceToken))
                    {
                        throw new QsException("You can't initialize negative sequence elements without inititialize positive sequence element(s) first");
                    }

                    seqo = new QsSequence(indexes.Length > 0 ? indexes[0] : string.Empty, parameters)
                    {
                        SequenceSymbolicName = sequenceName,
                        SequenceDeclaration = t[nsidx + 0].TokenValue + t[nsidx + 1].TokenValue + (shift == nsidx + 1 ? t[nsidx + 2].TokenValue : ""),
                        SequenceNamespace = declaredNamespace
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
                            SequenceSymbolicName = sequenceName,
                            SequenceDeclaration = t[nsidx + 0].TokenValue + t[nsidx + 1].TokenValue + (shift == nsidx + 1 ? t[nsidx + 2].TokenValue : "")

                        };

                    }
                    else
                    {
                        seqo.CachedValues.Clear();  //clear all cache because we are defining extra elements.
                    }
                }

                //beginElement is zero index element in positive sequence and -1 index element in negative sequence.

                QsSequenceElement beginElement = QsSequenceElement.Parse(t[nsidx + 3 + shift].TokenValue, qse, seqo);

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

                while ((nsidx + seqoIndex) < t.Count)
                {
                    if (t[nsidx + seqoIndex].TokenClassType != typeof(SemiColonToken))
                    {
                        //assuming for now all entered values are quantities.
                        QsSequenceElement seqoElement = QsSequenceElement.Parse(t[nsidx + seqoIndex].TokenValue, qse, seqo);
                        
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


        public static QsSequence GetSequence(Scope scope, string qsNamespace, string sequenceName)
        {
            if (string.IsNullOrEmpty(qsNamespace))
            {
                // no namespace included then it is from the local scope.

                var seq = (QsSequence)QsEvaluator.GetScopeVariable(scope, qsNamespace, sequenceName);

                return seq;

            }
            else
            {
                try
                {

                    QsNamespace ns = QsNamespace.GetNamespace(scope, qsNamespace);


                    return (QsSequence)ns.GetValue(sequenceName);

                }
                catch (QsVariableNotFoundException)
                {
                    return null;
                }

            }

        }


        /// <summary>
        /// Helper function to create the name of the sequence.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="indexesCount"></param>
        /// <param name="parametersCount"></param>
        /// <returns></returns>
        public static string FormSequenceSymbolicName(string name, int indexesCount, int parametersCount)
        {
            if (indexesCount == 0) indexesCount = 1;

            string sn = name + "%" + indexesCount.ToString(CultureInfo.InvariantCulture); // +"#" + parametersCount.ToString(CultureInfo.InvariantCulture);
            return sn;


        }
    }
}
