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
        public string inputPrefix { get; private set; }
        private IClassifier<Cell, string> htmClassifier;
        private IClassifier<Cell, string> knnClassifier;
        public ImageBinarizerSpatialPattern()

        {
            htmClassifier = new HtmImageClassifier();
            knnClassifier = new KnnImageClassifier();

        }

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
            //int imageSize = 25;
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
        public SpatialPooler RunExperiment(HtmConfig cfg, string inputPrefix)
        {
            var mem = new Connections(cfg);
            bool isInStableState = false;
            int numColumns = 64 * 64;

            //Accessing the Image Folder form the Cureent Directory
            string trainingFolder = "Sample\\TestFiles";
            //Accessing the Image Folder form the Cureent Directory Folder
            var actualImages = Directory.EnumerateFiles(trainingFolder).Where(file => file.StartsWith($"{trainingFolder}\\{inputPrefix}") &&
            (file.EndsWith(".jpeg") || file.EndsWith(".jpg") || file.EndsWith(".png"))).ToArray();

            // Shuffle to ensure randomness
            Random rnd = new Random();
            actualImages = actualImages.OrderBy(x => rnd.Next()).ToArray();

            // Define split ratio (e.g., 80% training, 20% testing)
            int trainSize = (int)(actualImages.Length * 0.8);
            var trainingImages = actualImages.Take(trainSize).ToArray();
            var testingImages = actualImages.Skip(trainSize).ToArray();

            //Image Size
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

            foreach (var trainingImage in trainingImages)
            {
                string trainingImageKey = Path.GetFileNameWithoutExtension(trainingImage);

                // Construct the output file name based on the input file name
                string outputFileName = trainingImageKey + "Training_Binarized";
                string outputPath = Path.Combine(outputFolder, outputFileName);

                // Binarizing the images
                string binarizedImagePath = ImageBinarizationUtils.BinarizeImages(imgWidth, imgHeight, outputPath, trainingImage);
                binarizedTrainingImagePaths.Add(binarizedImagePath);
                //Store mapping from binarized to actual
                binarizedTrainingToActualMap[outputFileName] = trainingImageKey;
            }
            foreach (var testImage in testingImages)
            {
                string testingImageKey = Path.GetFileNameWithoutExtension(testImage);

                // Construct the output file name based on the input file name
                string outputFileName = testingImageKey + "Testing_Binarized";
                string outputPath = Path.Combine(outputFolder, outputFileName);

                // Binarizing the images
                string binarizedImagePath = ImageBinarizationUtils.BinarizeImages(imgWidth, imgHeight, outputPath, testImage);
                binarizedTestingImagePaths.Add(binarizedImagePath);
                //Store mapping from binarized to actual
                binarizedTestToActualMap[outputFileName] = testingImageKey;
            }
            Debug.WriteLine("All images are binarized");


            HomeostaticPlasticityController hpa = new HomeostaticPlasticityController(mem, actualImages.Length * 50, (isStable, numPatterns, actColAvg, seenInputs) =>
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
            //It creates the instance of HTMClassifier
            // HtmClassifier<string, ComputeCycle> htmClassifier = new HtmClassifier<string, ComputeCycle>();
            //It creates the instance of KNNClassifier
            // var knnClassifier = new KNeighborsClassifier<string, ComputeCycle>();

            int[] activeArray = new int[numColumns];

            int numStableCycles = 0;
            int maxCycles = 500;
            int currentCycle = 0;

            // ===========================
            //       SPATIAL POOLER PHASE
            // ===========================
            Stopwatch stopwatchSp = Stopwatch.StartNew();
            while (currentCycle < maxCycles)
            {
                foreach (var binarizedImagePath in binarizedTrainingImagePaths)
                {
                    // Read Binarized and Encoded input CSV file into an array
                    int[] inputVector = NeoCortexUtils.ReadCsvIntegers(binarizedImagePath).ToArray();

                    sp.compute(inputVector, activeArray, true);

                    // Getting the Active Columns
                    var activeCols = ArrayUtils.IndexWhere(activeArray, (el) => el == 1);
                    var cells = activeCols.Select(index => new Cell { Index = index }).ToArray();

                    string binarizedKey = Path.GetFileNameWithoutExtension(binarizedImagePath);

                    // Store SDR representation mapped to the actual image for later training
                    string trainingImageKey = binarizedTrainingToActualMap[binarizedKey];
                    trainingImagesSDRs[trainingImageKey] = cells;

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

                if (numStableCycles > 5) break;

            }


            Debug.WriteLine("It has reached the stable stage\n");
            stopwatchSp.Stop();
            Debug.WriteLine($"\nSpatial Pooler Training Time: {stopwatchSp.ElapsedMilliseconds} ms");
            ClassifierTraining(trainingImagesSDRs);
            this.PredictionAndReconstruction(sp, activeArray, imgHeight, imgWidth, binarizedTestingImagePaths);
            return sp;
        }
        // ===========================
        //      CLASSIFIER TRAINING PHASE
        // ===========================
        public void ClassifierTraining(Dictionary<string, Cell[]> trainingImagesSDRs)
        {
            Debug.WriteLine("Starting Classifier Training Phase...");

            Stopwatch stopwatchClassifier = Stopwatch.StartNew();
            foreach (var entry in trainingImagesSDRs)
            {
                string actualImageKey = entry.Key;
                Cell[] cells = entry.Value;

                htmClassifier.Learn(actualImageKey, cells);
                Debug.WriteLine($"Trained HTM Classifier on Image: {actualImageKey}");
                knnClassifier.Learn(actualImageKey, cells);
                Debug.WriteLine($"Trained KNN Classifier on Image: {actualImageKey}");
            }

            Debug.WriteLine("Classifier Training Completed.\n");
            stopwatchClassifier.Stop();
            Debug.WriteLine($"Classifier Training Time: {stopwatchClassifier.ElapsedMilliseconds} ms");
        }

        // ===========================
        //      PREDICTION PHASE
        // ===========================
        public void PredictionAndReconstruction(SpatialPooler sp, int[] activeArray, int imgHeight, int imgWidth, List<string> binarizedTestingImagePaths)
        {
            Debug.WriteLine("Starting Prediction Phase...");
            Stopwatch stopwatchReconstruction = Stopwatch.StartNew();
            String outputReconstructedHTMFolder = ".\\ReconstructedHTM";
            if (Directory.Exists(outputReconstructedHTMFolder)) Directory.Delete(outputReconstructedHTMFolder, true);
            // Recreate the folder
            Directory.CreateDirectory(outputReconstructedHTMFolder);

            String outputReconstructedKNNFolder = ".\\ReconstructedKNN";
            if (Directory.Exists(outputReconstructedKNNFolder)) Directory.Delete(outputReconstructedKNNFolder, true);
            // Recreate the folder
            Directory.CreateDirectory(outputReconstructedKNNFolder);

            String htmSimilarityFolder = ".\\HTMSimilarityPlot";
            if (Directory.Exists(htmSimilarityFolder)) Directory.Delete(htmSimilarityFolder, true);
            // Recreate the folder
            Directory.CreateDirectory(htmSimilarityFolder);

            String knnSimilarityFolder = ".\\KNNSimilarityPlot";
            if (Directory.Exists(knnSimilarityFolder)) Directory.Delete(knnSimilarityFolder, true);
            // Recreate the folder
            Directory.CreateDirectory(knnSimilarityFolder);

            // Instantiate the ImageReconstructor with required dimensions
            ImageReconstructor reconstructor = new ImageReconstructor(imgWidth, imgHeight);
            List<double> htmSimilarities = new List<double>();
            List<double> knnSimilarities = new List<double>();

            foreach (var binarizedImagePath in binarizedTestingImagePaths)
            {
                int[] inputVector = NeoCortexUtils.ReadCsvIntegers(binarizedImagePath).ToArray();

                Array.Clear(activeArray, 0, activeArray.Length);

                sp.compute(inputVector, activeArray, false);  // false prevents SP from updating

                var activeCols = ArrayUtils.IndexWhere(activeArray, (el) => el == 1);
                var cells = activeCols.Select(index => new Cell { Index = index }).ToArray();

                // Get predicted image by htm classifier
                var predictedImagesHTM = htmClassifier.GetPredictedInputValues(cells, 3);

                // Get top predicted image SDRs from KNN Classifier
                var predictedImagesKNN = knnClassifier.GetPredictedInputValues(cells, 3);

                // Get the highest similarity prediction
                var bestPredictionHTM = predictedImagesHTM.OrderByDescending(p => p.Similarity).First();
                Cell[] predictedHTMCells = bestPredictionHTM.SDR.Select(index => new Cell { Index = index }).ToArray();
                Debug.WriteLine($"Predicted Image by HTM Classifier: {bestPredictionHTM.PredictedInput}\nHTM predictive cells similarity: {bestPredictionHTM.Similarity / 100:F2}\nSDR: [{string.Join(",", bestPredictionHTM.SDR)}]\n");
                double similarityHTM = reconstructor.ReconstructAndSave(sp, predictedHTMCells, outputReconstructedHTMFolder, $"HTM_reconstructed_{bestPredictionHTM.PredictedInput}.txt", inputVector, bestPredictionHTM.PredictedInput);
                Debug.WriteLine($"Similarity between HTM Reconstructed Image and Original Binarized image: {similarityHTM:F2}\n");
                double bestPredictionSimilarityHTM = Math.Round(bestPredictionHTM.Similarity / 100.0, 2);
                //Store the similarity value for HTM
                htmSimilarities.Add(similarityHTM); // Store similarity value


                // Get the highest similarity prediction
                var bestPredictionKNN = predictedImagesKNN.OrderByDescending(p => p.Similarity).First();
                Cell[] predictedKNNCells = bestPredictionKNN.SDR.Select(index => new Cell { Index = index }).ToArray();
                Debug.WriteLine($"Predicted Image by KNN Classifier: {bestPredictionKNN.PredictedInput}\nKNN predictive cells similarity: {bestPredictionKNN.Similarity:F2}\nSDR: [{string.Join(",", bestPredictionKNN.SDR)}]\n");
                double similarityKNN = reconstructor.ReconstructAndSave(sp, predictedKNNCells, outputReconstructedKNNFolder, $"KNN_reconstructed_{bestPredictionKNN.PredictedInput}.txt", inputVector, bestPredictionKNN.PredictedInput);
                Debug.WriteLine($"Similarity between KNN Reconstructed Image and Original Binarized image: {similarityKNN:F2}\n");
                double bestPredictionSimilarityKNN = Math.Round(bestPredictionKNN.Similarity,2);
                // Store similarity for KNN and debug
                knnSimilarities.Add(similarityKNN); // Store similarity for KNN
                Debug.WriteLine($"Storing KNN Similarity: {bestPredictionSimilarityKNN}, HTM Similarity: {bestPredictionSimilarityHTM}");
                stopwatchReconstruction.Stop();
                Debug.WriteLine($"Classifier Prediction and Reconstruction Time: {stopwatchReconstruction.ElapsedMilliseconds} ms");
                
                // ========================
                //Comparison of Classifiers
                //========================
                //Add per - input comparison based on similarity

                string betterClassifier = bestPredictionSimilarityKNN > bestPredictionSimilarityHTM
                    ? "KNN" : (bestPredictionSimilarityHTM > bestPredictionSimilarityKNN ? "HTM" : "Both classifiers performed equally");

                // Output which classifier performed better or if both were equal
                if (betterClassifier == "Both classifiers performed equally")
                {
                    Debug.WriteLine($"Both classifiers performed equally for image with KNN similarity: {bestPredictionSimilarityKNN} and HTM similarity: {bestPredictionSimilarityHTM}");
                }
                else
                {
                    Debug.WriteLine($"{betterClassifier} performed better for image with KNN similarity: {bestPredictionSimilarityKNN} and HTM similarity: {bestPredictionSimilarityHTM}");
                }
                Debug.WriteLine("Starting comparison of reconstructed images...");
                reconstructor.CompareReconstructedImages(outputReconstructedKNNFolder, outputReconstructedHTMFolder);
                Debug.WriteLine("Comparison of reconstructed images completed.");
            }
            // Generate the Similarity graph using the HTM Similarity list
            DrawSimilarityPlots(htmSimilarities, htmSimilarityFolder, "HTM Similarity Graph.png");
            // Generate the Similarity graph using the KNN Similarity list
            DrawSimilarityPlots(knnSimilarities, knnSimilarityFolder, "KNN Similarity Graph.png");
            PlotReconstructionResults(htmSimilarities, "HTM Similarity Plot", htmSimilarityFolder);
            PlotReconstructionResults(knnSimilarities, "KNN Similarity Plot", knnSimilarityFolder);

            Debug.WriteLine($"Reconstruction Completed");

            // ===========================
            //    RESET CLASSIFIER
            // ===========================
            Debug.WriteLine("Resetting  both the Classifiers for Next Experiment...");
            htmClassifier.ClearState();
            knnClassifier.ClearState();
        }

        public void PlotReconstructionResults(List<double> similarities, string title, string folderPath)
        {
            var plt = new ScottPlot.Plot(800, 600); // Define plot size

            // Generate X values (index of each test image)
            double[] xValues = Enumerable.Range(0, similarities.Count).Select(i => (double)i).ToArray();
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

            Console.WriteLine($"Plot saved: {filePath}");
        }

        public static void DrawSimilarityPlots(List<double> similaritiesList, string similarityFolder, string fileName)
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

            Debug.WriteLine($"Combined similarity plot generated and saved successfully.");

        }
    }
}