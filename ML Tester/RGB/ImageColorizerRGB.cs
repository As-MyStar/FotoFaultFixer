using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using Microsoft.ML;
using ML_Tester.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities = ML_Tester.Common.Utilities;

namespace ML_Tester.RGB
{
    class ImageColorizerRGB
    {
        public ImageColorizerRGB() { }

        public void Colorize(string sourceImagePath, string greyImagePath, string destinationImagePath)
        {
            List<PixelYRGB> trainingData;
            List<PixelYRGB> validationData;

            Console.WriteLine("Processing image data and splitting into Training and Validation Sets");
            Bitmap sourceBMP = (Bitmap)Image.FromFile(sourceImagePath);
            List<PixelYRGB> allData = GeneratePCRDataForColorImage(sourceBMP);
            sourceBMP.Dispose();

            int splitValue = (allData.Count / 100 * 80);
            Shuffle(ref allData);

            trainingData = allData.Take(splitValue).ToList();
            validationData = allData.TakeLast(allData.Count - splitValue).ToList();

            /************/
            /* Training */
            /************/
            MLContext mlContext = new MLContext(seed: 0);
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(trainingData);

            Console.WriteLine("Training Red Channel Model");
            var r_model = MLFunctions.Train(mlContext, trainingDataView, "R");

            Console.WriteLine("Training Green Channel Model");
            var g_model = MLFunctions.Train(mlContext, trainingDataView, "G");

            Console.WriteLine("Training Blue Channel Model");
            var b_model = MLFunctions.Train(mlContext, trainingDataView, "B");

            /**************/
            /* Evaluating */
            /**************/
            IDataView testDataView = mlContext.Data.LoadFromEnumerable(validationData);
            Console.WriteLine("Evaluating Red Channel Model:");
            MLFunctions.Evaluate(mlContext, r_model, testDataView);

            Console.WriteLine("Green Red Channel Model:");
            MLFunctions.Evaluate(mlContext, g_model, testDataView);

            Console.WriteLine("Blue Red Channel Model:");
            MLFunctions.Evaluate(mlContext, b_model, testDataView);

            /*********************************/
            /* Using Model to Predict Colors */
            /*********************************/
            Console.WriteLine("Loading Greyscale image");
            Bitmap bmp2 = (Bitmap)Image.FromFile(greyImagePath);

            List<PixelYRGB> realData = GeneratePCRDataForGreyImage(bmp2);

            CImage greyImage = new CImage(bmp2.Width, bmp2.Height, 24);
            bmp2.Dispose();

            IDataView realDataView = mlContext.Data.LoadFromEnumerable(realData);

            Console.WriteLine("Predicting Red Channel values");
            float[] rData = MLFunctions.MakeColorPredictions(mlContext, r_model, realDataView);

            Console.WriteLine("Predicting Green Channel values");
            float[] gData = MLFunctions.MakeColorPredictions(mlContext, g_model, realDataView);

            Console.WriteLine("Predicting Blue Channel values");
            float[] bData = MLFunctions.MakeColorPredictions(mlContext, b_model, realDataView);

            Console.WriteLine("Populating RGB Values");
            int pixelColorIndex = -1;
            for (int y3 = 0; y3 < greyImage.Height; y3++)
            {
                for (int x3 = 0; x3 < greyImage.Width; x3++)
                {
                    pixelColorIndex += 1;
                    int i3 = (x3 + (greyImage.Width * y3));

                    // Populate RGB with a little data massaging
                    greyImage.Grid[3 * i3 + 2] = Convert.ToByte((int)Math.Min(Math.Round(rData[pixelColorIndex]), 255));
                    greyImage.Grid[3 * i3 + 1] = Convert.ToByte((int)Math.Min(Math.Round(gData[pixelColorIndex]), 255));
                    greyImage.Grid[3 * i3 + 0] = Convert.ToByte((int)Math.Min(Math.Round(bData[pixelColorIndex]), 255));

                }
            }

            /*********************/
            /* Outputing results */
            /*********************/
            Console.WriteLine("Saving Colorized Image");
            Bitmap greyColored = greyImage.ToBitmap();
            greyColored.Save(destinationImagePath);
            greyColored.Dispose();
        }

