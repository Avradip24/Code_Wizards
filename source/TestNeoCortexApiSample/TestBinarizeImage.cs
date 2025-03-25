using NeoCortexApiSample;
using SkiaSharp;

namespace TestNeoCortexApiSample
{
    [TestClass]
    public sealed class TestBinarizeImage
    {
        private readonly string testImagePath = "test_image.png"; // Sample test image
        private readonly string outputDirectory = "TestBinarizedOutput";
        private readonly string outputFileName = "BinarizedTestImage";

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

        [TestCleanup]
        public void Cleanup()
        {
            // Delete test files after execution
            if (File.Exists(testImagePath)) File.Delete(testImagePath);
            if (Directory.Exists(outputDirectory)) Directory.Delete(outputDirectory, true);
        }
    }
}