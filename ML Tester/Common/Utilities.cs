using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_Tester.Common
{
    static class Utilities
    {
        public static double CalcLuminance(byte r, byte g, byte b)
        {
            return r * .299000 + g * .587000 + b * .114000;
        }

        public static double[] GetCardinalNeighbours(ref double[][] lumGrid, int x, int y)
        {
            List<double> cardinalNeighbors = new List<double>();

            bool west = (x - 1 >= 0);
            bool east = (x + 1) < lumGrid[y].Length;
            bool south = (y - 1) >= 0;
            bool north = (y + 1) < lumGrid.Length;

            if (west)
            {
                cardinalNeighbors.Add(lumGrid[y][x - 1]);

                if (north)
                {
                    cardinalNeighbors.Add(lumGrid[y + 1][x - 1]);
                }

                if (south)
                {
                    cardinalNeighbors.Add(lumGrid[y - 1][x - 1]);
                }
            }

            if (east)
            {
                cardinalNeighbors.Add(lumGrid[y][x + 1]);

                if (north)
                {
                    cardinalNeighbors.Add(lumGrid[y + 1][x + 1]);
                }

                if (south)
                {
                    cardinalNeighbors.Add(lumGrid[y - 1][x + 1]);
                }
            }

            if (north)
            {
                cardinalNeighbors.Add(lumGrid[y + 1][x]);
            }

            if (south)
            {
                cardinalNeighbors.Add(lumGrid[y - 1][x]);
            }

            return cardinalNeighbors.ToArray();
        }
        public static double GetMeanOfNeighbours(IEnumerable<double> neighbours)
        {
            double mean = 0;

            if (neighbours != null && neighbours.Count() != 0)
            {
                mean = neighbours.Sum() / neighbours.Count();
            }

            return mean;
        }

        // Source: http://csharphelper.com/blog/2015/12/make-an-extension-method-that-calculates-standard-deviation-in-c/#:~:text=The%20standard%20deviation%20is%20defined,each%20value%20and%20the%20mean.
        public static double GetStandardDeviationofNeighbours(IEnumerable<double> neighbours, double mean, bool asSample = true)
        {
            var squares_query =
                from double value in neighbours
                select (value - mean) * (value - mean);

            double sum_of_squares = squares_query.Sum();

            if (asSample)
            {
                return Math.Sqrt(sum_of_squares / (neighbours.Count() - 1));
            }
            else
            {
                return Math.Sqrt(sum_of_squares / neighbours.Count());
            }
        }
    }
}
