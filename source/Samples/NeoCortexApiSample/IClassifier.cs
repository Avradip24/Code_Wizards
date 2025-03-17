using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoCortexApiSample
{
    /// <summary>
    /// Generic interface for a classifier that learns and predicts inputs.
    /// </summary>
    /// <typeparam name="TIN">The type of input data used for learning and prediction.</typeparam>
    /// <typeparam name="TOUT">The type of predicted output.</typeparam>
    public interface IClassifier<TIN, TOUT>
    {
        /// Trains the classifier by associating an input key with active cells.
        void Learn(string key, TIN[] activeCells);
        /// Retrieves predicted input values based on the given predictive cells.
        List<PredictedResult<TOUT>> GetPredictedInputValues(TIN[] predictiveCells, int k);
        // Clears the internal state of the classifier, removing learned data.
        void ClearState();
        // Reconstructs the original input from predicted results.
        int[] ReconstructInput(List<PredictedResult<TOUT>> predictedResults);
    }               
    /// <summary>
    /// Represents the result of a prediction, including the predicted input, similarity score, and SDR.
    /// </summary>
    /// <typeparam name="T">The type of predicted input.</typeparam>
    public class PredictedResult<T>
    {
        // The predicted input value.
        public T PredictedInput { get; set; }
        // The similarity score between the prediction and actual input, typically ranging from 0 to 1.
        public double Similarity { get; set; }
        // The Sparse Distributed Representation (SDR) associated with the prediction.
        public int[] SDR { get; set; } 
    }
}