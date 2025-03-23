using NeoCortex;
using NeoCortexApi.Entities;
using NeoCortexApi.Utility;
using NeoCortexApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NeoCortexApi.Classifiers;
using Daenet.ImageBinarizerLib.Entities;
using Daenet.ImageBinarizerLib;
using NeoCortexEntities.NeuroVisualizer;
using Newtonsoft.Json.Linq;
using GridCell.js;

namespace NeoCortexApiSample
{
    public class ImageBinarizerSpatialPattern
    {
        //Accessing the Image Folder form the Cureent Directory
        string trainingFolder = "Sample\\TestFiles";
        // Classifier instance for HTM 
        private IClassifier<Cell, string> htmClassifier;
        // Classifier instance for KNN 
        private IClassifier<Cell, string> knnClassifier;

        /// <summary>
        /// Initializes a new instance of the class.
        /// Sets up the classifiers for HTM and KNN models.
        /// </summary>
        public ImageBinarizerSpatialPattern()
        {
            // Initializing Classifiers for HTM and KNN
            htmClassifier = new HtmImageClassifier();
            knnClassifier = new KnnImageClassifier();

        }

        /// <summary>
        /// Represents the result of binarizing images, including mappings between actual images
        /// and their corresponding binarized representations.
        /// </summary>
        public class BinarizedImagesResult
        {
            public string[] ActualImages { get; set; }
            public Dictionary<string, Cell[]> TrainingImagesSDRs { get; set; }
            public Dictionary<string, string> BinarizedTrainingToActualMap { get; set; }
            public Dictionary<string, string> BinarizedTestToActualMap { get; set; }
            public List<string> BinarizedTrainingImagePaths { get; set; }
            public List<string> BinarizedTestingImagePaths { get; set; }
        }

        /// <summary>
        /// Implements an experiment that demonstrates how to learn spatial patterns.
        /// SP will learn every presented Image input in multiple iterations.
        /// </summary>
        public void Run()
        {
            double minOctOverlapCycles = 1.0;
            double maxBoost = 5.0;
            // We will build a slice of the cortex with the given number of mini-columns
            int numColumns = 64 * 64;
            // The Size of the Image Height and width is 64 pixel
            int imgHeight = 64;
            int imgWidth = 64;
            var colDims = new int[] { 64, 64 };

            // This is a set of configuration parameters used in the experiment.
            HtmConfig cfg = new HtmConfig(new int[] { imgHeight, imgWidth }, new int[] { numColumns })
            {
                CellsPerColumn = 10,
                InputDimensions = new int[] { imgHeight, imgWidth },
                NumInputs = imgHeight * imgWidth,
                ColumnDimensions = colDims,
                MaxBoost = maxBoost,
                DutyCyclePeriod = 100,
                MinPctOverlapDutyCycles = minOctOverlapCycles,
                GlobalInhibition = false,
                NumActiveColumnsPerInhArea = 0.02 * numColumns,
                PotentialRadius = (int)(0.15 * imgHeight * imgWidth),
                LocalAreaDensity = -1,
                ActivationThreshold = 10,
                MaxSynapsesPerSegment = (int)(0.01 * numColumns),
                Random = new ThreadSafeRandom(42),
                StimulusThreshold = 10,
            };
            // Call binarizeImage() to process and return images
            var binarizedResult = binarizeImage(trainingFolder);
            // Pass binarized images to Spatial Pooler
            var sp = RunExperiment(cfg, imgHeight, imgWidth, binarizedResult);
        }

