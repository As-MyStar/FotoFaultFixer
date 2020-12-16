using System;
using System.Drawing;
using FotoFaultFixerLib.ImageFunctions;

namespace FotoFaultFixerLib.ImageProcessing
{
    class CPnoise
    {
        private int[][] _index;  // saving all pixels of the image ordered by lightness
        private int[] _comp;     // contains indices of pixels of a connected component
        private int[] _nPixel;   // number of pixels with certain lightness in the image
        private int _maxSize;    // admissible size of a component
        private Point[] _v;
        private CQueue _cQueue;  // Custom Queue iemplmentation

        public CPnoise(int[] histo, int Qlength, int Size)
        {
            _maxSize = Size;
            _cQueue = new CQueue(Qlength);   // necessary to find connected components
            _comp = new int[_maxSize];
            _nPixel = new int[256];          // 256 is the number of lightness values
            _v = new Point[20];
            _index = new int[256][];

            for (int lightValue = 0; lightValue < 256; lightValue++)
            {
                _nPixel[lightValue] = 0;
                _index[lightValue] = new int[histo[lightValue] + 2];

                for (int light1 = 0; light1 < histo[lightValue] + 1; light1++)
                {
                    _index[lightValue][light1] = 0;
                }
            }
        }

        public void NoiseFilter(ref CImage image, int[] histo, int minLight, int maxLight, int maxSizeD, int maxSizeL, IProgress<int> progress = null)
        {
            bool isColorImage = (image.NBits == 24);
            if (isColorImage)
            {
                Sort_Color(image, histo);
                DarkNoise_Color(ref image, minLight, maxLight, maxSizeD);
                image.DeleteBit0();
                LightNoise_Color(ref image, minLight, maxSizeL);
            }
            else
            {
                Sort_Greyscale(image, histo);
                DarkNoise_Greyscale(ref image, minLight, maxLight, maxSizeD);
                image.DeleteBit0();
                LightNoise_Greyscale(ref image, minLight, maxSizeL);
            }
        }

        private void Sort_Color(CImage Image, int[] histo)
        {
            int lightValue, 
                pixelIdx;
            
            for (int y = 0; y < Image.Height; y++)
            {
                for (int x = 0; x < Image.Width; x++)
                {                    
                    pixelIdx = x + y * Image.Width;

                    lightValue = Utilities.MaxC(
                        Image.Grid[3 * pixelIdx + 2] & 254,
                        Image.Grid[3 * pixelIdx + 1] & 254,
                        Image.Grid[3 * pixelIdx + 0] & 254
                    );

                    lightValue = Math.Max(lightValue, 0);
                    lightValue = Math.Min(lightValue, 255);

                    _index[lightValue][_nPixel[lightValue]] = pixelIdx; // record of pixel with lightness "light"
                    if (_nPixel[lightValue] < histo[lightValue])
                    {
                        _nPixel[lightValue]++;
                    }
                }
            }
        }

        private void Sort_Greyscale(CImage Image, int[] histo)
        {
            int lightValue,
                pixelIdx;

            for (int y = 0; y < Image.Height; y++)
            {
                for (int x = 0; x < Image.Width; x++)
                {
                    pixelIdx = x + y * Image.Width;
                    lightValue = Image.Grid[pixelIdx] & 252;

                    lightValue = Math.Max(lightValue, 0);
                    lightValue = Math.Min(lightValue, 255);

                    _index[lightValue][_nPixel[lightValue]] = pixelIdx; // record of pixel with lightness "light"
                    if (_nPixel[lightValue] < histo[lightValue])
                    {
                        _nPixel[lightValue]++;
                    }
                }
            }
        }

