using NeoCortexApi.Entities;
using NeoCortexApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoCortexApi.Utility;

namespace NeoCortexApiSample
{
    public class ImageReconstructor
    {
        private int imgWidth;
        private int imgHeight;

        public ImageReconstructor(int imgWidth, int imgHeight)
        {
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
        }

        public double ReconstructAndSave(SpatialPooler sp, Cell[] predictedCells, string outputFolder, string fileName, int[] inputVector)
        {
            var predictedCols = predictedCells.Select(c => c.Index).Distinct().ToArray();
            // Create a new dictionary to store extended probabilities
            Dictionary<int, double> reconstructedPermanence = sp.Reconstruct(predictedCols);
            int maxInput = inputVector.Length;

            // Iterate through all possible inputs and adding them to the dictionary
            Dictionary<int, double> allPermanenceDictionary = reconstructedPermanence.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Assigning the inactive columns Permanence 0
            for (int inputIndex = 0; inputIndex < maxInput; inputIndex++)
            {
                if (!allPermanenceDictionary.ContainsKey(inputIndex))
                {
                    allPermanenceDictionary[inputIndex] = 0.0;
                }
            }

            // Sort the dictionary by keys
            var sortedAllPermanenceDictionary = allPermanenceDictionary.OrderBy(kvp => kvp.Key);
            // Convert the sorted dictionary of all permanences to a list
            List<double> permanenceValuesList = sortedAllPermanenceDictionary.Select(kvp => kvp.Value).ToList();
            // Normalizing Permanence Threshold
            List<int> normalizePermanenceList = Helpers.ThresholdingProbabilities(permanenceValuesList, 40.5);
            // *Calculate Similarity*
            var similarity = MathHelpers.JaccardSimilarityofBinaryArrays(inputVector, normalizePermanenceList.ToArray());
            double[] similarityArray = new double[] { similarity };
            // Define the output text file name
            string reconstructedTxtPath = Path.Combine(outputFolder, fileName);

            // Convert the 1D list into a 2D binary-like structure
            using (StreamWriter writer = new StreamWriter(reconstructedTxtPath))
            {
                for (int i = 0; i < imgHeight; i++)
                {
                    // Extract a row of binary values from the flattened list
                    var row = normalizePermanenceList.Skip(i * imgWidth).Take(imgWidth);
                    // Convert row to a string and write to the file
                    writer.WriteLine(string.Join("", row));
                }
            }

            Debug.WriteLine($"Reconstructed Image Saved: {reconstructedTxtPath}");
            return similarity;
        }
    }
}