        /// <summary>
        /// Binarizes images from the specified training folder and organizes them into training and testing datasets.
        /// </summary>
        /// <returns>
        /// A <see cref="BinarizedImagesResult"/> containing the binarized images, mappings, and related metadata.
        /// </returns>
        public BinarizedImagesResult binarizeImage(string trainingFolder)
        {
            //Accessing the Image Folder form the Cureent Directory Folder
            var actualImages = Directory.EnumerateFiles(trainingFolder).Where(file => file.StartsWith($"{trainingFolder}") &&
            (file.EndsWith(".jpeg") || file.EndsWith(".jpg") || file.EndsWith(".png"))).ToArray();

            // Shuffle to ensure randomness
            Random rnd = new Random();
            actualImages = actualImages.OrderBy(x => rnd.Next()).ToArray();

            // Define split ratio (e.g., 80% training, 20% testing)
            int trainSize = (int)(actualImages.Length * 0.8);
            var trainingImages = actualImages.Take(trainSize).ToArray();
            var testingImages = actualImages.Skip(trainSize).ToArray();

            //Image Dimensions
            int imgHeight = 64;
            int imgWidth = 64;

            // Path to the folder where results will be saved
            String outputFolder = ".\\BinarizedImages";
            // Delete the folder if it exists
            if (Directory.Exists(outputFolder)) Directory.Delete(outputFolder, true);
            // Recreate the folder
            Directory.CreateDirectory(outputFolder);

            // Dictionaries to map actual images to their SDRs for later training phase
            Dictionary<string, Cell[]> trainingImagesSDRs = new Dictionary<string, Cell[]>();
            Dictionary<string, string> binarizedTrainingToActualMap = new Dictionary<string, string>();
            Dictionary<string, string> binarizedTestToActualMap = new Dictionary<string, string>();

            // Taking all the binarized image path in a list
            var binarizedTrainingImagePaths = new List<string>();
            var binarizedTestingImagePaths = new List<string>();

            // Process training images
            foreach (var trainingImage in trainingImages)
            {
                string trainingImageKey = Path.GetFileNameWithoutExtension(trainingImage);

                // Construct the output file name based on the input file name
                string outputFileName = trainingImageKey + "Training_Binarized";
                string outputPath = Path.Combine(outputFolder, outputFileName);

                // Binarizing the images
                string binarizedImagePath = ImageBinarizationUtils.BinarizeImages(imgWidth, imgHeight, outputPath, trainingImage);
                binarizedTrainingImagePaths.Add(binarizedImagePath);
                //Store mapping from binarized to actual image name
                binarizedTrainingToActualMap[outputFileName] = trainingImageKey;
            }
            // Process testing images
            foreach (var testImage in testingImages)
            {
                string testingImageKey = Path.GetFileNameWithoutExtension(testImage);

                // Construct the output file name based on the input file name
                string outputFileName = testingImageKey + "Testing_Binarized";
                string outputPath = Path.Combine(outputFolder, outputFileName);

                // Binarizing the images
                string binarizedImagePath = ImageBinarizationUtils.BinarizeImages(imgWidth, imgHeight, outputPath, testImage);
                binarizedTestingImagePaths.Add(binarizedImagePath);
                //Store mapping from binarized to actual image name
                binarizedTestToActualMap[outputFileName] = testingImageKey;
            }
            Debug.WriteLine("All images are binarized");

            // Return binarization results
            return new BinarizedImagesResult
            {
                ActualImages = actualImages,
                TrainingImagesSDRs = trainingImagesSDRs,
                BinarizedTrainingToActualMap = binarizedTrainingToActualMap,
                BinarizedTestToActualMap = binarizedTestToActualMap,
                BinarizedTrainingImagePaths = binarizedTrainingImagePaths,
                BinarizedTestingImagePaths = binarizedTestingImagePaths
            };
        }

