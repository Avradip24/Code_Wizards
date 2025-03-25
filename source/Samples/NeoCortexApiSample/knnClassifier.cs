using System.Collections.Generic;
using System.Linq;
using NeoCortexApi.Entities;
using NeoCortexApi.Classifiers;

namespace NeoCortexApiSample
{
    /// <summary>
    /// Implements a k-Nearest Neighbors (k-NN) classifier for image recognition using the NeoCortex HTM model.
    /// </summary>
     public class KnnImageClassifier : IClassifier<Cell, string>
    {
        // k-NN classifier instance for mapping ComputeCycle objects to string labels
        private readonly KNeighborsClassifier<string, ComputeCycle> KnnClassifier = new KNeighborsClassifier<string, ComputeCycle>();

        /// <summary>
        /// Trains the classifier by associating a label (key) with active cells.
        /// </summary>
        /// <param name="key">The label associated with the active cells.</param>
        /// <param name="activeCells">Array of active cells representing the learned pattern.</param>
        public void Learn(string key, Cell[] activeCells)
        {
            KnnClassifier.Learn(key, activeCells);
        }

        /// <summary>
        /// Retrieves predicted labels based on predictive cells using k-NN.
        /// </summary>
        /// <param name="predictiveCells">Array of predictive cells used for classification.</param>
        /// <param name="k">Number of nearest neighbors to consider.</param>
        /// <returns>List of predicted results with similarity scores and SDR representations.</returns>
        public List<PredictedResult<string>> GetPredictedInputValues(Cell[] predictiveCells, int k)
        {
            var predictions = KnnClassifier.GetPredictedInputValues(predictiveCells, (short)k);
            // Convert and return predictions as a list of PredictedResult<string> objects
            return predictions.Select(p => new PredictedResult<string>
            {
                // The predicted label
                PredictedInput = p.PredictedInput, 
                // Similarity score (e.g., Jaccard similarity)
                Similarity = p.Similarity, 
                // Sparse Distributed Representation of the input
                SDR = p.SDR, 
            }).ToList();
        }

        // Clears the stored learned data in the classifier
        public void ClearState()
        {
            KnnClassifier.ClearState();
        }

        /// <summary>
        /// Reconstructs the most likely input SDR (Sparse Distributed Representation) from predicted results.
        /// </summary>
        /// <param name="predictedResults">List of predicted results from the classifier.</param>
        /// <returns>The SDR representation of the most similar predicted result.</returns>
        public int[] ReconstructInput(List<PredictedResult<string>> predictedResults)
        {
            // Return the SDR of the first (most similar) predicted result
            return predictedResults.First().SDR; 
        }
    }
}