        // Returns the index of the nth neighboor of the pixel W. If the neighboor
        // is outside the grid, then it returns -1.
        private int Neighb(CImage Image, int W, int n)
        {
            int dx, dy, xn, yn;
            if (n == 4)
            {
                return -1; // "n==4" means Neigb==W
            }

            yn = W / Image.Width;
            xn = W % Image.Width;

            dx = (n % 3) - 1;
            dy = (n / 3) - 1;

            xn += dx;
            yn += dy;

            if (xn < 0 || xn >= Image.Width || yn < 0 || yn >= Image.Height)
            {
                return -2;
            }

            return xn + Image.Width * yn;
        }

        /* Looks for pixels with lightness <= light composing with the pixel "Index[light][i]"
           an 8-connected subset. The size of the subset must be less than "maxSize".
           Instead of labeling the pixels of the subset, indices of pixels of the subset are saved in Comp.
           Variable "index" is the index of the starting pixel in Index[light][i];
           Pixels which are put into queue and into Comp[] are labeled in "Image.Grid(green)" by setting Bit 0 to 1.
           Pixels which belong to a too big component and having the gray value equal to "light" are
           labeled in "Image.Grid(red)" by setting Bit 0 to 1. If such a labeled pixel is found in the while loop 
           then "small" is set to 0. The instruction for breaking the loop is at the end of the loop. --*/
        private int BreadthFirst_D_Greyscale(ref CImage Image, int i, int light, int maxSize)
        {
            int lightNeb,   // lightness of the neighbor
                index, 
                LabelQ1, 
                LabelBig2, 
                maxNeib,    // maxNeib is the maximum number of neighbors of a pixel
                Neib,       // the index of a neighbor
                nextIndex,  // index of the next pixel in the queue
                numbPix;    // number of pixel indices in "Comp"

            bool small;

            index = _index[light][i];

            // color of a pixel with minimum lightness among pixels near the subset
            int[] MinBound = new int[3] { 300, 300, 300 };

            for (int p = 0; p < _maxSize; p++)
            {
                _comp[p] = -1; // MaxSize is element of class CPnoise
            }

            numbPix = 0;
            maxNeib = 8; // maximum number of neighbors
            small = true;
            _comp[numbPix] = index;
            numbPix++;

            Image.Grid[index] |= 1; // Labeling as in Comp            

            _cQueue.Reset();
            _cQueue.Put(index); // putting index into the queue

            while (!_cQueue.IsEmpty()) 
            {
                nextIndex = _cQueue.Get();
                for (int n = 0; n <= maxNeib; n++) 
                {
                    Neib = Neighb(Image, nextIndex, n); // the index of the nth neighbor of nextIndex 
                    if (Neib < 0)
                    {
                        continue; // Neib<0 means outside the image
                    }

                    LabelQ1 = Image.Grid[Neib] & 1;
                    LabelBig2 = Image.Grid[Neib] & 2;
                    lightNeb = Image.Grid[Neib] & 252; // MaskGV;                    

                    if (lightNeb == light && LabelBig2 > 0)
                    {
                        small = false;
                    }

                    if (lightNeb <= light) 
                    {
                        if (LabelQ1 > 0)
                        {
                            continue;
                        }

                        _comp[numbPix] = Neib; // putting the element with index Neib into Comp
                        numbPix++;

                        Image.Grid[Neib] |= 1; // Labeling with "1" as in Comp                         

                        if (numbPix > maxSize)
                        {
                            small = false;
                            break;
                        }

                        _cQueue.Put(Neib);
                    }
                    else // lightNeb < light
                    {
                        if (Neib != index)
                        {
                            if (lightNeb < MinBound[0])
                            {
                                MinBound[0] = lightNeb;
                            }
                        }
                    }
                }

                if (small == false)
                {
                    break;
                }
            }

            // Deleting 
            int lightComp; // lightness of a pixel whose index is contained in "Comp"
            for (int m = 0; m < numbPix; m++)
            {
                if (small && MinBound[0] < 300) // "300" means MinBound was not calculated
                {
                    Image.Grid[_comp[m]] = (byte)MinBound[0];                    
                }
                else
                {
                    lightComp = Image.Grid[_comp[m]] & 252; // MaskGV;                    

                    if (lightComp == light)
                    {
                        Image.Grid[_comp[m]] |= 2;                        
                    }
                    else // lightComp!=light
                    {
                        Image.Grid[_comp[m]] &= 252;
                    }
                }
            }

            return numbPix;
        }

