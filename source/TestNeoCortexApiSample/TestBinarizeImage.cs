using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using NeoCortexApiSample;

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

            // Ensure a real test image exists (use an actual image file if possible)
            if (!File.Exists(testImagePath))
            {
                // Create a placeholder black image (64x64 PNG)
                using (var bmp = new System.Drawing.Bitmap(64, 64))
                {
                    bmp.Save(testImagePath, System.Drawing.Imaging.ImageFormat.Png);
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
