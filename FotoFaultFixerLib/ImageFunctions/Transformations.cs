using FotoFaultFixerLib.ImageProcessing;
using System;
using System.Drawing;

namespace FotoFaultFixerLib.ImageFunctions
{
    // CFR: All of the methods heer assume a 3Byte image and don't handle a 1-bit one well.
    public static class Transformations
    {
        public static CImage FlipVertical(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            CImage verticallyFlipped = new CImage(original.Width, original.Height, original.nBytes);

            int i_orig;
            int i_flipped;
            int maxY = original.Height - 1;
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    i_orig = (x + (original.Width * y));
                    i_flipped = (x + (original.Width * (maxY - y)));
                    verticallyFlipped.Grid[0 + 3 * i_flipped] = original.Grid[0 + 3 * i_orig];
                    verticallyFlipped.Grid[1 + 3 * i_flipped] = original.Grid[1 + 3 * i_orig];
                    verticallyFlipped.Grid[2 + 3 * i_flipped] = original.Grid[2 + 3 * i_orig];
                }
            }

            return verticallyFlipped;
        }

        public static CImage FlipHorizontal(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            CImage horizontallyFlipped = new CImage(original.Width, original.Height, original.nBytes);

            int i_orig;
            int i_flipped;
            int maxX = original.Width - 1;
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    i_orig = (x + (original.Width * y));
                    i_flipped = ((maxX - x) + (original.Width * y));
                    horizontallyFlipped.Grid[0 + 3 * i_flipped] = original.Grid[0 + 3 * i_orig];
                    horizontallyFlipped.Grid[1 + 3 * i_flipped] = original.Grid[1 + 3 * i_orig];
                    horizontallyFlipped.Grid[2 + 3 * i_flipped] = original.Grid[2 + 3 * i_orig];
                }
            }

            return horizontallyFlipped;
        }

        public static CImage RotateCW(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            // Height and width are switched here
            CImage rotatedCW = new CImage(original.Height, original.Width, original.nBytes);

            // TODO: Review - this seems real wonky
            int i_orig;
            int i_rotated;
            int maxY = original.Height - 1;
            int yIdx = -1;
            for (int y = maxY; y >= 0; y--)
            {
                yIdx += 1;
                for (int x = 0; x < original.Width; x++)
                {
                    i_orig = (x + (original.Width * y));
                    i_rotated = yIdx + (rotatedCW.Width * x);
                    rotatedCW.Grid[0 + 3 * i_rotated] = original.Grid[0 + 3 * i_orig];
                    rotatedCW.Grid[1 + 3 * i_rotated] = original.Grid[1 + 3 * i_orig];
                    rotatedCW.Grid[2 + 3 * i_rotated] = original.Grid[2 + 3 * i_orig];
                }
            }

            return rotatedCW;
        }

        public static CImage RotateCCW(CImage original)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            // Height and width are switched here
            CImage rotatedCCW = new CImage(original.Height, original.Width, original.nBytes);

            int i_orig;
            int i_rotated;
            int maxX = original.Width - 1;

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = maxX; x >= 0; x--)
                {
                    i_orig = (x + (original.Width * y));
                    i_rotated = y + (rotatedCCW.Width * (maxX - x));

                    rotatedCCW.Grid[0 + 3 * i_rotated] = original.Grid[0 + 3 * i_orig];
                    rotatedCCW.Grid[1 + 3 * i_rotated] = original.Grid[1 + 3 * i_orig];
                    rotatedCCW.Grid[2 + 3 * i_rotated] = original.Grid[2 + 3 * i_orig];
                }
            }

            return rotatedCCW;
        }

        public static CImage Crop(CImage original, int startX, int startY, int newWidth, int newHeight)
        {
            if (original == null)
            {
                throw new ArgumentNullException();
            }

            if (startX < 0 || startX > original.Width)
            {
                throw new ArgumentException("Startpoint needs to be within image bounds", nameof(startX));
            }

            if (startY < 0 || startY > original.Height)
            {
                throw new ArgumentException("Startpoint needs to be within image bounds", nameof(startY));
            }

            if (newWidth <= 0)
            {
                throw new ArgumentException("Cropped Width needs to be greater than 0", nameof(newWidth));
            }

            if (newHeight <= 0)
            {
                throw new ArgumentException("Cropped Height needs to be greater than 0", nameof(newHeight));
            }

            // Confine cropped width and height to image space
            if ((startX + newWidth) > original.Width)
            {
                newWidth = original.Width - startX;
            }

            if ((startY + newHeight) > original.Height)
            {
                newHeight = original.Height - startY;
            }

            int croppedIdx_X = 0;
            int croppedIdx_Y = 0;
            int originalPixelIdx;
            int croppedPixelIdx;

            CImage cropped = new CImage(newWidth, newHeight, original.nBytes);

            for (int y = startY; y < (startY + newHeight); y++)
            {
                for (int x = startX; x < (startX + newWidth); x++)
                {
                    originalPixelIdx = (x + (original.Width * y));
                    croppedPixelIdx = (croppedIdx_X + (newWidth * croppedIdx_Y));

                    cropped.Grid[0 + 3 * croppedPixelIdx] = original.Grid[0 + 3 * originalPixelIdx];
                    cropped.Grid[1 + 3 * croppedPixelIdx] = original.Grid[1 + 3 * originalPixelIdx];
                    cropped.Grid[2 + 3 * croppedPixelIdx] = original.Grid[2 + 3 * originalPixelIdx];

                    croppedIdx_X += 1;
                }

                croppedIdx_X = 0;
                croppedIdx_Y += 1;
            }

            return cropped;
        }

        public static CImage FourPointStraighten(CImage image, Point[] newCornerPoints, IProgress<int> progressReporter = null)
        {
            if (image == null)
            {
                throw new ArgumentNullException();
            }

            if (newCornerPoints == null)
            {
                throw new ArgumentNullException();
            }

            if (newCornerPoints.Length < 4)
            {
                throw new ArgumentException();
            }

            Utilities.SetProgress(progressReporter, 0);

            image = Rect_Optimal(newCornerPoints, image, progressReporter);

            Utilities.SetProgress(progressReporter, 100);

            return image;
        }

        // Calculates the corners of the rectifyed image and then makes a modified bilinear transformation
        private static CImage Rect_Optimal(Point[] v, CImage source, IProgress<int> progressReporter = null)
        {
            CImage destination = null;

            int width = source.Width;
            int height = source.Height;
            int nBytes = source.nBytes;

            int MaxSize;
            double focalLength,
                Height, Width,
                alphaX, alphaY, 
                betaX, betaY, 
                phiX, phiY,
                RedX, RedY,  
                M0X, M1X, M3Y, M2Y;

            M0X = (double)v[1].X + (v[0].X - v[1].X) * ((double)height / 2 - v[1].Y) / ((double)v[0].Y - v[1].Y);
            M1X = (double)v[2].X + (v[3].X - v[2].X) * ((double)height / 2 - v[2].Y) / ((double)v[3].Y - v[2].Y);
            M3Y = (double)v[0].Y + (v[3].Y - v[0].Y) * ((double)width / 2 - v[0].X) / ((double)v[3].X - v[0].X);
            M2Y = (double)v[1].Y + (v[2].Y - v[1].Y) * ((double)width / 2 - v[1].X) / ((double)v[2].X - v[1].X);

            RedY = (double)(v[3].Y - v[2].Y) / (double)(v[0].Y - v[1].Y);
            RedX = (double)(v[3].X - v[0].X) / (double)(v[2].X - v[1].X);

            if (width > height)
            {
                MaxSize = width;
            }
            else
            {
                MaxSize = height;
            }

            focalLength = 1.0 * (MaxSize);
            alphaY = Math.Atan2(focalLength, (double)(width / 2 - M0X));
            betaY = Math.Atan2(focalLength, (double)(M1X - width / 2));
            phiY = Math.Atan2(RedY * Math.Sin(betaY) - Math.Sin(alphaY), Math.Cos(alphaY) + RedY * Math.Cos(betaY));

            alphaX = Math.Atan2(focalLength, (double)(M3Y - height / 2));
            betaX = Math.Atan2(focalLength, (double)(height / 2 - M2Y));
            phiX = Math.Atan2(RedX * Math.Sin(betaX) - Math.Sin(alphaX), Math.Cos(alphaX) + RedX * Math.Cos(betaX));

            double P0X = focalLength * Math.Cos(alphaY) / Math.Sin(alphaY - phiY);
            double P1X = focalLength * Math.Cos(betaY) / Math.Sin(betaY + phiY);
            double P0Y = focalLength * Math.Cos(alphaX) / Math.Sin(alphaX + phiX);

            Width = focalLength * (Math.Cos(alphaY) / Math.Sin(alphaY - phiY) + Math.Cos(betaY) / Math.Sin(betaY + phiY));
            Height = focalLength * (Math.Cos(alphaX) / Math.Sin(alphaX - phiX) + Math.Cos(betaX) / Math.Sin(betaX + phiX));
            destination = new CImage((int)Width, (int)Height, nBytes);

            double OptCX = 0.0;
            double OptCY = 0.0;
            double CX = Math.Tan(phiY);
            double CY = Math.Tan(phiX);

            Optimization(focalLength, v, width, height, CX, CY, ref OptCX, ref OptCY);
            CX = OptCX;
            CY = OptCY;
            
            double A, B, C, D, Det, E, G;
            double[] xc = new double[4];
            double[] yc = new double[4];
            double[] zc = new double[4];

            for (int i = 0; i < 4; i++)
            {
                //A = B = C = D = 0.0;
                A = (focalLength / (v[i].X - width / 2) + CX);
                B = CY;
                C = width / 2 * focalLength / (v[i].X - width / 2) + CX * width / 2 + CY * height / 2 + focalLength;
                D = CX;
                E = (focalLength / (v[i].Y - height / 2) + CY);
                G = height / 2 * focalLength / (v[i].Y - height / 2) + CX * width / 2 + CY * height / 2 + focalLength;

                Det = A * E - B * D;

                xc[i] = (C * E - B * G) / Det;
                yc[i] = (A * G - C * D) / Det;
                zc[i] = focalLength - CX * (xc[i] - width / 2) - CY * (yc[i] - height / 2); // corrected
            }

            double xp, yp, xp0, xp1, yp0, yp1, xf, yf;
            
            // Loop over the (to be) rectifyed image
            for (int Y = 0; Y < destination.Height; Y++)
            {
                xp0 = xc[1] + (xc[0] - xc[1]) * Y / (destination.Height - 1);
                xp1 = xc[2] + (xc[3] - xc[2]) * Y / (destination.Height - 1);

                for (int X = 0; X < destination.Width; X++)
                {
                    yp0 = yc[1] + (yc[2] - yc[1]) * X / (destination.Width - 1);
                    yp1 = yc[0] + (yc[3] - yc[0]) * X / (destination.Width - 1);

                    xp = xp0 + (xp1 - xp0) * X / (destination.Width - 1);
                    yp = yp0 + (yp1 - yp0) * Y / (destination.Height - 1);

                    xf = width / 2 + (xp - width / 2) * focalLength / (focalLength - CX * (xp - width / 2) - CY * (yp - height / 2));
                    yf = height / 2 + (yp - height / 2) * focalLength / (focalLength - CX * (xp - width / 2) - CY * (yp - height / 2));

                    if ((int)xp >= 0 && (int)xp < width && (int)yp >= 0 && (int)yp < height)
                    {
                        if (nBytes == 24)
                        {
                            for (int ic = 0; ic < 3; ic++)
                            {
                                destination.Grid[ic + 3 * X + 3 * destination.Width * Y] = source.Grid[ic + 3 * (int)xf + 3 * width * (int)yf];
                            }
                        }
                        else
                        {
                            destination.Grid[X + destination.Width * (destination.Height - 1 - Y)] = source.Grid[(int)xf + width * (int)yf];
                        }
                    }        
                } 
            }

            return destination;
        } 

        private static void Optimization(double F, Point[] v, int width, int height, double CX, double CY, ref double OptCX, ref double OptCY)
        {
            double A, B, C, D, Det, E, G;
            
            double[] xc = new double[4];
            double[] yc = new double[4];
            double[] zc = new double[4];

            double[] xopt = new double[4];
            double[] yopt = new double[4];
            double[] zopt = new double[4];

            double dev1, dev2, dev3, Crit;
            double MinCrit = 10000000.0;

            int IterX, IterY;

            OptCX = 0.0;
            OptCY = 0.0;

            CX -= 0.40; CY -= 0.40;
            double CX0 = CX, Step = 0.08;

            for (IterY = 0; IterY < 11; IterY++)
            {
                CX = CX0;

                for (IterX = 0; IterX < 11; IterX++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        A = B = C = D = 0.0;
                        A = (F / (v[i].X - width / 2) + CX);
                        B = CY;
                        C = width / 2 * F / (v[i].X - width / 2) + CX * width / 2 + CY * height / 2 + F;
                        D = CX;
                        E = (F / (v[i].Y - height / 2) + CY);
                        G = height / 2 * F / (v[i].Y - height / 2) + CX * width / 2 + CY * height / 2 + F;

                        Det = A * E - B * D;

                        xc[i] = (C * E - B * G) / Det;
                        yc[i] = (A * G - C * D) / Det;
                        zc[i] = F - CX * (xc[i] - width / 2) - CY * (yc[i] - height / 2);
                    }

                    dev1 = ((xc[0] - xc[1]) * (xc[2] - xc[1]) + (yc[0] - yc[1]) * (yc[2] - yc[1]) + (zc[0] - zc[1]) * (zc[2] - zc[1])) / Math.Sqrt(Math.Pow((xc[0] - xc[1]), 2.0) + Math.Pow((yc[0] - yc[1]), 2.0) + Math.Pow((zc[0] - zc[1]), 2.0));
                    dev2 = Math.Sqrt(Math.Pow((xc[3] - xc[2]), 2.0) + Math.Pow((yc[3] - yc[2]), 2.0) + Math.Pow((zc[3] - zc[2]), 2.0)) - Math.Sqrt(Math.Pow((xc[0] - xc[1]), 2.0) + Math.Pow((yc[0] - yc[1]), 2.0) + Math.Pow((zc[0] - zc[1]), 2.0));
                    dev3 = Math.Sqrt(Math.Pow((xc[2] - xc[1]), 2.0) + Math.Pow((yc[2] - yc[1]), 2.0) + Math.Pow((zc[2] - zc[1]), 2.0)) - Math.Sqrt(Math.Pow((xc[3] - xc[0]), 2.0) + Math.Pow((yc[3] - yc[0]), 2.0) + Math.Pow((zc[3] - zc[0]), 2.0));

                    Crit = Math.Sqrt(Math.Pow(dev1, 2.0) + Math.Pow(dev2, 2.0) + Math.Pow(dev3, 2.0));

                    if (Crit < MinCrit)
                    {
                        MinCrit = Crit;
                        OptCX = CX;
                        OptCY = CY;

                        for (int i = 0; i < 4; i++)
                        {
                            xopt[i] = xc[i];
                            yopt[i] = yc[i];
                            zopt[i] = zc[i];
                        }
                    }

                    CX += Step;
                }

                CY += Step;
            }

            CX = OptCX; CY = OptCY;
            CX -= 0.05; CY -= 0.05;
            CX0 = CX;

            double step = 0.01;
            for (IterY = 0; IterY < 11; IterY++)
            {
                CX = CX0;
                for (IterX = 0; IterX < 11; IterX++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        A = (F / (v[i].X - width / 2) + CX); // all corrected
                        B = CY;
                        C = width / 2 * F / (v[i].X - width / 2) + CX * width / 2 + CY * height / 2 + F;
                        D = CX;
                        E = (F / (v[i].Y - height / 2) + CY);
                        G = height / 2 * F / (v[i].Y - height / 2) + CX * width / 2 + CY * height / 2 + F;

                        Det = A * E - B * D;

                        xc[i] = (C * E - B * G) / Det;
                        yc[i] = (A * G - C * D) / Det;
                        zc[i] = F - CX * (xc[i] - width / 2) - CY * (yc[i] - height / 2);
                    }

                    // deviation from a 90° angle:
                    dev1 = ((xc[0] - xc[1]) * (xc[2] - xc[1]) + (yc[0] - yc[1]) * (yc[2] - yc[1]) + (zc[0] - zc[1]) * (zc[2] - zc[1])) / Math.Sqrt(Math.Pow((xc[0] - xc[1]), 2.0) + Math.Pow((yc[0] - yc[1]), 2.0) + Math.Pow((zc[0] - zc[1]), 2.0));

                    // difference |pc[3] - pc[2]| - |pc[0] - pc[1]|:
                    dev2 = Math.Sqrt(Math.Pow((xc[3] - xc[2]), 2.0) + Math.Pow((yc[3] - yc[2]), 2.0) + Math.Pow((zc[3] - zc[2]), 2.0)) - Math.Sqrt(Math.Pow((xc[0] - xc[1]), 2.0) + Math.Pow((yc[0] - yc[1]), 2.0) + Math.Pow((zc[0] - zc[1]), 2.0));

                    // difference |pc[2] - pc[1]| - |pc[3] - pc[0]|:
                    dev3 = Math.Sqrt(Math.Pow((xc[2] - xc[1]), 2.0) + Math.Pow((yc[2] - yc[1]), 2.0) + Math.Pow((zc[2] - zc[1]), 2.0)) - Math.Sqrt(Math.Pow((xc[3] - xc[0]), 2.0) + Math.Pow((yc[3] - yc[0]), 2.0) + Math.Pow((zc[3] - zc[0]), 2.0));

                    Crit = Math.Sqrt(Math.Pow(dev1, 2.0) + Math.Pow(dev2, 2.0) + Math.Pow(dev3, 2.0));

                    if (Crit < MinCrit)
                    {
                        MinCrit = Crit;
                        OptCX = CX;
                        OptCY = CY;

                        for (int i = 0; i < 4; i++)
                        {
                            xopt[i] = xc[i];
                            yopt[i] = yc[i];
                            zopt[i] = zc[i];
                        }
                    }

                    CX += step;
                }

                CY += step;
            }
        }
    }
}
