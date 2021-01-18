using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_Tester.Common
{
    static class MLFunctions
    {
        // R, G or B for usage with RGBColorizer
        // OR
        // U or V for usage with YUVCOlorizer
        public static ITransformer Train(MLContext mlContext, IDataView dataView, string label)
        {
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: label)                
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "LuminanceEncoded", inputColumnName: "Y"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NeighbourMeanLuminanceEncoded", inputColumnName: "NeighbourMeanLuminance"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "NeighbourLuminanceStandardDeviationEncoded", inputColumnName: "NeighbourLuminanceStandardDeviation"))
                .Append(mlContext.Transforms.Concatenate("Features", "LuminanceEncoded", "NeighbourMeanLuminanceEncoded", "NeighbourLuminanceStandardDeviationEncoded"))
                .Append(mlContext.Regression.Trainers.FastTree());

            return pipeline.Fit(dataView);
        }

        public static void Evaluate(MLContext mlContext, ITransformer model, IDataView dataView)
        {
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

            Console.WriteLine($"*RSquared Score: {metrics.RSquared:0.##}");
            Console.WriteLine($"*Root Mean Squared Error: {metrics.RootMeanSquaredError:#.##}");
        }

        public static float[] MakeColorPredictions(MLContext mlContext, ITransformer model, IDataView inputData)
        {
            IDataView predictions = model.Transform(inputData);
            float[] data = mlContext.Data.CreateEnumerable<ValuePrediction>(predictions, false).Select(x => x.ColorValue).ToArray();
            return data;
        }
    }
}
