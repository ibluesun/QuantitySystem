using System;
using System.Collections.Generic;
using Qs.Types.Attributes;


namespace Qs.Types
{
    public partial class QsMatrix : QsValue
    {

        private Dictionary<string, QsValue> Statistics = new Dictionary<string, QsValue>(StringComparer.OrdinalIgnoreCase);


        protected override void OnClearCache()
        {
            


            Statistics.Clear();
        }


        [QsValueProperty("CoVariance")]
        public QsScalar CoVariance()
        {

            if (!Statistics.ContainsKey("CoVariance"))
            {
                var vv1 = this.GetColumnVector(0).VarianceVector();
                var vv2 = this.GetColumnVector(1).VarianceVector();

                var vvm = vv1.MultiplyVector(vv2);

                // sum it 
                var vmmsum = vvm.Sum();

                Statistics["CoVariance"] = vmmsum.DivideScalar(Rows.Count.ToQuantity().ToScalar());
            }

            return (QsScalar)Statistics["CoVariance"];

        }

        [QsValueProperty("SampleCoVariance")]
        public QsScalar SampleCoVariance()
        {
            if (!Statistics.ContainsKey("SampleCoVariance"))
            {
                var vv1 = this.GetColumnVector(0).VarianceVector();
                var vv2 = this.GetColumnVector(1).VarianceVector();

                var vvm = vv1.MultiplyVector(vv2);

                // sum it 
                var vmmsum = vvm.Sum();

                Statistics["SampleCoVariance"] = vmmsum.DivideScalar((Rows.Count - 1).ToQuantity().ToScalar());

            }
            return (QsScalar)Statistics["SampleCoVariance"];
        }

        [QsValueProperty("Correlation")]
        public QsScalar Correlation()
        {
            if (!Statistics.ContainsKey("Correlation"))
            {
                var xstd = this.GetColumnVector(0).StandardDeviation();
                var ystd = this.GetColumnVector(1).StandardDeviation();

                var xystd = xstd.MultiplyScalar(ystd);

                var covar = CoVariance();

                Statistics["Correlation"] = covar.DivideScalar(xystd);

            }
            return (QsScalar)Statistics["Correlation"];
        }


        [QsValueProperty("SampleCorrelation")]
        public QsScalar SampleCorrelation()
        {
            if (!Statistics.ContainsKey("SampleCorrelation"))
            {
                var xstd = this.GetColumnVector(0).SampleStandardDeviation();
                var ystd = this.GetColumnVector(1).SampleStandardDeviation();

                var xystd = xstd.MultiplyScalar(ystd);

                var covar = SampleCoVariance();

                Statistics["SampleCorrelation"] = covar.DivideScalar(xystd);

            }
            return (QsScalar)Statistics["SampleCorrelation"];
        }

    }
}