        /// <summary>
        /// Runs an experiment using the Spatial Pooler (SP) algorithm on binarized images.
        /// This process involves training the SP with binarized image representations and 
        /// assessing its stability over multiple cycles.
        /// </summary>
        /// <param name="cfg">The HTM (Hierarchical Temporal Memory) configuration used for initializing the experiment.</param>
        /// <param name="imgHeight">The height of the input images in pixels.</param>
        /// <param name="imgWidth">The width of the input images in pixels.</param>
        /// <param name="binarizedResult">A <see cref="BinarizedImagesResult"/> containing binarized image data, including SDR mappings.</param>
        /// <returns>
        /// The trained <see cref="SpatialPooler"/> instance after processing the binarized image dataset.
        /// </returns>
        public SpatialPooler RunExperiment(HtmConfig cfg, int imgHeight, int imgWidth, BinarizedImagesResult binarizedResult)
        {
            var mem = new Connections(cfg);
            bool isInStableState = false;
            int numColumns = 64 * 64;

            HomeostaticPlasticityController hpa = new HomeostaticPlasticityController(mem, binarizedResult.ActualImages.Length * 50, (isStable, numPatterns, actColAvg, seenInputs) =>
            {
                // Event should only be fired when entering the stable state.
                if (isStable)
                {
                    isInStableState = true;
                    Debug.WriteLine($"Entered STABLE state: Patterns: {numPatterns}, Inputs: {seenInputs}, iteration: {seenInputs / numPatterns}");
                }
                else
                {
                    isInStableState = false;
                    Debug.WriteLine($"INSTABLE STATE");
                }
                // Ideal SP should never enter unstable state after stable state.
                Debug.WriteLine($"Entered STABLE state: Patterns: {numPatterns}, Inputs: {seenInputs}, iteration: {seenInputs / numPatterns}");
            }, requiredSimilarityThreshold: 0.975);

            // It creates the instance of Spatial Pooler.
            SpatialPooler sp = new SpatialPooler(hpa);

            //Initializing the Spatial Pooler Algorithm
            sp.Init(mem, new DistributedMemory() { ColumnDictionary = new InMemoryDistributedDictionary<int, NeoCortexApi.Entities.Column>(1) });

            int[] activeArray = new int[numColumns];

            int numStableCycles = 0;
            int maxCycles = 500;
            int currentCycle = 0;

            // SPATIAL POOLER PHASE
            Stopwatch stopwatchSp = Stopwatch.StartNew();
            while (currentCycle < maxCycles)
            {
                foreach (var binarizedImagePath in binarizedResult.BinarizedTrainingImagePaths)
                {
                    // Read Binarized and Encoded input CSV file into an array
                    int[] inputVector = NeoCortexUtils.ReadCsvIntegers(binarizedImagePath).ToArray();

                    // Compute the active columns for the input vector
                    sp.compute(inputVector, activeArray, true);

                    // Extract active column indices and convert them into SDR cells
                    var activeCols = ArrayUtils.IndexWhere(activeArray, (el) => el == 1);
                    var cells = activeCols.Select(index => new Cell { Index = index }).ToArray();

                    string binarizedKey = Path.GetFileNameWithoutExtension(binarizedImagePath);

                    // Store SDR representation mapped to the actual image for later training
                    string trainingImageKey = binarizedResult.BinarizedTrainingToActualMap[binarizedKey];
                    binarizedResult.TrainingImagesSDRs[trainingImageKey] = cells;

                    Debug.WriteLine($"Cycle: {currentCycle} - Image-Input: {trainingImageKey}");
                    Debug.WriteLine($"INPUT :{Helpers.StringifyVector(inputVector)}");
                    Debug.WriteLine($"SDR: {Helpers.StringifyVector(activeCols)}\n");

                }

                Debug.WriteLine($"Completed Cycle * {currentCycle} * Stability: {isInStableState}\n");
                currentCycle++;
                //Check if the desired number of cycles is reached
                if (currentCycle >= maxCycles) break;

                // Increment numStableCycles only when it's in a stable state
                if (isInStableState) numStableCycles++;

                // Stop training once the model has remained stable for several cycles
                if (numStableCycles > 5) break;

            }


            Debug.WriteLine("It has reached the stable stage\n");
            stopwatchSp.Stop();
            Debug.WriteLine($"\nSpatial Pooler Training Time: {stopwatchSp.ElapsedMilliseconds} ms");
            // Train classifiers using the generated SDRs from training images
            ClassifierTraining(binarizedResult.TrainingImagesSDRs);
            // Perform predictions and image reconstruction using testing images
            PredictionAndReconstruction(sp, activeArray, imgHeight, imgWidth, binarizedResult.BinarizedTestingImagePaths);
            return sp;
        }

