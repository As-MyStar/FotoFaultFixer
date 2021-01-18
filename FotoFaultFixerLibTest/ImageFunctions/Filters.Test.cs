using FotoFaultFixerLib.ImageFunctions;
using FotoFaultFixerLib.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FotoFaultFixerLibTest.ImageFunctions
{
    public class FiltersTest
    {
        [Fact]
        public void ImpulseNoiseReduction_NullImage_ThrowsException()
        {
            Action act = () => Filters.ImpulseNoiseReduction_Universal(null, 20, 20);
            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null.", exc.Message);
        }

        [Fact]
        public void ImpulseNoiseReduction_ImageTooSmall()
        {
            Bitmap bmp = new Bitmap(10, 10);
            CImage cImg = new CImage(bmp);
            Action act = () => Filters.ImpulseNoiseReduction_Universal(cImg, 20, 20);
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
