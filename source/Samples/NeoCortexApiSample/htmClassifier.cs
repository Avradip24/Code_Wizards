using System.Collections.Generic;
using System.Linq;
using NeoCortexApi.Entities;
using NeoCortexApi.Classifiers;

namespace NeoCortexApiSample
{
    /// <summary>
    /// Implements a HTM classifier for image recognition using the NeoCortex HTM model.
    /// </summary>
    public class HtmImageClassifier : IClassifier<Cell, string>
    {
        // Instance of HTM Classifier that maps ComputeCycle objects to string labels
        private readonly HtmClassifier<string, ComputeCycle> HtmClassifier = new HtmClassifier<string, ComputeCycle>();

        /// <summary>
        /// Trains the HTM classifier by associating a given label (key) with active cells.
        /// </summary>
        /// <param name="key">The label associated with the input SDR.</param>
        /// <param name="activeCells">Array of active cells representing the SDR.</param>

        public void Learn(string key, Cell[] activeCells)
        {
            HtmClassifier.Learn(key, activeCells);
        }

        /// <summary>
        /// Retrieves the predicted labels for a given set of predictive cells.
        /// </summary>
        /// <param name="predictiveCells">The predictive cells (SDR) used for classification.</param>
        /// <param name="k">Number of top predictions to retrieve.</param>
        /// <returns>List of predicted labels along with their similarity scores.</returns>
        public List<PredictedResult<string>> GetPredictedInputValues(Cell[] predictiveCells, int k)
        {
            // Retrieve predictions from the HTM classifier using the top-k results
            var predictions = HtmClassifier.GetPredictedInputValues(predictiveCells, (short)k);
            // Convert predictions into a structured format
            return predictions.Select(p => new PredictedResult<string>
            {
                // The predicted label
                PredictedInput = p.PredictedInput, 
                // Similarity score (e.g., Jaccard similarity)
                Similarity = p.Similarity, 
                // Sparse Distributed Representation of the predicted input
                SDR = p.SDR 
            }).ToList();
        }

        /// <summary>
        /// Clears the classifier's stored learning data, resetting its state.
        /// </summary>
        public void ClearState()
        {
            HtmClassifier.ClearState();
        }

        /// <summary>
        /// Reconstructs the most likely input SDR (Sparse Distributed Representation) from predicted results.
        /// </summary>
        /// <param name="predictedResults">List of predicted results from the classifier.</param>
        /// <returns>The SDR of the most similar predicted input.</returns>
        public int[] ReconstructInput(List<PredictedResult<string>> predictedResults)
        {
            // Reconstruct input from SDR
            return predictedResults.First().SDR;  
        }
    }
}