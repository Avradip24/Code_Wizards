using NeoCortex;
using NeoCortexApi.Entities;
using NeoCortexApi.Utility;
using NeoCortexApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GemBox.Spreadsheet.Drawing;
using NeoCortexApi.Classifiers;

namespace NeoCortexApiSample
{
    internal class ImageBinarizerSpatialPattern
    {
        public string inputPrefix { get; private set; }

        /// <summary>
        /// Implements an experiment that demonstrates how to learn spatial patterns.
        /// SP will learn every presented Image input in multiple iterations.
        /// </summary>
        public void Run()
        {
            Console.WriteLine($"Hello NeocortexApi! Experiment {nameof(ImageBinarizerSpatialPattern)}");

            double minOctOverlapCycles = 1.0;
            double maxBoost = 5.0;
            // We will build a slice of the cortex with the given number of mini-columns
            int numColumns = 64 * 64;
            // The Size of the Image Height and width is 28 pixel
            int imageSize = 25;
            var colDims = new int[] { 64, 64 };

            // This is a set of configuration parameters used in the experiment.
            HtmConfig cfg = new HtmConfig(new int[] { imageSize, imageSize }, new int[] { numColumns })
            {
                CellsPerColumn = 10,
                InputDimensions = new int[] { imageSize, imageSize },
                NumInputs = imageSize * imageSize,
                ColumnDimensions = colDims,
                MaxBoost = maxBoost,
                DutyCyclePeriod = 100,
                MinPctOverlapDutyCycles = minOctOverlapCycles,
                GlobalInhibition = false,
                NumActiveColumnsPerInhArea = 0.02 * numColumns,
                PotentialRadius = (int)(0.15 * imageSize * imageSize),
                LocalAreaDensity = -1,
                ActivationThreshold = 10,
                MaxSynapsesPerSegment = (int)(0.01 * numColumns),
                Random = new ThreadSafeRandom(42),
                StimulusThreshold = 10,
            };

            //Runnig the Experiment
            var sp = RunExperiment(cfg, inputPrefix);
            //Runing the Reconstruction Method Experiment
            //RunRustructuringExperiment(sp);

        }

        /// <summary>
        /// Implements the experiment.
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="inputPrefix"> The name of the images</param>
        /// <returns>The trained bersion of the SP.</returns>
        private SpatialPooler RunExperiment(HtmConfig cfg, string inputPrefix)
        {
            var mem = new Connections(cfg);
            bool isInStableState = false;

            int numColumns = 64 * 64;
            string trainingFolder = "Sample\\TestFiles";
            var trainingImages = Directory.EnumerateFiles(trainingFolder)
                .Where(file => file.StartsWith($"{trainingFolder}\\{inputPrefix}") &&
                              (file.EndsWith(".jpeg") || file.EndsWith(".jpg") || file.EndsWith(".png")))
                .ToArray();

            int imageSize = 25;
            string outputFolder = ".\\BinarizedImages";
            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }
            Directory.CreateDirectory(outputFolder);

            var binarizedImagePaths = new List<string>();
            foreach (var image in trainingImages)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(image) + "_Binarized";
                string outputPath = Path.Combine(outputFolder, outputFileName);

                string binarizedImagePath = NeoCortexUtils.BinarizeImage($"{image}", imageSize, outputPath);
                binarizedImagePaths.Add(binarizedImagePath);
            }
            Debug.WriteLine("All images are binarized");

            HomeostaticPlasticityController hpa = new HomeostaticPlasticityController(mem, trainingImages.Length * 50, (isStable, numPatterns, actColAvg, seenInputs) =>
            {
                if (isStable)
                {
                    isInStableState = true;
                    Debug.WriteLine($"Entered STABLE state: Patterns: {numPatterns}, Inputs: {seenInputs}, iteration: {seenInputs / numPatterns}");
                }
                else
                {
                    isInStableState = false;
                    Debug.WriteLine("INSTABLE STATE");
                }
            }, requiredSimilarityThreshold: 0.975);

            SpatialPooler sp = new SpatialPooler(hpa);
            sp.Init(mem, new DistributedMemory() { ColumnDictionary = new InMemoryDistributedDictionary<int, NeoCortexApi.Entities.Column>(1) });

            int[] activeArray = new int[numColumns];
            int numStableCycles = 0;
            bool storedStableCycleSDRs = false;
            int maxCycles = 200;
            int currentCycle = 0;

            string sdrOutputFolder = ".\\SDROutput";
            if (Directory.Exists(sdrOutputFolder))
            {
                Directory.Delete(sdrOutputFolder, true);
            }
            Directory.CreateDirectory(sdrOutputFolder);
            string sdrFilePath = Path.Combine(sdrOutputFolder, "SDRs.txt");

            var knnClassifier = new KNeighborsClassifier<string, string>();
            var labeledSDRs = new Dictionary<string, List<int[]>>();

