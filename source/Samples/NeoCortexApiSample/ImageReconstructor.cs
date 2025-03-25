using NeoCortexApi.Entities;
using NeoCortexApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NeoCortexApi.Utility;
using NeoCortex;

namespace NeoCortexApiSample
{
    /// <summary>
    /// The ImageReconstructor class is responsible for reconstructing images from predicted cell activity
    /// using the Spatial Pooler (SP) and evaluating the quality of reconstruction through similarity measures.
    /// </summary>
    public class ImageReconstructor
    {
        /// <summary>
        /// Reconstructs an image from predicted active cells and saves it as a text file.
        /// Also computes the similarity between the reconstructed image and the original binarized image.
        /// </summary>
        /// <param name="sp">The Spatial Pooler used for reconstruction.</param>
        /// <param name="predictedCells">Array of predicted cells.</param>
        /// <param name="outputFolder">The folder where the reconstructed image file will be saved.</param>
        /// <param name="fileName">Name of the output file for the reconstructed image.</param>
        /// <param name="inputVector">Original input vector used for encoding the image.</param>
        /// <param name="binarizedImage">The name of the binarized image used for comparison.</param>
        /// <returns>
        /// A tuple containing:
        /// - <see cref="double"/>: Jaccard similarity between the reconstructed image and the original binarized image.
        /// - <see cref="double"/>: Hamming Distance similarity between the reconstructed image and the original binarized image.
        /// - <see cref="string"/>: The name of the reconstructed image file.
        /// </returns>
        public static (double, double, String) ReconstructAndSave(int imgHeight, int imgWidth, SpatialPooler sp, Cell[] predictedCells, string outputFolder, string fileName, int[] inputVector, string binarizedImage)
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
            string recontructedImage = Path.GetFileNameWithoutExtension(reconstructedTxtPath);
            // Split the filename by spaces
            string[] imageName = recontructedImage.Split(' ');
            // Extract the last two words
            string recontructedImageName = imageName.Length >= 2 ? $"{imageName[^2]} {imageName[^1]}" : recontructedImage;
            Debug.WriteLine($"Reconstructed Image Saved as {recontructedImageName}");
            int[] reconstructedVectorHTM = NeoCortexUtils.ReadCsvIntegers(reconstructedTxtPath).ToArray();
            string binarizedFolder = ".\\BinarizedImages";
            string binarizedImageName = $"{binarizedImage}Training_Binarized.txt";
            string binarizedImagePath = Path.Combine(binarizedFolder, binarizedImageName);
            // Read Binarized and Encoded input CSV file into an array
            int[] binarizedInputVector = NeoCortexUtils.ReadCsvIntegers(binarizedImagePath).ToArray();
            // Calculate Similarity
            var jacSimilarity = MathHelpers.JaccardSimilarityofBinaryArrays(binarizedInputVector, reconstructedVectorHTM);
            var hamSimilarity = MathHelpers.GetHammingDistance(binarizedInputVector, reconstructedVectorHTM);
            return (jacSimilarity, hamSimilarity, reconstructedTxtPath);
        }

        /// <summary>
        /// Compares two reconstructed images (from KNN and HTM classifiers) by computing their Jaccard similarity
        /// if the predictions are different. If the classifiers predict the same image, similarity calculation is skipped.
        /// </summary>
        /// <param name="knnFilePath">File path of the reconstructed image from the KNN classifier.</param>
        /// <param name="htmFilePath">File path of the reconstructed image from the HTM classifier.</param>
        public static void CompareReconstructedImages(string htmFilePath, string knnFilePath, Action<string> logger = null)
        {
            // Extract image names from file names
            string knnImageName = Path.GetFileNameWithoutExtension(knnFilePath).Replace("KNN_reconstructed_", "");
            string htmImageName = Path.GetFileNameWithoutExtension(htmFilePath).Replace("HTM_reconstructed_", "");

            // Read binary vectors from files
            int[] knnVector = NeoCortexUtils.ReadCsvIntegers(knnFilePath).ToArray();
            int[] htmVector = NeoCortexUtils.ReadCsvIntegers(htmFilePath).ToArray();

            // Compute Jaccard Similarity
            double similarity = MathHelpers.JaccardSimilarityofBinaryArrays(knnVector, htmVector);

            // Create the log message
            string message = $"Similarity between HTM ({htmImageName}) and KNN ({knnImageName}): {similarity:F2}";

            // Print to Debug window + Pass to logger if provided
            Debug.WriteLine(message);
            // Call the mock logger if it's passed in
            logger?.Invoke(message); 
        }
    }
}