        private int BreadthFirst_D_Color(ref CImage Image, int i, int light, int maxSize)
        {
            int lightNeb, // lightness of the neighbor
                index, 
                LabelQ1, 
                LabelBig2, 
                maxNeib, // maxNeib is the maximum number of neighbors of a pixel
                Neib, // the index of a neighbor
                nextIndex, // index of the next pixel in the queue
                numbPix; // number of pixel indices in "Comp"

            bool small;
            index = _index[light][i];

            // color of a pixel with minimum lightness among pixels near the subset
            int[] MinBound = new int[3] { 300, 300, 300 };

            for (int p = 0; p < _maxSize; p++)
            {
                _comp[p] = -1; // MaxSize is element of class CPnoise
            }

            numbPix = 0;
            maxNeib = 8; // maximum number of neighbors
            small = true;
            _comp[numbPix] = index;
            numbPix++;

            Image.Grid[1 + 3 * index] |= 1; // Labeling as in Comp (LabelQ1)

            _cQueue.Reset();
            _cQueue.Put(index); // putting index into the queue

            while (!_cQueue.IsEmpty()) 
            {
                nextIndex = _cQueue.Get();
                for (int n = 0; n <= maxNeib; n++) 
                {
                    Neib = Neighb(Image, nextIndex, n); // the index of the nth neighbor of nextIndex 
                    if (Neib < 0)
                    {
                        continue; // Neib<0 means outside the image
                    }

                    LabelQ1 = Image.Grid[1 + 3 * Neib] & 1;
                    LabelBig2 = Image.Grid[2 + 3 * Neib] & 1;

                    lightNeb = Utilities.MaxC(
                        Image.Grid[2 + 3 * Neib],
                        Image.Grid[1 + 3 * Neib],
                        Image.Grid[0 + 3 * Neib]
                    ) & 254; // MaskColor;                    

                    if (lightNeb == light && LabelBig2 > 0)
                    {
                        small = false;
                    }

                    if (lightNeb <= light) 
                    {
                        if (LabelQ1 > 0)
                        {
                            continue;
                        }

                        _comp[numbPix] = Neib; // putting the element with index Neib into Comp
                        numbPix++;

                        Image.Grid[1 + 3 * Neib] |= 1; // Labeling with "1" as in Comp                         

                        if (numbPix > maxSize)
                        {
                            small = false;
                            break;
                        }

                        _cQueue.Put(Neib);
                    }
                    else // lightNeb < light
                    {
                        if (Neib != index) 
                        {
                            if (lightNeb < Utilities.MaxC(MinBound[2], MinBound[1], MinBound[0]))
                            {
                                for (int c = 0; c < 3; c++)
                                {
                                    MinBound[c] = Image.Grid[c + 3 * Neib];
                                }
                            }
                        }
                    }
                }

                if (small == false)
                {
                    break;
                }
            }

            // Deleting 
            int lightComp; // lightness of a pixel whose index is contained in "Comp"
            for (int m = 0; m < numbPix; m++)
            {
                if (small && MinBound[0] < 300) //--"300" means MinBound was not calculated ---
                {
                    for (int c = 0; c < 3; c++)
                    {
                        Image.Grid[c + 3 * _comp[m]] = (byte)MinBound[c];
                    }
                }
                else
                {
                    lightComp = Utilities.MaxC(
                        Image.Grid[2 + 3 * _comp[m]],
                        Image.Grid[1 + 3 * _comp[m]],
                        Image.Grid[0 + 3 * _comp[m]]
                    ) & 254;

                    if (lightComp == light)
                    {
                        Image.Grid[2 + 3 * _comp[m]] |= 1; // setting label 2
                    }
                    else // lightComp!=light
                    {
                        Image.Grid[1 + 3 * _comp[m]] &= (byte)254; // deleting label 1
                        Image.Grid[2 + 3 * _comp[m]] &= (byte)254; // deleting label 2
                    }
                }
            }

            return numbPix;
        }