            using (StreamWriter writer = new StreamWriter(sdrFilePath, append: false))
            {
                Console.SetOut(writer);

                while (currentCycle < maxCycles)
                {
                    foreach (var binarizedImagePath in binarizedImagePaths)
                    {
                        int[] inputVector = NeoCortexUtils.ReadCsvIntegers(binarizedImagePath).ToArray();

                        sp.compute(inputVector, activeArray, true);
                        var activeCols = ArrayUtils.IndexWhere(activeArray, el => el == 1);

                        string image = Path.GetFileNameWithoutExtension(binarizedImagePath);

                        Debug.WriteLine($"Cycle: {currentCycle} - Image-Input: {image}");
                        Debug.WriteLine($"INPUT :{Helpers.StringifyVector(inputVector)}");
                        Debug.WriteLine($"SDR: {Helpers.StringifyVector(activeCols)}\n");

                        if (isInStableState && !storedStableCycleSDRs)
                        {
                            string label = image; // Label the SDRs with the image name for training
                            knnClassifier.Learn(label, activeCols.Select(idx => new Cell { Index = idx }).ToArray());
                            if (!labeledSDRs.ContainsKey(label))
                            {
                                labeledSDRs[label] = new List<int[]>();
                            }
                            labeledSDRs[label].Add(activeCols);

                            Console.WriteLine($"Stable Cycle: {currentCycle} - Image-Input: {image}");
                            Console.WriteLine($"SDR: {Helpers.StringifyVector(activeCols)}\n");
                            writer.Flush();
                        }

                        if (isInStableState)
                        {
                            foreach (var testSDR in labeledSDRs)
                            {
                                var predictions = knnClassifier.GetPredictedInputValues(activeCols.Select(idx => new Cell { Index = idx }).ToArray());
                                Debug.WriteLine($"Predictions for {image}: {string.Join(", ", predictions.Select(p => p.PredictedInput))}");
                            }
                        }
                    }

                    if (isInStableState && !storedStableCycleSDRs)
                    {
                        Debug.WriteLine($"Storing SDRs for the first stable cycle: {currentCycle}");
                        storedStableCycleSDRs = true;
                    }

                    Debug.WriteLine($"Completed Cycle * {currentCycle} * Stability: {isInStableState}\n");
                    currentCycle++;
                    if (currentCycle >= maxCycles)
                        break;

                    if (isInStableState)
                    {
                        numStableCycles++;
                    }

                    if (numStableCycles > 5)
                        break;
                }
            }

