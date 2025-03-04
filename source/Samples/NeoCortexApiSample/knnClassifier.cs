using System;
using System.Collections.Generic;
using System.Linq;
using NeoCortexApi.Entities;
using NeoCortexApi.Classifiers;
using NeoCortexApiSample;

namespace NeoCortexApiSample
{
    public class KnnImageClassifier : IClassifier<Cell, string>
    {
        private readonly KNeighborsClassifier<string, ComputeCycle> _knnClassifier = new KNeighborsClassifier<string, ComputeCycle>();

        public void Learn(string key, Cell[] activeCells)
        {
            _knnClassifier.Learn(key, activeCells);
        }

        public List<PredictedResult<string>> GetPredictedInputValues(Cell[] predictiveCells, int k)
        {
            var predictions = _knnClassifier.GetPredictedInputValues(predictiveCells, (short)k);
            return predictions.Select(p => new PredictedResult<string>
            {
                PredictedInput = p.PredictedInput,
                Similarity = p.Similarity,
                SDR = p.SDR,
            }).ToList();
        }

        public void ClearState()
        {
            _knnClassifier.ClearState();
        }

        public int[] ReconstructInput(List<PredictedResult<string>> predictedResults)
        {
            return predictedResults.First().SDR;
        }
    }
}