        private List<PixelYRGB> GeneratePCRDataForColorImage(Bitmap bmp)
        {
            List<PixelYRGB> imageData = new List<PixelYRGB>();
            CImage colorCImg = new CImage(bmp);

            Bitmap greyBMP = Coloring.ConvertToGrayscale(bmp);
            //greyBMP.Save(@"C:\Temp\SamplesImages\color\v3\ColoredConvertedToGrey.jpg");
            CImage greyCImg = new CImage(greyBMP);
            greyBMP.Dispose();

            // Populate LumGrid
            double[][] lumGrid = new double[colorCImg.Height][];
            for (int y = 0; y < bmp.Height; y++)
            {
                lumGrid[y] = new double[colorCImg.Width];
                for (int x = 0; x < bmp.Width; x++)
                {
                    int i = (x + (bmp.Width * y));
                    lumGrid[y][x] = Utilities.CalcLuminance(
                        greyCImg.Grid[3 * i + 2],   // R
                        greyCImg.Grid[3 * i + 1],   // G
                        greyCImg.Grid[3 * i + 0]    // B
                    );
                }
            }

            // Populate imageData
            for (int y2 = 0; y2 < bmp.Height; y2++)
            {
                for (int x2 = 0; x2 < bmp.Width; x2++)
                {
                    // get neighbours
                    double[] neighbours = Utilities.GetCardinalNeighbours(ref lumGrid, x2, y2);
                    double mean = Utilities.GetMeanOfNeighbours(neighbours);
                    int i2 = (x2 + (bmp.Width * y2));

                    var pcr = new PixelYRGB()
                    {
                        Y = lumGrid[y2][x2],
                        NeighbourMeanLuminance = mean,
                        NeighbourLuminanceStandardDeviation = Utilities.GetStandardDeviationofNeighbours(neighbours, mean, false),
                    };

                    pcr.R = colorCImg.Grid[3 * i2 + 2]; // R
                    pcr.G = colorCImg.Grid[3 * i2 + 1]; // G
                    pcr.B = colorCImg.Grid[3 * i2 + 0]; // B                

                    imageData.Add(pcr);
                }
            }

            return imageData;
        }

        private static List<PixelYRGB> GeneratePCRDataForGreyImage(Bitmap bmp)
        {
            List<PixelYRGB> imageData = new List<PixelYRGB>();

            // Really make sure its greyscale
            Bitmap greyBMP = Coloring.ConvertToGrayscale(bmp);
            //greyBMP.Save(@"C:\Temp\SamplesImages\color\v3\GreyConvertedToGrey.jpg");
            CImage greyCImg = new CImage(greyBMP);
            greyBMP.Dispose();

            // Populate LumGrid
            double[][] lumGrid = new double[greyCImg.Height][];
            for (int y = 0; y < bmp.Height; y++)
            {
                lumGrid[y] = new double[greyCImg.Width];
                for (int x = 0; x < bmp.Width; x++)
                {
                    int i = (x + (bmp.Width * y));
                    lumGrid[y][x] = Utilities.CalcLuminance(
                        greyCImg.Grid[3 * i + 2],   // R
                        greyCImg.Grid[3 * i + 1],   // G
                        greyCImg.Grid[3 * i + 0]    // B
                    );
                }
            }

            // Populate imageData
            for (int y2 = 0; y2 < bmp.Height; y2++)
            {
                for (int x2 = 0; x2 < bmp.Width; x2++)
                {
                    // get neighbours
                    double[] neighbours = Utilities.GetCardinalNeighbours(ref lumGrid, x2, y2);
                    double mean = Utilities.GetMeanOfNeighbours(neighbours);

                    var pcr = new PixelYRGB()
                    {
                        Y = lumGrid[y2][x2],
                        NeighbourMeanLuminance = mean,
                        NeighbourLuminanceStandardDeviation = Utilities.GetStandardDeviationofNeighbours(neighbours, mean, false),
                    };

                    imageData.Add(pcr);
                }
            }

            return imageData;
        }

        public void Shuffle(ref List<PixelYRGB> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                PixelYRGB value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
