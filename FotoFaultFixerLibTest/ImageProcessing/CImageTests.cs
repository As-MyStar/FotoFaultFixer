namespace FotoFaultFixerLibTest.ImageProcessing
{
    using FotoFaultFixerLib.ImageProcessing;
    using Moq;
    using System;
    using Xunit;

    public class CImageTests
    {
        private MockRepository mockRepository;

        public CImageTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        // CFR: We should have widht/height and NBytes as test parameters isntead hardcoded.
        
        [Theory]
        [InlineData(640, 360, 1)]
        [InlineData(1280, 720, 1)]
        [InlineData(1920, 1080, 1)]
        [InlineData(640, 360, 3)]
        [InlineData(1280, 720, 3)]
        [InlineData(1920, 1080, 3)]
        public void CTor_WithValidValues(int width, int height, int nBytes)
        {
            CImage image = new CImage(width, height, nBytes);

            Assert.NotNull(image);
            Assert.Equal(image.Grid.Length, (width * height * nBytes));
            this.mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(0, 360, 1)]
        [InlineData(0, 720, 1)]
        [InlineData(0, 1080, 1)]
        [InlineData(0, 360, 3)]
        [InlineData(0, 720, 3)]
        [InlineData(0, 1080, 3)]
        public void CTor_WithInvalidWidth_ShouldThrowException(int width, int height, int nBytes)
        {
            Assert.Throws<ArgumentException>(() => { 
                CImage image = new CImage(width, height, nBytes); 
            });
            this.mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(640, 0, 1)]
        [InlineData(1280, 0, 1)]
        [InlineData(1920, 0, 1)]
        [InlineData(640, 0, 3)]
        [InlineData(1280, 0, 3)]
        [InlineData(1920, 0, 3)]
        public void CTor_WithInvalidHeight_ShouldThrowException(int width, int height, int nBytes)
        {
            Assert.Throws<ArgumentException>(() => {
                CImage image = new CImage(width, height, nBytes);
            });
            this.mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(640, 360, 2)]
        [InlineData(1280, 720, 2)]
        [InlineData(1920, 1080, 2)]
        [InlineData(640, 360, -234)]
        [InlineData(1280, 720, 1000)]
        [InlineData(1920, 1080, 2346245)]
        public void CTor_WithInvalidBytes_ShouldThrowException(int width, int height, int nBytes)
        {
            Assert.Throws<ArgumentException>(() => {
                CImage image = new CImage(width, height, nBytes);
            });
            this.mockRepository.VerifyAll();
        }

        // If we clone, this should be identical
        [Fact]
        public void Clone_ValidateCloneIsEqual()
        {
            //Arrange
            int width = 1920;
            int height = 1080;
            int nBytes = 3;
            CImage image = new CImage(width, height, nBytes);

            // Act
            CImage clonedImage = (CImage)image.Clone();

            // Assert
            Assert.Equal(image.Width, clonedImage.Width);
            Assert.Equal(image.Height, clonedImage.Height);
            Assert.Equal(image.nBytes, clonedImage.nBytes);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void DeleteBit0_RGB_TestRAndGChannels()
        {
            // Arrange
            int width = 1920;
            int height = 1080;
            int nBytes = 3;
            CImage image = new CImage(width, height, nBytes);

            // populate random values
            Random randNum = new Random();
            for (int x = 0; x < image.Grid.Length; x++)
            {
                image.Grid[x] = (byte)randNum.Next(0, 255);
            }

            CImage clonedImage = (CImage)image.Clone();

            // Act
            image.DeleteBit0();

            // Assert
            for (int i = 0; i < (image.Width * image.Height); i++)
            {
                Assert.NotEqual(image.Grid[3 * i + 2], clonedImage.Grid[3 * i + 2]); // R
                Assert.NotEqual(image.Grid[3 * i + 1], clonedImage.Grid[3 * i + 1]); // B
            }
        }
    }
}