        private int DarkNoise_Greyscale(ref CImage Image, int minLight, int maxLight, int maxSize)
        {
            int LabelBig2, 
                Lum, 
                rv = 0;

            if (maxSize == 0)
            {
                return 0;
            }

            for (int light = maxLight - 2; light >= minLight; light--)
            {
                for (int i = 0; i < _nPixel[light]; i++)
                {                                        
                    LabelBig2 = Image.Grid[_index[light][i]] & 2;
                    Lum = Image.Grid[_index[light][i]] & 252;

                    if (Lum == light && LabelBig2 == 0)
                    {
                        rv = BreadthFirst_D_Greyscale(ref Image, i, light, maxSize);

                        if (rv < 0)
                        {
                            return -1;
                        }
                    }
                }
            }

            return rv;
        }
        private int DarkNoise_Color(ref CImage Image, int minLight, int maxLight, int maxSize)
        {
            int ind3,       // index multiplied with 3
                LabelBig2, 
                Lum, 
                rv = 0;

            if (maxSize == 0)
            {
                return 0;
            }

            for (int light = maxLight - 2; light >= minLight; light--)
            {
                for (int i = 0; i < _nPixel[light]; i++)
                {
                    ind3 = 3 * _index[light][i];                    
                    LabelBig2 = Image.Grid[2 + ind3] & 1;
                    Lum = Utilities.MaxC(Image.Grid[2 + ind3], Image.Grid[1 + ind3], Image.Grid[0 + ind3]) & 254;
                    
                    if (Lum == light && LabelBig2 == 0)
                    {
                        rv = BreadthFirst_D_Color(ref Image, i, light, maxSize);
                        if (rv < 0)
                        {
                            return -1;
                        }
                    }
                }
            }

            return rv;
        }

        private int LightNoise_Greyscale(ref CImage Image, int minLight, int maxSize)
        {
            int LabelBig2, 
                Lum, 
                rv = 0;

            if (maxSize == 0)
            {
                return 0;
            }

            for (int light = minLight; light <= 255; light++)
            {
                for (int i = 0; i <= _nPixel[light]; i++)
                {
                    LabelBig2 = Image.Grid[_index[light][i]] & 2;
                    Lum = Image.Grid[_index[light][i]];                    

                    if (Lum == light && LabelBig2 == 0)
                    {
                        rv = BreadthFirst_L_Greyscale(ref Image, i, light, maxSize);
                    }
                }
            }

            return rv;
        }
        private int LightNoise_Color(ref CImage Image, int minLight, int maxSize)
        {
            int ind3,       // index multiplied with 3
                LabelBig2, 
                Lum, 
                rv = 0;

            if (maxSize == 0)
            {
                return 0;
            }

            for (int light = minLight; light <= 255; light++) 
            {
                for (int i = 0; i <= _nPixel[light]; i++) 
                {
                    ind3 = 3 * _index[light][i];

                    LabelBig2 = Image.Grid[2 + 3 * _index[light][i]] & 1;
                    Lum = Utilities.MaxC(
                        Image.Grid[2 + ind3], 
                        Image.Grid[1 + ind3], 
                        Image.Grid[0 + ind3]
                    ) & 254;
                    
                    if (Lum == light && LabelBig2 == 0)
                    {
                        rv = BreadthFirst_L_Color(ref Image, i, light, maxSize);
                    }
                }
            }

            return rv;
        }