            Debug.WriteLine("It has reached the stable stage");
            return sp;
        }

        /// <summary>
        /// Runs the restructuring experiment using the provided spatial pooler. 
        /// This method iterates through a set of training images, computes spatial pooling, 
        /// reconstructs permanence values, and generates heatmaps and similarity graphs based on the results.
        /// </summary>
        /// <param name="sp">The spatial pooler to use for the experiment.</param>
        private void RunRustructuringExperiment(SpatialPooler sp)
        {
            // Path to the folder containing training images
            string trainingFolder = "Sample\\TestFiles";
            // Get all image files matching the specified prefix
            var trainingImages = Directory.GetFiles(trainingFolder, $"{inputPrefix}*.png");
            // Size of the images
            int imgSize = 25;
            // Name for the test image
            string testName = "test_image";
            // Array to hold active columns
            int[] activeArray = new int[64 * 64];
            // List to store heatmap data
            List<List<double>> heatmapData = new List<List<double>>();
            // Initialize a list to get normalized permanence values.
            List<int[]> BinarizedencodedInputs = new List<int[]>();
            // List to store normalized permanence values
            List<int[]> normalizedPermanence = new List<int[]>();
            // List to store similarity values
            List<double[]> similarityList = new List<double[]>();
            foreach (var Image in trainingImages)
            {
                string inputBinaryImageFile = NeoCortexUtils.BinarizeImage($"{Image}", imgSize, testName);

                // Read input csv file into array
                int[] inputVector = NeoCortexUtils.ReadCsvIntegers(inputBinaryImageFile).ToArray();

                // Initialize arrays and lists for computations
                int[] oldArray = new int[activeArray.Length];
                List<double[,]> overlapArrays = new List<double[,]>();
                List<double[,]> bostArrays = new List<double[,]>();

                // Compute spatial pooling on the input vector
                sp.compute(inputVector, activeArray, true);
                var activeCols = ArrayUtils.IndexWhere(activeArray, (el) => el == 1);

                Dictionary<int, double> reconstructedPermanence = sp.Reconstruct(activeCols);

                int maxInput = inputVector.Length;

                // Create a new dictionary to store extended probabilities
                Dictionary<int, double> allPermanenceDictionary = new Dictionary<int, double>();
                // Iterate through all possible inputs using a foreach loop
                foreach (var kvp in reconstructedPermanence)
                {
                    int inputIndex = kvp.Key;
                    double probability = kvp.Value;

                    // Use the existing probability
                    allPermanenceDictionary[inputIndex] = probability;
                }

                //Assinginig the inactive columns Permanence 0
                for (int inputIndex = 0; inputIndex < maxInput; inputIndex++)
                {
                    if (!reconstructedPermanence.ContainsKey(inputIndex))
                    {
                        // Key doesn't exist, set the probability to 0
                        allPermanenceDictionary[inputIndex] = 0.0;
                    }
                }

                // Sort the dictionary by keys
                var sortedAllPermanenceDictionary = allPermanenceDictionary.OrderBy(kvp => kvp.Key);
                // Convert the sorted dictionary of allpermanences to a list
                List<double> permanenceValuesList = sortedAllPermanenceDictionary.Select(kvp => kvp.Value).ToList();

                //Collecting Heatmap Data for Visualization
                heatmapData.Add(permanenceValuesList);

                //Collecting Encoded Data for Visualization
                BinarizedencodedInputs.Add(inputVector);

                //Normalizing Permanence Threshold
                var ThresholdValue = 30.5;

                // Normalize permanences (0 and 1) based on the threshold value and convert them to a list of integers.
                List<int> normalizePermanenceList = Helpers.ThresholdingProbabilities(permanenceValuesList, ThresholdValue);

                //Collecting Normalized Permanence List for Visualizing
                normalizedPermanence.Add(normalizePermanenceList.ToArray());

                //Calculating Similarity with encoded Inputs and Reconstructed Inputs
                var similarity = MathHelpers.JaccardSimilarityofBinaryArrays(inputVector, normalizePermanenceList.ToArray());

                double[] similarityArray = new double[] { similarity };

                //Collecting Similarity Data for visualizing
                similarityList.Add(similarityArray);
            }
            // Generate the 1D heatmaps using the heatmapData list
            Generate1DHeatmaps(heatmapData, BinarizedencodedInputs, normalizedPermanence);
            // Generate the Similarity graph using the Similarity list
            DrawSimilarityPlots(similarityList);
        }

        /// <summary>
        /// Generates 1D heatmaps based on the provided heatmap data and normalized permanence values.
        /// </summary>
        /// <param name="heatmapData">List of lists containing heatmap data.</param>
        /// <param name="normalizedPermanence">List of arrays containing normalized permanence values.</param>
        private void Generate1DHeatmaps(List<List<double>> heatmapData, List<int[]> normalizedPermanence, List<int[]> BinarizedencodedInputs)
        {
            int i = 1;

            foreach (var values in heatmapData)
            {
                // Define the folder path based on your requirements
                string folderPath = Path.Combine(Environment.CurrentDirectory, "1DHeatMap_Image_Inputs");

                // Create the folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // Define the file path with the folder path
                string filePath = Path.Combine(folderPath, $"heatmap_{i}.png");
                Debug.WriteLine($"FilePath: {filePath}");

                // Convert the probabilitiesList to a 1D array using ToArray
                double[] array1D = values.ToArray();

                // Call the  Draw1DHeatmap function with the dynamically generated file path along with all necessary Perameters
                NeoCortexUtils.Draw1dHeatmap(new List<double[]>() { array1D }, new List<int[]>() { normalizedPermanence[i - 1] }, new List<int[]>() { BinarizedencodedInputs[i - 1] }, filePath, 784, 15, 30, 15, 5, 30);

                Debug.WriteLine("Heatmap generated and saved successfully.");
                i++;
            }

        }

        // <summary>
        /// Draws a combined similarity plot based on the provided list of arrays containing similarity values.
        /// The combined similarity plot is generated by combining all similarity values from the list of arrays,
        /// creating a single list of similarities, and then drawing the plot.
        /// </summary>
        /// <param name="similaritiesList">List of arrays containing similarity values.</param>
        public static void DrawSimilarityPlots(List<double[]> similaritiesList)
        {
            // Combine all similarities from the list of arrays

            List<double> combinedSimilarities = new List<double>();
            foreach (var similarities in similaritiesList)

            {
                combinedSimilarities.AddRange(similarities);
            }

            // Define the folder path based on the current directory

            string folderPath = Path.Combine(Environment.CurrentDirectory, "SimilarityPlots_Image_Inputs");


            // Create the folder if it doesn't exist

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Define the file name
            string fileName = "combined_similarity_plot_Image_Inputs.png";

            // Define the file path with the folder path and file name

            string filePath = Path.Combine(folderPath, fileName);

            // Draw the combined similarity plot
            NeoCortexUtils.DrawCombinedSimilarityPlot(combinedSimilarities, filePath, 1000, 850);

            Debug.WriteLine($"Combined similarity plot generated and saved successfully.");

        }
    }
}

