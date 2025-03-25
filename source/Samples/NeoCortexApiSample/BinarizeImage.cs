using Daenet.ImageBinarizerLib.Entities;
using Daenet.ImageBinarizerLib;
using System.Linq;
using System.IO;

namespace NeoCortexApiSample
{
    public class ImageBinarizationUtils
    {
        /// <summary>
        /// Converts an image to a binarized format and saves it as a .txt file.
        /// </summary>
        /// <param name="imageWidth">Width of the binarized image.</param>
        /// <param name="imageHeight">Height of the binarized image.</param>
        /// <param name="destinationPath">The file path where the binarized image will be saved.</param>
        /// <param name="imagePath">Path to the input image file.</param>
        /// <returns>Path of the saved binarized image in .txt format.</returns>
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
            // Inverting the binarized images 
            var binaryData = File.ReadAllLines(binaryImage).Select(line => new string(line.Select(ch => ch == '0' ? '1' : '0').ToArray())).ToArray();
            // Writing the binary data to the file
            File.WriteAllLines(binaryImage, binaryData);
            return binaryImage; 
            
        }
    }
}
