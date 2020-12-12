using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_Tester.RGB
{
    class PixelYRGB
    {
        [LoadColumn(0)]
        public double Y;

        [LoadColumn(1)]
        public double NeighbourMeanLuminance;

        [LoadColumn(2)]
        public double NeighbourLuminanceStandardDeviation;

        [LoadColumn(3)]
        public float R;

        [LoadColumn(4)]
        public float G;

        [LoadColumn(5)]
        public float B;
    }
}