        private int BreadthFirst_L_Greyscale(ref CImage Image, int i, int light, int maxSize)
        {
            int lightNeb, 
                index, 
                LabelQ1, 
                LabelBig2, 
                MaskBri = 252, 
                maxNeib = 8, 
                Neib, 
                nextIndex;
            bool small = true;
            int[] MaxBound = new int[3] { -255, -255, -255 };

            index = _index[light][i];

            for (int p = 0; p < _maxSize; p++)
            {
                _comp[p] = -1;
            }

            int numbPix = 0;
            _comp[numbPix] = index;
            numbPix++;

            Image.Grid[index] |= 1; // Labeling as in Comp            

            _cQueue.Reset();
            _cQueue.Put(index); // putting index into the queue

            while (!_cQueue.IsEmpty())
            {
                nextIndex = _cQueue.Get();
                for (int n = 0; n <= maxNeib; n++)
                {
                    Neib = Neighb(Image, nextIndex, n); // the index of the nth neighbor of nextIndex 
                    if (Neib < 0)
                    {
                        continue; // Neib<0 means outside the image
                    }

                    LabelQ1 = Image.Grid[Neib] & 1;
                    LabelBig2 = Image.Grid[Neib] & 2;
                    lightNeb = Image.Grid[Neib] & MaskBri;

                    if (lightNeb == light && LabelBig2 > 0)
                    {
                        small = false;
                    }

                    if (lightNeb >= light)
                    {
                        if (LabelQ1 > 0)
                        {
                            continue;
                        }

                        _comp[numbPix] = Neib; // putting the element with index Neib into Comp

                        numbPix++;

                        Image.Grid[Neib] |= 1; // Labeling with "1" as in Comp                         

                        if (numbPix > maxSize)
                        {
                            small = false;
                            break;
                        }

                        _cQueue.Put(Neib);
                    }
                    else // lightNeb < light
                    {
                        if (Neib != index)
                        {
                            if (lightNeb > MaxBound[0])
                            {
                                MaxBound[0] = lightNeb;
                            }
                        }
                    }
                }

                if (small == false)
                {
                    break;
                }
            }

            int lightComp,      // lightness of a pixel whose index is contained in "Comp"
                nChanged = 0;   // number of pixels whose lightness was changed

            for (int m = 0; m < numbPix; m++)
            {
                if (small == true && MaxBound[0] >= 0)
                {
                    Image.Grid[_comp[m]] = (byte)MaxBound[0];
                    nChanged++;
                }
                else
                {
                    lightComp = Image.Grid[_comp[m]] & MaskBri;

                    if (lightComp == light)
                    {
                        Image.Grid[_comp[m]] |= 2;
                    }
                    else
                    {
                        Image.Grid[_comp[m]] &= (byte)MaskBri; // deleting the labels
                    }
                }
            }

            return nChanged; // numbPix;
        }

