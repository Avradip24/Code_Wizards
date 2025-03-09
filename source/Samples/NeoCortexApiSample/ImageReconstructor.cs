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
using NeoCortex;

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

        public double ReconstructAndSave(SpatialPooler sp, Cell[] predictedCells, string outputFolder, string fileName, int[] inputVector, string binarizedImage)
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
            Debug.WriteLine($"Reconstructed Image Saved as  {recontructedImageName}");
            int[] reconstructedVectorHTM = NeoCortexUtils.ReadCsvIntegers(reconstructedTxtPath).ToArray();
            string binarizedFolder = ".\\BinarizedImages";
            string binarizedImageName = $"{binarizedImage}Training_Binarized.txt";
            string binarizedImagePath = Path.Combine(binarizedFolder, binarizedImageName);
            // Read Binarized and Encoded input CSV file into an array
            int[] binarizedInputVector = NeoCortexUtils.ReadCsvIntegers(binarizedImagePath).ToArray();
            // *Calculate Similarity*
            var similarity = MathHelpers.JaccardSimilarityofBinaryArrays(binarizedInputVector, reconstructedVectorHTM);
            return similarity;
        }
        public void CompareReconstructedImages(string knnFolder, string htmFolder)
        {
            // Get all files from both directories
            var knnFiles = Directory.GetFiles(knnFolder, "*.txt")
                                    .Select(f => Path.GetFileNameWithoutExtension(f).Replace("KNN_reconstructed_", "")).ToList();

            var htmFiles = Directory.GetFiles(htmFolder, "*.txt")
                                    .Select(f => Path.GetFileNameWithoutExtension(f).Replace("HTM_reconstructed_", "")).ToList();

           

            // Find common images in both folders
            var commonImages = knnFiles.Intersect(htmFiles).ToList();

            if (!commonImages.Any())
            {
                Debug.WriteLine("Both the classifiers predicted different images, hence ignoring comparing reconstructed images");
                return;
            }

            foreach (var imageName in commonImages)
            {
                string knnImagePath = Path.Combine(knnFolder, $"KNN_reconstructed_{imageName}.txt");
                string htmImagePath = Path.Combine(htmFolder, $"HTM_reconstructed_{imageName}.txt");

                // Read binary vectors from files
                int[] knnVector = NeoCortexUtils.ReadCsvIntegers(knnImagePath).ToArray();
                int[] htmVector = NeoCortexUtils.ReadCsvIntegers(htmImagePath).ToArray();

                // Compute Jaccard Similarity
                double similarity = MathHelpers.JaccardSimilarityofBinaryArrays(knnVector, htmVector);

                // Print similarity to debug window
                Debug.WriteLine($"Similarity between {imageName} (KNN vs HTM): {similarity:F2}");
            }
        }
    }
}