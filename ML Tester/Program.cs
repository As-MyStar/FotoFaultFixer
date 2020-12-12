using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FotoFaultFixerLib;
using FotoFaultFixerLib.ImageProcessor;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using ML_Tester.Common;
using ML_Tester.RGB;
using ML_Tester.YUV;

namespace ML_Tester
{
    // PG: Various Sources
    // https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/predict-prices
    // https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/machine-learning-model-predictions-ml-net
    // https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/retrain-model-ml-net
    // https://www.researchgate.net/publication/236833384_A_PROPOSED_METHOD_FOR_COLORIZING_GRAYSCALE_IMAGES
    // https://www.cs.huji.ac.il/~yweiss/Colorization/
    // https://www.cs.huji.ac.il/~yweiss/Colorization/colorization-siggraph04.pdf
    class Program
    {        
        static void Main(string[] args)
        {
            string folder = @"C:\Temp\SamplesImages\color\v7\";
            
            // Predict Colors via RGB Method
            //ImageColorizerRGB rgbColorizer = new ImageColorizerRGB();
            //rgbColorizer.Colorize(
            //    folder + "Colored.jpg",
            //    folder + "Greyscale.jpg",
            //    folder + "GreyscaleColoredRGB.jpg"
            //);

            // Predict Colors via YUV Method
            // Better Results
            ImageColorizerYUV yuvColorizer = new ImageColorizerYUV();
            yuvColorizer.Colorize(
                folder + "Colored.jpg",
                folder + "Greyscale.jpg",
                folder + "GreyscaleColoredYUV_FastTree.jpg"
            );   
        }
    }
}
