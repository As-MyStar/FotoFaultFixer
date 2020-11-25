using System;
using System.Drawing;
using Xunit;
using FotoFaultFixerLib;

namespace FotoFaultFixerLibTest
{
    public class ImageFunctionsTest
    {
        [Fact]
        public void ImpulseNoiseReduction_NullImage_ThrowsException()
        {
            Action act = () => ImageFunctions.ImpulseNoiseReduction_Universal(null, 20, 20);
            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null.", exc.Message);
        }

        [Fact]
        public void ImpulseNoiseReduction_ImageTooSmall()
        {
            Bitmap bmp = new Bitmap(10, 10);
            Action act = () => ImageFunctions.ImpulseNoiseReduction_Universal(bmp, 20, 20);
            ArgumentOutOfRangeException exc = Assert.Throws<ArgumentOutOfRangeException>(act);
            Assert.Equal("Image needs to be greater than 100 x 100 (Parameter 'original')", exc.Message);
        }

        //[Fact]
        //public void ImpulseNoiseReduction_ImageIncorrectType()
        //{

        //}

        //[Fact]
        //public void ImpulseNoiseReduction_ReportsProgressCorrectly()
        //{

        //}
    }
}
