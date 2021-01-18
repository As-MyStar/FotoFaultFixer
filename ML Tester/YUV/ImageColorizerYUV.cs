using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using Microsoft.ML;
using ML_Tester.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities = ML_Tester.Common.Utilities;

namespace ML_Tester.YUV
{
    class ImageColorizerYUV
    {
        public ImageColorizerYUV() { }

        public void Colorize(string sourceImagePath, string greyImagePath, string destinationImagePath, IProgress<int> progressReporter = null)
        {
            List<PixelYUV> trainingData;
            List<PixelYUV> validationData;

            Console.WriteLine("Processing image data and splitting into Training and Validation Sets");

            Bitmap sourceBMP = (Bitmap)Image.FromFile(sourceImagePath);
            List<PixelYUV> allData = GenerateYUVCRDataForColorImage(sourceBMP);
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

            Console.WriteLine("Training U Channel Model");
            var u_model = MLFunctions.Train(mlContext, trainingDataView, "U");

            Console.WriteLine("Training V Channel Model");
            var v_model = MLFunctions.Train(mlContext, trainingDataView, "V");

            Common.Utilities.SetProgress(progressReporter, 25);

            /**************/
            /* Evaluating */
            /**************/
            IDataView testDataView = mlContext.Data.LoadFromEnumerable(validationData);

            Console.WriteLine("Evaluating U Channel Model:");
            MLFunctions.Evaluate(mlContext, u_model, testDataView);

            Console.WriteLine("Evaluating U Channel Model:");
            MLFunctions.Evaluate(mlContext, v_model, testDataView);

            Common.Utilities.SetProgress(progressReporter, 50);

            /*********************************/
            /* Using Model to Predict Colors */
            /*********************************/
            Console.WriteLine("Loading Greyscale image");
            Bitmap bmp2 = (Bitmap)Image.FromFile(greyImagePath);

            List<PixelYUV> realData = GenerateYUVPCRDataForGreyImage(bmp2);

            CImage greyImage = new CImage(bmp2.Width, bmp2.Height, 24);
            bmp2.Dispose();

            IDataView realDataView = mlContext.Data.LoadFromEnumerable(realData);

            Console.WriteLine("Predicting U Channel values");
            float[] uData = MLFunctions.MakeColorPredictions(mlContext, u_model, realDataView);

            Console.WriteLine("Predicting V Channel values");
            float[] vData = MLFunctions.MakeColorPredictions(mlContext, v_model, realDataView);

            Common.Utilities.SetProgress(progressReporter, 75);

            Console.WriteLine("Populating RGB Values");
            int pixelColorIndex = -1;
            for (int y3 = 0; y3 < greyImage.Height; y3++)
            {
                for (int x3 = 0; x3 < greyImage.Width; x3++)
                {
                    pixelColorIndex += 1;
                    int i3 = (x3 + (greyImage.Width * y3));

                    // Convert our YUV values to RGB
                    var rgb = new YUV(
                        realData[pixelColorIndex].Y,
                        Convert.ToDouble(uData[pixelColorIndex]),
                        Convert.ToDouble(vData[pixelColorIndex])
                    ).ToRGB();

                    greyImage.Grid[3 * i3 + 2] = rgb.R;
                    greyImage.Grid[3 * i3 + 1] = rgb.G;
                    greyImage.Grid[3 * i3 + 0] = rgb.B;

                }
            }

            /*********************/
            /* Outputing results */
            /*********************/
            Console.WriteLine("Saving Colorized Image");
            Bitmap greyColored = greyImage.ToBitmap();
            greyColored.Save(destinationImagePath);
            greyColored.Dispose();

            Common.Utilities.SetProgress(progressReporter, 100);
        }

        public void Shuffle(ref List<PixelYUV> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                PixelYUV value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private List<PixelYUV> GenerateYUVPCRDataForGreyImage(Bitmap bmp)
        {
            List<PixelYUV> imageData = new List<PixelYUV>();

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

                    var yuvpcr = new PixelYUV()
                    {
                        Y = lumGrid[y2][x2],
                        NeighbourMeanLuminance = mean,
                        NeighbourLuminanceStandardDeviation = Utilities.GetStandardDeviationofNeighbours(neighbours, mean, false),
                    };

                    imageData.Add(yuvpcr);
                }
            }

            return imageData;
        }

        private List<PixelYUV> GenerateYUVCRDataForColorImage(Bitmap bmp)
        {
            List<PixelYUV> imageData = new List<PixelYUV>();
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

                    var yuvRow = new PixelYUV()
                    {
                        Y = lumGrid[y2][x2],
                        NeighbourMeanLuminance = mean,
                        NeighbourLuminanceStandardDeviation = Utilities.GetStandardDeviationofNeighbours(neighbours, mean, false),
                    };

                    var tempYUV = new RGB(
                        colorCImg.Grid[3 * i2 + 2],
                        colorCImg.Grid[3 * i2 + 1],
                        colorCImg.Grid[3 * i2 + 0]).ToYUV();

                    yuvRow.U = (float)tempYUV.U;
                    yuvRow.V = (float)tempYUV.V;

                    imageData.Add(yuvRow);
                }
            }

            return imageData;
        }
    }
}
