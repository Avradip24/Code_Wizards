using ExcelDataReader;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using NeoCortexApi.Entities;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Org.BouncyCastle.Ocsp;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using static NeoCortexApiSample.MultisequenceLearningTeamMSL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NeoCortexApiSample
{
    class Program
    {
        /// <summary>
        /// This sample shows a typical experiment code for SP and TM.
        /// You must start this code in debugger to follow the trace.
        /// and TM.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            Console.WriteLine($"Hello! Welcome to our project ML 24/25-01");
            Console.WriteLine("Investigate Image Reconstruction by using Classifiers");
            Console.WriteLine("Created by:");
            Console.WriteLine("     Anoushka Piplai[1566664]");
            Console.WriteLine("     Avradip Mazumdar[1566651]");
            Console.WriteLine("     Raka Sarkar[1567153]");
            Console.WriteLine("     Somava Ganguly[1566916]\n\n");
            // Starts experiment that demonstrates how to learn spatial patterns and Reconstruct Predicted input images.
            ImageBinarizerSpatialPattern experiment = new ImageBinarizerSpatialPattern();
            experiment.Run();

        }
    }
}