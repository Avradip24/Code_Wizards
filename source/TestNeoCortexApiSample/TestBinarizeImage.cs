using NeoCortexApiSample;
using SkiaSharp;

namespace TestNeoCortexApiSample
{
    /// <summary>
    /// Unit tests for the image binarization functionality in the ImageBinarizationUtils class.
    /// Ensures images are properly binarized and saved to the expected location.
    /// </summary>
    [TestClass]
    public sealed class TestBinarizeImage
    {
        private readonly string testImagePath = "test_image.png"; // Sample test image
        private readonly string outputDirectory = "TestBinarizedOutput";
        private readonly string outputFileName = "BinarizedTestImage";

        /// <summary>
        /// Initializes the test environment by creating a 64x64 black image.
        /// Ensures that the output directory exists before each test run.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
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
        }

        /// <summary>
        /// Tests the image binarization process.
        /// Verifies that the binarized image file is created with the expected filename.
        /// </summary>
        [TestMethod]
        public void TestBinarizeImages()
        {
            // Arrange
            string outputFilePath = Path.Combine(outputDirectory, outputFileName);
            int imageWidth = 64;
            int imageHeight = 64;

            // Act
            string binaryImage = ImageBinarizationUtils.BinarizeImages(imageWidth, imageHeight, outputFilePath, testImagePath);

            string binarizedImage = Path.GetFileNameWithoutExtension(binaryImage);
            // Assert
            Assert.AreEqual(outputFileName, binarizedImage);
        }

        /// <summary>
        /// Cleans up test artifacts by deleting the test image and output directory after execution.
        /// Ensures that temporary test files do not persist between test runs.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Delete test files after execution
            if (File.Exists(testImagePath)) File.Delete(testImagePath);
            if (Directory.Exists(outputDirectory)) Directory.Delete(outputDirectory, true);
        }
    }
}