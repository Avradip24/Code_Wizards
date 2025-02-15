using System;
using System.Collections.Generic;
using System.Linq;
using NeoCortexApi.Entities;
using NeoCortexApi.Classifiers;
using NeoCortexApiSample;

namespace NeoCortexApiSample
{
    public class HtmImageClassifier : IClassifier<Cell, string>
    {
        private readonly HtmClassifier<string, ComputeCycle> _htmClassifier = new HtmClassifier<string, ComputeCycle>();

        public void Learn(string key, Cell[] activeCells)
        {
            _htmClassifier.Learn(key, activeCells);
        }

        public List<PredictedResult<string>> GetPredictedInputValues(Cell[] predictiveCells, int k)
        {
            var predictions = _htmClassifier.GetPredictedInputValues(predictiveCells, (short)k);
            return predictions.Select(p => new PredictedResult<string>
            {
                PredictedInput = p.PredictedInput,
                Similarity = p.Similarity,
            }).ToList();
        }

        public void ClearState()
        {
            _htmClassifier.ClearState();
        }

        public int[] ReconstructInput(List<PredictedResult<string>> predictedResults)
        {
            return predictedResults.First().SDR;  // Reconstruct input from SDR
        }
    }
}