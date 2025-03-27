using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortexApi.Entities;
using NeoCortexApi;
using NeoCortexApiSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestNeoCortexApiSample
{
    [TestClass]
    public class TestImageBinarizerSpatialPattern
    {
        private ImageBinarizerSpatialPattern classifierInstance;
        private Dictionary<string, Cell[]> sampleTrainingData;
        private List<string> sampleBinarizedTestingImagePaths;
        private SpatialPooler sp;
        private int[] activeArray;
        private int imgHeight = 64;
        private int imgWidth = 64;

        [TestInitialize]
        public void Setup()
        {
            classifierInstance = new ImageBinarizerSpatialPattern();
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
            classifierInstance.PlotReconstructionResults(similarities, title, folderPath);

            // Assert
            string expectedFilePath = Path.Combine(folderPath, "Test_Similarity_Plot.png");
            Assert.IsTrue(File.Exists(expectedFilePath), "The similarity plot should be created.");
        }
    }
}