        // Looks for pixels with gray values >=light composing with the pixel "Index[light][i]"
        // an 8-connected subset. The size of the subset must be less than "maxSize".
        // Instead of labeling the pixels of the subset, indices of pixels of the subset are saved in Comp.
        // Variable "i" is the index of the starting pixel in Index[light][i];
        // Pixels which are put into queue and into Comp[] are labeled in "Image.Grid(green)" by setting Bit 0 to 1.
        // Pixels wich belong to a too big component and having the gray value equal to "light" are
        // labeled in "Image.Grid(red)" by setting Bit 0 to 1. If such a labeled pixel is found in the while loop
        // then "small" is set to 0. The insruction for breaking the loop is at the end of the loop. 
        private int BreadthFirst_L_Color(ref CImage Image, int i, int light, int maxSize)
        {
            int lightNeb, 
                index, 
                LabelQ1, 
                LabelBig2, 
                MaskColor = 254, 
                maxNeib = 8, 
                Neib, 
                nextIndex;
            bool small = true;
            int[] MaxBound = new int[3] { -255, -255, -255 };

            index = _index[light][i];

            for (int p = 0; p < _maxSize; p++)
            {
                _comp[p] = -1;
            }

            int numbPix = 0;
            _comp[numbPix] = index;
            numbPix++;

            Image.Grid[1 + 3 * index] |= 1; // Labeling as in Comp

            _cQueue.Reset();
            _cQueue.Put(index); // putting index into the queue

            while (!_cQueue.IsEmpty()) 
            {
                nextIndex = _cQueue.Get();
                for (int n = 0; n <= maxNeib; n++) 
                {
                    Neib = Neighb(Image, nextIndex, n); // the index of the nth neighbor of nextIndex 
                    if (Neib < 0)
                    {
                        continue; // Neib<0 means outside the image
                    }

                    LabelQ1 = Image.Grid[1 + 3 * Neib] & 1;
                    LabelBig2 = Image.Grid[2 + 3 * Neib] & 1;

                    lightNeb = Utilities.MaxC(
                        Image.Grid[2 + 3 * Neib],
                        Image.Grid[1 + 3 * Neib],
                        Image.Grid[0 + 3 * Neib]
                    ) & MaskColor;

                    if (lightNeb == light && LabelBig2 > 0)
                    {
                        small = false;
                    }

                    if (lightNeb >= light)
                    {
                        if (LabelQ1 > 0)
                        {
                            continue;
                        }

                        _comp[numbPix] = Neib; // putting the element with index Neib into Comp

                        numbPix++;

                        Image.Grid[1 + 3 * Neib] |= 1; // Labeling with "1" as in Comp 

                        if (numbPix > maxSize)
                        {
                            small = false;
                            break;
                        }

                        _cQueue.Put(Neib);
                    }
                    else // lightNeb < light
                    {
                        if (Neib != index)
                        {
                            if (lightNeb > Utilities.MaxC(MaxBound[2], MaxBound[1], MaxBound[0]))
                            {
                                MaxBound[0] = (Image.Grid[0 + 3 * Neib] & MaskColor);
                                MaxBound[1] = (Image.Grid[1 + 3 * Neib] & MaskColor);
                                MaxBound[2] = (Image.Grid[2 + 3 * Neib] & MaskColor);
                            }
                        }
                    }
                }

                if (small == false)
                {
                    break;
                }
            }

            int lightComp, // lightness of a pixel whose index is contained in "Comp"
                nChanged = 0; // number of pixels whose lightness was changed

            for (int m = 0; m < numbPix; m++)
            {
                if (small == true && MaxBound[0] >= 0)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        Image.Grid[c + 3 * _comp[m]] = (byte)MaxBound[c];
                    }
                }
                else
                {
                    lightComp = Utilities.MaxC(
                        Image.Grid[2 + 3 * _comp[m]],
                        Image.Grid[1 + 3 * _comp[m]],
                        Image.Grid[0 + 3 * _comp[m]]
                    ) & MaskColor;

                    if (lightComp == light)
                    {
                        Image.Grid[2 + 3 * _comp[m]] |= 1;
                    }
                    else
                    {
                        Image.Grid[1 + 3 * _comp[m]] &= (byte)MaskColor; // deleting label 1
                        Image.Grid[2 + 3 * _comp[m]] &= (byte)MaskColor; // deleting label 2
                    }
                }
            }

            return nChanged; // numbPix;
        }

        // Calculates bounds of the rectangle defined by global "fm1.v" and returns the condition
        // that the point (x, y) lies inside the rectangle.
        private bool getCond(int i, int x, int y, double marginX, double marginY, double Scale)
        {
            double fxmin = (_v[i].X - marginX) / Scale; // "marginX" is the space of pictureBox1 left of image (may be 0)
            int xmin = (int)fxmin;

            double fxmax = (_v[i + 1].X - marginX) / Scale; // Scale is the scale of the presentation of image
            int xmax = (int)fxmax;

            double fymin = (_v[i].Y - marginY) / Scale; // "marginY" is the space of pictureBox1 above the image  (may be 0)
            int ymin = (int)fymin;

            double fymax = (_v[i + 1].Y - marginY) / Scale;
            int ymax = (int)fymax;

            bool Condition = (y >= ymin && y <= ymax && x >= xmin && x <= xmax);
            return Condition;
        }
    }
}
