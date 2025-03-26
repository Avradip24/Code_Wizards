# Methodology: ML 24/25-01 Investigate Image Reconstruction by using Classifiers
Here are all the Classes and Methods inside them that has been created to run the experiment keeping in mind the concept of code reusabilty.

## Table of Contents

- [Program.cs](#program.cs)
- [ImageBinarizerSpatialPattern.cs](#imagebinarizerspatialpattern.cs)
- [BinarizeImage.cs](#binarizeimage.cs)
- [ImageReconstruction.cs](#imagereconstruction.cs)
- [IClassifier.cs](#iclassifier.cs)
- [HtmClassifier.cs](#htmclassifier.cs)
- [KnnClassifier.cs](#knnclassifier.cs)

### 1. Program.cs 
This file serves as the entry point for the Image Reconstruction experiment. It initializes and triggers the core experiment by invoking the ImageBinarizerSpatialPattern class. Upon execution, the program first displays a welcome message, introducing the experiment's purpose and acknowledging the contributors. 
This experiment follows a structured pipeline that includes image binarization, Spatial Pooling algorithm, generation of SDRs, training of classifiers (HTM & KNN), prediction of images, and image reconstruction to assess the accuracy of the classifiers. By modularizing the functionality, Program.cs   ensures code reusability and maintainability, allowing the experiment to be extended or modified with ease. [Program.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/Program.cs)

### 2. ImageBinarizerSpatialPattern.cs
This code showcases the functionality of Spatial Pooler, Classifier Training and Prediction, Reconstruction and Similarity comparsions. The set of images are divided into Training Dataset (80%) and Testing Dataset(20%) then both the set of images undergo binarization, the binarized images are converted into a one-dimensional array whichis fed into the Spatial Pooler for training and SDR generation. The training process persists until the spatial pooler achieved stability, with oversight from the HomeostaticPlasticityController (HPC) class. The generated SDRs are passed to both the classifier in order to train them with the test images. After the completion of the training phase, the classifier starts it's prediction phase to get the best predicted images based on the test dataset. As this experiment ends the classifiers are resetted for the next experiment. Thereafter the predicted images by both the classiffiers are reconstructed to binarized form and the similarity between the original images and reconstructed images is calculated. With the similarities obtained from both the classifiers, a Scott Plot and a Graph Plot is constructed. The classifier performance is also calculated based to the similarities obtained. [ImageBinarizerSpatialPattern.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/ImageBinarizerSpatialPattern.cs)
#### Splitting image dataset and Binarizing them - binarizeImage()
```
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
```
#### Training SP for SDR generation - RunExperiment()
```
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
```
#### Training the Classifiers - ClassifierTraining()
```
    Stopwatch stopwatchClassifier = Stopwatch.StartNew();
    foreach (var entry in trainingImagesSDRs)
    {
        string actualImageKey = entry.Key;
        Cell[] cells = entry.Value;

        htmClassifier.Learn(actualImageKey, cells);
        knnClassifier.Learn(actualImageKey, cells);
        trainedImages.Add(actualImageKey);
    }
```
#### Classifier Prediction and Reconstruction - PredictionAndReconstruction() 
```
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
//Store the Hamming similarity value for HTM
htmHamSimilarities.Add(hamSimilarityHTM);
```
#### Scott Plot for Hamming Distance Similarity - PlotReconstructionResults()
```
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
```
#### Graph Plot for Jaccard Index Similarity - DrawSimilarityGraph()
```
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
```
### 3. BinarizeImage.cs 
This class is designed to perform the binarization, the method initializes an instance of the ImageBinarizer class with specific parameters that define how the image is processed. It 
sets red, green, and blue threshold values to 200, meaning that any pixel with RGB values exceeding these thresholds is considered active and assigned a binary 
value of 1, while all other pixels are marked as inactive with a 0. The image is also resized to the specified width and height to maintain consistency in input 
dimensions, ensuring compatibility with downstream processing models.
Finally, the method returns the path of the binarized text file, which now contains a structured binary representation of the original image. [BinarizeImage.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/BinarizeImage.cs)
```
public static string BinarizeImages(int imageWidth, int imageHeight, string destinationPath, string imagePath)
{
    string binaryImage = $"{destinationPath}.txt";

    // Delete existing file if it exists
    if (File.Exists(binaryImage)) File.Delete(binaryImage);


    // Initialize and run the binarizer
    ImageBinarizer imageBinarizer = new ImageBinarizer(new BinarizerParams
    {
        RedThreshold = 200,
        GreenThreshold = 200,
        BlueThreshold = 200,
        ImageWidth = imageWidth,
        ImageHeight = imageHeight,
        InputImagePath = imagePath,
        OutputImagePath = binaryImage
    });

    imageBinarizer.Run();
    //// Returning the binary data
    return binaryImage; 
    
}
```
### 4. ImageReconstruction.cs
The ImageReconstructor class is responsible for reconstructing binarized images from predicted cell activity using a Spatial Pooler (SP) and evaluating their quality through similarity metrics. It ensures all pixels have permanence values, thresholds them into a binary format, and writes the reconstructed image in a structured way. The method then reads both the original binarized image and the reconstructed version to compute Jaccard Index Similarity, which measures the overlap between the two images, and Hamming Distance, which calculates pixel-wise differences. The CompareReconstructedImages method further evaluates the similarity between reconstructed images from HTM and KNN classifiers by computing their Jaccard Similarity, allowing an analysis of which classifier produces more accurate reconstructions. This implementation facilitates image reconstruction validation and comparison, ensuring an efficient approach to evaluating classifier performance based on image similarity. The reconstructed images are stored in separate folders, enabling direct comparison of the results. [ImageReconstruction.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/ImageReconstructor.cs)
#### Reconstruction of predicted image and similarity calculation - ReconstructAndSave()
```
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
```
#### Comparison of reconstructed images - CompareReconstructedImages()
```
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
```
### 5. IClassifier.cs
The IClassifier<TIN, TOUT> interface defines a generic classifier capable of learning patterns from input data and making predictions based on previously learned associations. It provides four key methods: Learn, which trains the classifier by associating a key (label) with a set of active cells; GetPredictedInputValues, which retrieves predicted inputs based on a given set of predictive cells, returning a list of PredictedResult<TOUT> objects; ClearState, which resets the classifier by removing all learned data; and ReconstructInput, which reconstructs the original input from predicted results. This interface provides a structured way to implement classifiers for Hierarchical Temporal Memory (HTM) or other machine learning models, allowing flexible learning, prediction, and reconstruction of input data. [IClassifier.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/IClassifier.cs)
```
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
```
### 6. HtmClassifier.cs
The HtmImageClassifier implements IClassifier<Cell, string> and acts as a wrapper around HtmClassifier<string, ComputeCycle> for image classification using Sparse Distributed Representations (SDRs). It supports training, prediction, resetting, and input reconstruction. The Learn method associates labels with SDRs, while GetPredictedInputValues retrieves top-k predictions with similarity scores. ClearState resets learned associations, and ReconstructInput extracts the most probable SDR from predictions. This classifier enables HTM-based learning for recognizing and reconstructing binarized image patterns.
[HtmClassifier.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/htmClassifier.cs)

### 7. KnnClassifier.cs
The KnnImageClassifier also implements IClassifier<Cell, string> but utilizes a k-Nearest Neighbors (k-NN) approach for image recognition. It wraps KNeighborsClassifier<string, ComputeCycle>, providing learning, prediction, and reconstruction functionalities. Learn maps labels to SDRs, GetPredictedInputValues retrieves predictions based on similarity, ClearState resets learned data, and ReconstructInput extracts the most similar SDR. This classifier applies k-NN for recognizing and reconstructing binarized images based on learned spatial patterns. [KnnClassifier.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/knnClassifier.cs)

  The link to all the above mentioned classes is given here -     [Classes Link](https://github.com/Avradip24/Code_Wizards/tree/master/source/Samples/NeoCortexApiSample)
