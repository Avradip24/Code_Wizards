using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoCortexApiSample
{
    public interface IClassifier<TIN, TOUT>
    {
        void Learn(string key, TIN[] activeCells);
        List<PredictedResult<TOUT>> GetPredictedInputValues(TIN[] predictiveCells, int k);
        void ClearState();
        int[] ReconstructInput(List<PredictedResult<TOUT>> predictedResults);
    }

    public class PredictedResult<T>
    {
        public T PredictedInput { get; set; }
        public double Similarity { get; set; }
        public int[] SDR { get; set; }
    }
}