using NeoCortexApi.Entities;
using NeoCortexApi;
using NeoCortexApiSample;

namespace TestNeoCortexApiSample;

[TestClass]
public class TestImageReconstructor
{
    private int imgWidth = 64;
    private int imgHeight = 64;

    [TestMethod]
    public void TestReconstructAndSave()
    {
        // Arrange
        double minOctOverlapCycles = 1.0;
        double maxBoost = 5.0;
        // We will build a slice of the cortex with the given number of mini-columns
        int numColumns = 64 * 64;
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

        string testBinarizedFolder;
        string testHTMFile;
        string testKNNFile;
        string testOutputFolder = "TestReconstructedOutput";

        // Ensure the output directory exists
        if (!Directory.Exists(testOutputFolder))
            Directory.CreateDirectory(testOutputFolder);

        testBinarizedFolder = Path.Combine(Directory.GetCurrentDirectory(), "BinarizedImages");

        if (!Directory.Exists(testBinarizedFolder))
            Directory.CreateDirectory(testBinarizedFolder);

        // Create dummy HTM and KNN reconstructed files
        testHTMFile = Path.Combine(testOutputFolder, "HTM_reconstructed_testImage.txt");
        testKNNFile = Path.Combine(testOutputFolder, "KNN_reconstructed_testImage.txt");

        string fileName = "Reconstructed_Test.txt";

        int[] inputVector = Enumerable.Repeat(0, 80).Concat(Enumerable.Repeat(1, 20)).ToArray();
        var mem = new Connections(cfg);
        SpatialPooler sp = new SpatialPooler();
        //Initializing the Spatial Pooler Algorithm
        sp.Init(mem, new DistributedMemory() { ColumnDictionary = new InMemoryDistributedDictionary<int, NeoCortexApi.Entities.Column>(1) });
        // Ensure 82 distinct values
        Cell[] predictedCells = new Cell[82];
        // To store unique values
        HashSet<int> uniqueIndexes = new HashSet<int>(); 
        Random random = new Random();

        int i = 0;
        // Ensure 82 distinct values
        while (uniqueIndexes.Count < 82) 
        {
            // Generate random index
            int newIndex = random.Next(0, 1000);
            // Adds if not already present
            if (uniqueIndexes.Add(newIndex)) 
            {
                predictedCells[i] = new Cell { Index = newIndex };
                i++;
            }
        }
        string binarizedImage = "testImage";
        string binarizedImagePath = Path.Combine(testBinarizedFolder, $"{binarizedImage}Training_Binarized.txt");
        // The binary pattern 
        string pattern = "01101100";

        // Ensure each row has imgWidth length
        string row = string.Concat(Enumerable.Repeat(pattern, (imgWidth / pattern.Length) + 1))
                           .Substring(0, imgWidth); // Trim to match exact width

        // Repeat this row for imgHeight rows
        string[] grid = Enumerable.Repeat(row, imgHeight).ToArray();

        // Write to file
        File.WriteAllLines(testHTMFile, grid);
        File.WriteAllLines(testKNNFile, grid);
        File.WriteAllLines(binarizedImagePath, grid);

        // Act
        var (jacSimilarity, hamSimilarity, reconstructedFilePath) = ImageReconstructor.ReconstructAndSave(
            imgHeight, imgWidth, sp, predictedCells, testOutputFolder, fileName, inputVector, binarizedImage
        );

        // Assert
        Assert.IsTrue(File.Exists(reconstructedFilePath), "Reconstructed file was not created.");
        Assert.IsTrue(jacSimilarity >= 0 && jacSimilarity <= 1, "Jaccard Similarity should be between 0 and 1.");
        Assert.IsTrue(hamSimilarity >= 0 && hamSimilarity <= 100, "Hamming Distance Similarity should be between 0 and 100.");

        if (Directory.Exists(testOutputFolder))
            Directory.Delete(testOutputFolder, true);

        if (Directory.Exists(testBinarizedFolder))
            Directory.Delete(testBinarizedFolder, true);
    }

    [TestMethod]
    public void CompareReconstructedImages()
    {
        string testCompareHTMFile;
        string testCompareKNNFile;
        string testCompareOutputFolder = "TestCompareReconstructedOutput";
        // Ensure the output directory exists
        if (!Directory.Exists(testCompareOutputFolder))
            Directory.CreateDirectory(testCompareOutputFolder);
        // Create dummy HTM and KNN reconstructed files
        testCompareHTMFile = Path.Combine(testCompareOutputFolder, "HTM_Reconstructed_testImage.txt");
        testCompareKNNFile = Path.Combine(testCompareOutputFolder, "KNN_Reconstructed_testImage.txt");
        string pattern = "01101100"; // The repeating binary pattern

        // Generate a row that matches imgWidth
        string row = string.Concat(Enumerable.Repeat(pattern, (imgWidth / pattern.Length) + 1))
                           .Substring(0, imgWidth); // Ensure exact width

        // Generate the grid with imgHeight rows
        string[] grid = Enumerable.Repeat(row, imgHeight).ToArray();
        File.WriteAllLines(testCompareHTMFile, grid);
        File.WriteAllLines(testCompareKNNFile, grid);

        // Capture log messages using a List<string>
        List<string> logMessages = new List<string>();

        // Run the method with a custom logger
        ImageReconstructor.CompareReconstructedImages(testCompareHTMFile, testCompareKNNFile, logMessages.Add);

        // Verify that similarity output is logged
        Assert.IsTrue(logMessages.Count > 0, "No log messages captured.");
        Assert.IsTrue(logMessages[0].Contains("Similarity between HTM"), "Expected log message format not found.");


        if (Directory.Exists(testCompareOutputFolder))
            Directory.Delete(testCompareOutputFolder, true);
    }
}