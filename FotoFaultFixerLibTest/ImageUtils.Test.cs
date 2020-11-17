using System;
using System.Drawing;
using Xunit;
using FotoFaultFixerLib;

namespace FotoFaultFixerLibTest
{
    public class ImageUtilsTest
    {
        [Fact]
        public void ImpulseNoiseReduction_NullImage_ThrowsException()
        {            
            Action act = () => ImageUtils.ImpulseNoiseReduction_Universal(null, 20, 20);
            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null.", exc.Message);
        }

        /*
        public void ImpulseNoiseReduction_ImageTooSmall() { }
        public void ImpulseNoiseReduction_ImageIncorrectType() { }
        public void ImpulseNoiseReduction_ReportsProgressCorrectly() { }
        */
    }
}
