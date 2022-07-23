namespace FotoFaultFixerLib.ImageProcessing
{
    using System;

    /// <summary>
    /// Computation Image or 'Fast' Image.
    /// </summary>
    /// <remarks>
    /// We need individual pixel access, but .Net's Bitmap implementation is WAY too slow
    /// By storing all data in a single Byte array, its MUCH more efficient and simpler to work with image data
    /// </remarks>
    public class CImage : ICloneable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int nBytes { get; set; }
        public byte[] Grid { get; set; }

        /// <summary>
        /// CTor for CImage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="nBytes">1 for B&W image, 3 for Color/RGB Image</param>
        public CImage(int width, int height, int nBytes)
        {
            if (width <= 0)
            {
                throw new ArgumentException(nameof(width), "width has to be greater than 0");
            }

            if (height <= 0)
            {
                throw new ArgumentException(nameof(height), "height has to be greater than 0");
            }

            if (nBytes != 1 && nBytes != 3)
            {
                throw new ArgumentException(nameof(nBytes), "nBytes has to be either 1 or 3");
            }

            this.Width = width;
            this.Height = height;
            this.nBytes = nBytes;
            this.Grid = new byte[this.Width * this.Height * this.nBytes];
        }

        /// <summary>
        /// Clones this CImage and returns an Identical copy
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new CImage(this.Width, this.Height, this.nBytes)
            {
                Grid = (byte[])this.Grid.Clone()
            };
        }

        /// <summary>
        /// If "this" is a 8 bit image, then sets the bits 0 and 1 of each pixel to 0.
        /// Else it is a 24 bit one, then sets the bit 0 of green and red chanels to 0.
        /// </summary>
        /// <param name="nbyte"></param>
        /// <returns></returns>
        public void DeleteBit0()
        {
            if (this.nBytes == 1)
            {
                for (int i = 0; i < Width * Height; i++)
                {
                    Grid[i] = (byte)(Grid[i] - (Grid[i] % 4));
                }
            }
            else
            {
                for (int i = 0; i < Width * Height; i++)
                {
                    Grid[3 * i + 2] = (byte)(Grid[3 * i + 2] & 254);
                    Grid[3 * i + 1] = (byte)(Grid[3 * i + 1] & 254);
                }
            }
        }
    }
}
