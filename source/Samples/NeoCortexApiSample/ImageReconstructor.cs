using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoCortexApiSample
{
    public class ImageReconstructor
    {
        private int imgWidth;
        private int imgHeight;

        public ImageReconstructor(int imgWidth, int imgHeight)
        {
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
        }

        public void SaveReconstructedImage(int[] SDR, string outputFolder, string prefix, string predictedLabel)
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string outputFileName = $"{prefix}_{predictedLabel}.txt";
            string filePath = Path.Combine(outputFolder, outputFileName);

            GenerateBinarizedImageAsText(SDR, filePath);
        }

        private void GenerateBinarizedImageAsText(int[] SDR, string filePath)
        {
            // Create a 2D array filled with '0's
            char[,] binarizedImage = new char[imgHeight, imgWidth];

            for (int i = 0; i < imgHeight; i++)
                for (int j = 0; j < imgWidth; j++)
                    binarizedImage[i, j] = '0';

            // Directly map SDR indices to grid positions
            foreach (int index in SDR)
            {
                if (index >= 0 && index < imgWidth * imgHeight) // Ensure index is within bounds
                {
                    int row = index / imgWidth;  // Calculate row position
                    int col = index % imgWidth;  // Calculate column position

                    binarizedImage[row, col] = '1';  // Mark active cells as '1'
                }
            }

            // Write to text file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < imgHeight; i++)
                {
                    for (int j = 0; j < imgWidth; j++)
                    {
                        writer.Write(binarizedImage[i, j]);
                    }
                    writer.WriteLine(); // Newline for next row
                }
            }
        }
    }
}