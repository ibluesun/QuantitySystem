using Qs.Types.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qs.Types
{
    public partial class QsVector : QsValue, IEnumerable<QsScalar>
    {

        private Dictionary<string, QsValue> Statistics = new Dictionary<string, QsValue>(StringComparer.OrdinalIgnoreCase);


        protected override void OnClearCache()
        {
            _AscendedVector = null;
            _DescendedVector = null;


            Statistics.Clear();
        }

        public QsScalar Mean()
        {
            if (!Statistics.ContainsKey("Mean"))
            {

                var total = this.Sum().DivideScalar(Count.ToQuantity().ToScalar());
                Statistics["Mean"] = total;
            }
            return (QsScalar)Statistics["Mean"];
        }

        QsVector _AscendedVector = null;

        public QsVector AscendedVecor
        {
            get
            {
                if (_AscendedVector == null)
                {
                    var ordered = ListStorage.OrderBy(v => v, new QsValueComparer());

                    _AscendedVector = new QsVector(ordered.ToArray());

                }
                return _AscendedVector;
            }

        }

        QsVector _DescendedVector = null;
        public QsVector DescendedVector
        {
            get
            {
                if (_DescendedVector == null)
                {
                    var ordered = ListStorage.OrderByDescending(v => v, new QsValueComparer());

                    _DescendedVector = new QsVector(ordered.ToArray());

                }
                return _DescendedVector;
            }
        }

        /// <summary>
        /// arranged data in ascending order .. it gives the middle value of odd elements count or the average of the two middle values of even elements count
        /// </summary>
        /// <returns></returns>
        public QsScalar Median()
        {
            if (!Statistics.ContainsKey("Median"))
            {
                var ao = AscendedVecor;
                Math.DivRem(ao.Count, 2, out var rm);
                if (rm == 0)
                {
                    // even number  so we take the average of the inner ones  
                    var aoc = ao.Count / 2;

                    var twos = ao[aoc - 1].AddScalar(ao[aoc]);

                    Statistics["Median"] = twos.DivideScalar(((double)2).ToQuantity().ToScalar());

                }
                else
                {
                    var aoc = (int)Math.Floor(ao.Count / 2.0);

                    Statistics["Median"] = ao[aoc];
                }
            }
            return (QsScalar)Statistics["Median"];
        }

        public QsVector LowerHalf()
        {
            if (!Statistics.ContainsKey("LowerHalf"))
            {

                QsVector ao = AscendedVecor;
                Math.DivRem(ao.Count, 2, out int rm);
                int aoc = 0;
                if (rm == 0)
                {
                    // even number  so we take the average of the inner ones  
                    aoc = ao.Count / 2;
                }
                else
                {
                    aoc = (int)Math.Floor(ao.Count / 2.0);
                }

                List<QsScalar> result = new List<QsScalar>();
                for (int i = 0; i < aoc; i++)
                    result.Add(ao[i]);

                Statistics["LowerHalf"] = new QsVector(result.ToArray());
            }

            return (QsVector)Statistics["LowerHalf"];

        }

        public QsVector UpperHalf()
        {
            if (!Statistics.ContainsKey("UpperHalf"))
            {

                QsVector ao = AscendedVecor;
                Math.DivRem(ao.Count, 2, out int rm);
                int aoc = 0;
                if (rm == 0)
                {
                    // even number  so we take the average of the inner ones  
                    aoc = ao.Count / 2;
                }
                else
                {
                    aoc = (int)Math.Ceiling(ao.Count / 2.0);
                }

                List<QsScalar> result = new List<QsScalar>();
                for (int i = aoc; i < ao.Count; i++)
                    result.Add(ao[i]);

                Statistics["UpperHalf"] = new QsVector(result.ToArray());
            }
            return (QsVector)Statistics["UpperHalf"];

        }

        public QsScalar Q1() => LowerHalf().Median();
        public QsScalar Q3() => UpperHalf().Median();


        public QsScalar InterquartileRange()
        {
            var aa = Q3().SubtractScalar(Q1());
            return aa;
        }

        public QsScalar Minimum()
        {
            if(!Statistics.ContainsKey("Minimum"))
                Statistics["Minimum"] = AscendedVecor[0];
            return (QsScalar)Statistics["Minimum"];
        }

        public QsScalar Maximum()
        {
            if(!Statistics.ContainsKey("Maximum"))
                Statistics["Maximum"] = AscendedVecor[AscendedVecor.Count - 1];
            return (QsScalar)Statistics["Maximum"];
            
        }

        /// <summary>
        /// Minimum and maximum values of the vector
        /// </summary>
        /// <returns></returns>
        public QsScalar Range()
        {
            if(!Statistics.ContainsKey("Range"))
                Statistics["Range"] = Maximum() - Minimum();
            return (QsScalar)Statistics["Range"];
        }

        public QsFlowingTuple Frequency()
        {

            if (!Statistics.ContainsKey("Frequency"))
            {

                List<(QsValue, int)> values = new List<(QsValue, int)>();


                for (int ix = 0; ix < AscendedVecor.Count; ix++)
                {
                    int ix_freq = 1;

                    bool last = true;
                    for (int iy = ix + 1; iy < AscendedVecor.Count; iy++)
                    {
                        if (AscendedVecor[ix].Equality(AscendedVecor[iy]))
                            ix_freq++;
                        else
                        {

                            values.Add((AscendedVecor[ix], ix_freq));


                            ix = iy - 1;

                            last = false;
                            break;
                        }
                    }

                    if (last)
                    {
                        values.Add((AscendedVecor[ix], ix_freq));
                        break;
                    }
                }

                var vvdodesc = values.OrderByDescending(v => v.Item2).ToArray();

                if (vvdodesc.Length == 0) Statistics["Mode"] = new QsText("No Mode");
                if (vvdodesc.Length == 1) Statistics["Mode"] = vvdodesc[0].Item1;

                if (vvdodesc.Length == 2 && vvdodesc[0].Item2 > vvdodesc[1].Item2)
                    Statistics["Mode"] = vvdodesc[0].Item1;
                else
                    Statistics["Mode"] = new QsText("No Mode");

                if (vvdodesc.Length > 2)
                {
                    if (vvdodesc[0].Item2 == vvdodesc[1].Item2 && vvdodesc[1].Item2 == vvdodesc[2].Item2) Statistics["Mode"] = new QsText("No Mode");
                    else if (vvdodesc[0].Item2 == vvdodesc[1].Item2) Statistics["Mode"] = new QsText($"Bi-Modal ({vvdodesc[0].Item1.ToShortString()}, {vvdodesc[1].Item1.ToShortString()})");
                    else if (vvdodesc[0].Item2 > vvdodesc[1].Item2) Statistics["Mode"] = vvdodesc[0].Item1;
                    else
                        Statistics["Mode"] = new QsText("No Mode");


                }

                var vvd = vvdodesc.Select(x => new QsTupleValue(x.Item1.ToValueString(), x.Item2.ToScalarValue())).ToArray();

                Statistics["Frequency"] = new QsFlowingTuple(vvd);
            }

            return (QsFlowingTuple)Statistics["Frequency"];
        }


        [QsValueProperty("Mode")]
        public QsValue Mode()
        {
            if (!Statistics.ContainsKey("Mode"))
                Frequency();

            return Statistics["Mode"];

        }

        [QsValueProperty("VarianceVector")]
        public QsVector VarianceVector()
        {
            var mn = Mean();

            return this.SubtractScalar(mn);
        }

        [QsValueProperty("Variance")]
        public QsScalar Variance()
        {
            if (!Statistics.ContainsKey("Variance"))
            {
                var vv = VarianceVector();
                var vvsq = vv.PowerScalar(2.ToQuantity().ToScalar());
                var vvsqsum = vvsq.Sum();
                Statistics["Variance"] = vvsqsum.DivideScalar(Count.ToQuantity().ToScalar());
            }
            return (QsScalar)Statistics["Variance"];
        }

        [QsValueProperty("SampleVariance")]
        public QsScalar SampleVariance()
        {
            if (!Statistics.ContainsKey("SampleVariance"))
            {
                var vv = VarianceVector();
                var vvsq = vv.PowerScalar(2.ToQuantity().ToScalar());
                var vvsqsum = vvsq.Sum();
                Statistics["SampleVariance"] = vvsqsum.DivideScalar((Count - 1).ToQuantity().ToScalar());

            }
            return (QsScalar)Statistics["SampleVariance"];
        }

        [QsValueProperty("SampleStandardDeviation", "StDev")]
        public QsScalar SampleStandardDeviation()
        {
            if (!Statistics.ContainsKey("SampleStandardDeviation"))
            {
                var stdev = SampleVariance().PowerScalar((0.5).ToQuantity().ToScalar());
                Statistics["SampleStandardDeviation"] = stdev;
            }
            return (QsScalar)Statistics["SampleStandardDeviation"];
        }


        [QsValueProperty("StandardDeviation", "StD")]
        public QsScalar StandardDeviation()
        {
            if (!Statistics.ContainsKey("StandardDeviation"))
            {
                var stdev = Variance().PowerScalar((0.5).ToQuantity().ToScalar());
                Statistics["StandardDeviation"] = stdev;
            }
            return (QsScalar)Statistics["StandardDeviation"];
        }

        [QsValueProperty("Statistics", "Stats")]
        public QsFlowingTuple CurrentStatistics()
        {
            StandardDeviation(); // just calculate the thing if it is not calculated.

            return QsFlowingTuple.FromDictionary(Statistics);
        }

    }
}