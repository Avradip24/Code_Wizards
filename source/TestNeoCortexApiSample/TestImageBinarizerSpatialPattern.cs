using NeoCortexApi.Entities;
using NeoCortexApi;
using NeoCortexApiSample;
using SkiaSharp;

namespace TestNeoCortexApiSample
{
    /// <summary>
    /// Unit tests for the <see cref="ImageBinarizerSpatialPattern"/> class.
    /// This class tests various functionalities including image binarization,
    /// similarity graph generation.
    /// </summary>
    [TestClass]
    public class TestImageBinarizerSpatialPattern
    {
        private Dictionary<string, Cell[]>? sampleTrainingData;
        private List<string>? sampleBinarizedTestingImagePaths;
        private SpatialPooler? sp;
        private int[]? activeArray;
        private int imgHeight = 64;
        private int imgWidth = 64;

        /// <summary>
        /// Initializes the test setup with sample training data,
        /// binarized test image paths
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            sampleTrainingData = new Dictionary<string, Cell[]>
            {
                { "image1", new Cell[] { new Cell { Index = 1 }, new Cell { Index = 2 } } },
                { "image2", new Cell[] { new Cell { Index = 3 }, new Cell { Index = 4 } } }
            };
            sampleBinarizedTestingImagePaths = new List<string> { "test1.csv", "test2.csv" };
            sp = new SpatialPooler();
            activeArray = new int[imgHeight * imgWidth];
        }

        /// <summary>
        /// Tests the image binarization process.
        /// Verifies that the binarized image result is generated correctly.
        /// </summary>
        [TestMethod]
        public void TestBinarizeImage()
        {

            // Arrange
            string outputDirectory = "TestBinarizedOutput";
            string outputFileName = "test_image.png"; // Image file name
            string testImagePath = Path.Combine(outputDirectory, outputFileName); // Full path for the image

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // Create a 64x64 image with a black background
            using (var surface = SKSurface.Create(new SKImageInfo(64, 64)))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Black); // Fill with black

                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(testImagePath))
                {
                    data.SaveTo(stream);
                }
            }

            // Capture log messages using a List<string>
            List<string> logMessages = new List<string>();

            var result = ImageBinarizerSpatialPattern.binarizeImage(outputDirectory);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.ActualImages, "ActualImages should not be null");
        }

        /// <summary>
        /// Tests if PlotReconstructionResults generates a valid similarity plot.
        /// </summary>
        [TestMethod]
        public void TestPlotReconstructionResults()
        {
            // Arrange
            List<double> similarities = new List<double> { 0.8, 0.7, 0.9 };
            string title = "Test Similarity Plot";
            string folderPath = Path.Combine(Environment.CurrentDirectory, "TestPlots");
            Directory.CreateDirectory(folderPath);

            // Act
            ImageBinarizerSpatialPattern.PlotReconstructionResults(similarities, title, folderPath);

            // Assert
            string expectedFilePath = Path.Combine(folderPath, "Test_Similarity_Plot.png");
            Assert.IsTrue(File.Exists(expectedFilePath), "The similarity plot should be created.");
        }
        /// <summary>
        /// Tests if DrawSimilarityGraph generates a combined similarity graph correctly.
        /// </summary>
        [TestMethod]
        public void TestDrawSimilarityGraph()
        {
            // Arrange
            List<double> similarities = new List<double> { 0.85, 0.78, 0.9 };
            string folderPath = Path.Combine(Environment.CurrentDirectory, "TestGraphs");
            Directory.CreateDirectory(folderPath);
            string fileName = "TestGraph.png";

            // Act
            ImageBinarizerSpatialPattern.DrawSimilarityGraph(similarities, folderPath, fileName, "Test Graph");

            // Assert
            string expectedFilePath = Path.Combine(folderPath, fileName);
            Assert.IsTrue(File.Exists(expectedFilePath), "The similarity graph should be generated.");
        }
    }
}