        /// <summary>
        /// Trains both the HTM and KNN classifiers using the Sparse Distributed Representations (SDRs) of training images.
        /// </summary>
        /// <param name="trainingImagesSDRs">
        /// A dictionary where the key is the actual image identifier (filename or label),
        /// and the value is an array of <see cref="Cell"/> objects representing the binarized SDR of the image.
        /// </param>
        public void ClassifierTraining(Dictionary<string, Cell[]> trainingImagesSDRs)
        {
            Debug.WriteLine("Starting Classifier Training Phase...");
            var trainedImages = new List<string>();
            Stopwatch stopwatchClassifier = Stopwatch.StartNew();
            foreach (var entry in trainingImagesSDRs)
            {
                string actualImageKey = entry.Key;
                Cell[] cells = entry.Value;

                htmClassifier.Learn(actualImageKey, cells);
                knnClassifier.Learn(actualImageKey, cells);
                trainedImages.Add(actualImageKey);
            }
            // Logging trained image names in a readable format
            Debug.WriteLine($"Trained HTM Classifier on Images: {string.Join(", ", trainedImages)}");
            Debug.WriteLine($"Trained KNN Classifier on Images: {string.Join(", ", trainedImages)}");
            Debug.WriteLine("Classifier Training Completed.\n");
            stopwatchClassifier.Stop();
            Debug.WriteLine($"Classifier Training Time: {stopwatchClassifier.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Performs image prediction and reconstruction using both HTM and KNN classifiers.
        /// Compares the reconstructed images and generates similarity plots.
        /// </summary>
        /// <param name="sp">The Spatial Pooler used for encoding.</param>
        /// <param name="activeArray">An array representing active columns in the Spatial Pooler.</param>
        /// <param name="imgHeight">The height of the images.</param>
        /// <param name="imgWidth">The width of the images.</param>
        /// <param name="binarizedTestingImagePaths">A list of file paths to the binarized testing images.</param>
        public void PredictionAndReconstruction(SpatialPooler sp, int[] activeArray, int imgHeight, int imgWidth, List<string> binarizedTestingImagePaths)
        {
            Debug.WriteLine("Starting Prediction Phase...");
            Stopwatch stopwatchReconstruction = Stopwatch.StartNew();

            // Directories for reconstructed images and similarity plots
            String outputReconstructedHTMFolder = ".\\ReconstructedHTM";
            String outputReconstructedKNNFolder = ".\\ReconstructedKNN";
            String htmSimilarityFolder = ".\\HTMSimilarityPlot";
            String knnSimilarityFolder = ".\\KNNSimilarityPlot";

            // Delete existing directories and recreate them
            foreach (var folder in new[] { outputReconstructedHTMFolder, outputReconstructedKNNFolder, htmSimilarityFolder, knnSimilarityFolder })
            {
                if (Directory.Exists(folder)) Directory.Delete(folder, true);
                Directory.CreateDirectory(folder);
            }

            // List for storing Similarities
            List<double> htmJacSimilarities = new List<double>();
            List<double> knnJacSimilarities = new List<double>();
            List<double> htmHamSimilarities = new List<double>();
            List<double> knnHamSimilarities = new List<double>();

            foreach (var binarizedImagePath in binarizedTestingImagePaths)
            {
                int[] inputVector = NeoCortexUtils.ReadCsvIntegers(binarizedImagePath).ToArray();

                Array.Clear(activeArray, 0, activeArray.Length);

                // Compute active columns without learning
                sp.compute(inputVector, activeArray, false);

                var activeCols = ArrayUtils.IndexWhere(activeArray, (el) => el == 1);
                var cells = activeCols.Select(index => new Cell { Index = index }).ToArray();

                // Predict using HTM and KNN classifiers
                var predictedImagesHTM = htmClassifier.GetPredictedInputValues(cells, 3);
                var predictedImagesKNN = knnClassifier.GetPredictedInputValues(cells, 3);

                // Get the highest similarity prediction for HTM
                var bestPredictionHTM = predictedImagesHTM.OrderByDescending(p => p.Similarity).First();
                Cell[] predictedHTMCells = bestPredictionHTM.SDR.Select(index => new Cell { Index = index }).ToArray();
                Debug.WriteLine($"Predicted Image by HTM Classifier: {bestPredictionHTM.PredictedInput}\nHTM predictive cells similarity: {bestPredictionHTM.Similarity / 100:F2}\n" +
                    $"SDR: [{string.Join(",", bestPredictionHTM.SDR)}]\n");
                // Reconstruct and evaluate similarity for HTM
                var (jacSimilarityHTM, hamSimilarityHTM, reconstructedHTMPath) = ImageReconstructor.ReconstructAndSave(imgHeight, imgWidth, sp, predictedHTMCells, outputReconstructedHTMFolder, $"HTM_reconstructed_{bestPredictionHTM.PredictedInput}.txt",
                    inputVector, bestPredictionHTM.PredictedInput);
                Debug.WriteLine($"Similarity between HTM Reconstructed Image and Original Binarized Image using Jaccard Similarity: {jacSimilarityHTM:F2} and Hamming Distance Similarity: {hamSimilarityHTM:F2}\n");
                double bestPredictionSimilarityHTM = Math.Round(bestPredictionHTM.Similarity / 100.0, 2);
                //Store the Jaccard similarity value for HTM
                htmJacSimilarities.Add(jacSimilarityHTM);
                //Store the Array similarity value for HTM
                htmHamSimilarities.Add(hamSimilarityHTM);

                // Get the highest similarity prediction for KNN
                var bestPredictionKNN = predictedImagesKNN.OrderByDescending(p => p.Similarity).First();
                Cell[] predictedKNNCells = bestPredictionKNN.SDR.Select(index => new Cell { Index = index }).ToArray();
                Debug.WriteLine($"Predicted Image by KNN Classifier: {bestPredictionKNN.PredictedInput}\nKNN predictive cells similarity: {bestPredictionKNN.Similarity:F2}\n" +
                    $"SDR: [{string.Join(",", bestPredictionKNN.SDR)}]\n");
                // Reconstruct and evaluate similarity for KNN
                var (jacSimilarityKNN, hamSimilarityKNN, reconstructedKNNPath) = ImageReconstructor.ReconstructAndSave(imgHeight, imgWidth, sp, predictedKNNCells, outputReconstructedKNNFolder, $"KNN_reconstructed_{bestPredictionKNN.PredictedInput}.txt",
                    inputVector, bestPredictionKNN.PredictedInput);
                Debug.WriteLine($"Similarity between KNN Reconstructed Image and Original Binarized Image using Jaccard Similarity: {jacSimilarityKNN:F2} and Hamming Distance Similarity: {hamSimilarityKNN:F2}\n");
                double bestPredictionSimilarityKNN = Math.Round(bestPredictionKNN.Similarity, 2);
                // Store the Jaccard Similarity value for KNN 
                knnJacSimilarities.Add(jacSimilarityKNN);
                // Store the Array Similarity value for KNN 
                knnHamSimilarities.Add(hamSimilarityKNN);
                stopwatchReconstruction.Stop();
                Debug.WriteLine($"Classifier Prediction and Reconstruction Time: {stopwatchReconstruction.ElapsedMilliseconds} ms");

                // Compare classifier performance
                string betterClassifier = bestPredictionSimilarityKNN > bestPredictionSimilarityHTM
                    ? "KNN" : (bestPredictionSimilarityHTM > bestPredictionSimilarityKNN ? "HTM" : "Both classifiers performed equally");

                // Output which classifier performed better or if both were equal
                if (betterClassifier == "Both classifiers performed equally")
                {
                    Debug.WriteLine($"Both classifiers performed equally for this Test Image with HTM internal similarity: {bestPredictionSimilarityHTM} and KNN internal similarity: {bestPredictionSimilarityKNN}");
                }
                else
                {
                    Debug.WriteLine($"{betterClassifier} performed better for this Test Image with HTM internal similarity: {bestPredictionSimilarityHTM} and KNN internal similarity: {bestPredictionSimilarityKNN}");
                }

                Debug.WriteLine("Starting comparison of reconstructed images...");
                // Compare reconstructed images
                ImageReconstructor.CompareReconstructedImages(reconstructedHTMPath, reconstructedKNNPath);
                Debug.WriteLine("Comparison of reconstructed images completed.\n");
            }
            // Generate the Similarity graph using the HTM & KNN Jaccard Similarity list
            DrawSimilarityGraph(htmJacSimilarities, htmSimilarityFolder, "HTM Similarity Graph.png", "HTM");
            DrawSimilarityGraph(knnJacSimilarities, knnSimilarityFolder, "KNN Similarity Graph.png", "KNN");
            // Generate the Similarity Scott Plot using the HTM & KNN Hamming Distance Similarity list
            PlotReconstructionResults(htmHamSimilarities, "HTM Similarity Plot", htmSimilarityFolder);
            PlotReconstructionResults(knnHamSimilarities, "KNN Similarity Plot", knnSimilarityFolder);

            Debug.WriteLine($"Reconstruction Completed");
            // Reset classifiers
            Debug.WriteLine("Resetting both the Classifiers for Next Experiment...");
            htmClassifier.ClearState();
            knnClassifier.ClearState();
        }
        /// <summary>
        /// Generates and saves a reconstruction similarity plot using ScottPlot.
        /// </summary>
        /// <param name="similarities">A list of similarity values for reconstructed images.</param>
        /// <param name="title">The title of the plot.</param>
        /// <param name="folderPath">The directory where the plot image will be saved.</param>
        public void PlotReconstructionResults(List<double> similarities, string title, string folderPath)
        {
            // Define plot size
            var plt = new ScottPlot.Plot(800, 600);

            // Generate X values (index of each test image)
            double[] xValues = Enumerable.Range(1, similarities.Count).Select(i => (double)i).ToArray();
            double[] yValues = similarities.ToArray();

            // Add scatter plot
            plt.AddScatter(xValues, yValues, lineWidth: 2, markerSize: 5);

            // Set titles and labels
            plt.Title(title);
            plt.XLabel("Test Image Index");
            plt.YLabel("Reconstruction Similarity");

            // Save plot as an image in the specified folder
            string filePath = Path.Combine(folderPath, $"{title.Replace(" ", "_")}.png");
            plt.SaveFig(filePath);

            Debug.WriteLine($"{title} saved: {filePath}");
        }

        /// <summary>
        /// Generates and saves a combined similarity plot from multiple similarity lists.
        /// </summary>
        /// <param name="similaritiesList">A list containing multiple lists of similarity values.</param>
        /// <param name="similarityFolder">The directory where the plot image will be saved.</param>
        /// <param name="fileName">The name of the output image file.</param>
        public static void DrawSimilarityGraph(List<double> similaritiesList, string similarityFolder, string fileName, string title)
        {
            // Combine all similarities from the list of arrays
            List<double> combinedSimilarities = new List<double>();
            foreach (var similarities in similaritiesList)
            {
                combinedSimilarities.AddRange(similarities);
            }

            // Define the file path with the folder path and file name
            string filePath = Path.Combine(similarityFolder, fileName);

            // Draw the combined similarity plot
            NeoCortexUtils.DrawCombinedSimilarityPlot(combinedSimilarities, filePath, 1000, 850);

            Debug.WriteLine($"Combined similarity Graph of {title} is generated and saved successfully.");

        }
    }
}