using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Linq;
using Xunit;

namespace Captcha.Net.Test.IntegrationTests
{
    public class CaptchaGeneratorTest
    {
        [Fact]
        public void GenerateCaptchaCodeWithPureNumericalLettersTest()
        {
            // arrange
            var codeLetters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var generator = new CaptchaGenerator();

            // act
            var code = generator.GenerateCaptchaCode(8);

            // assert
            Assert.Equal(8, code.Length);
            Assert.True(code.Select(k => codeLetters.Contains(k)).All(i => i));
        }

        [Fact]
        public void GenerateCaptchaImageTest()
        {
            // arrange
            ushort width = 100;
            ushort height = 50;
            var code = "012340";
            var generator = new CaptchaGenerator();
            var backgroundHistogram = 0;

            // act
            var captcha = generator.GenerateCaptchaImage(width, height, code);
            var image = Image.Load<Rgba32>(captcha.CaptchaByteData);
            var pixels = image.GetPixelMemoryGroup().SelectMany(g => g.ToArray());
            foreach (var pixel in pixels)
            {
                if (pixel == Color.White.ToPixel<Rgba32>())
                    backgroundHistogram++;
            }

            // assert
            Assert.NotNull(captcha);
            Assert.NotNull(captcha.CaptchaByteData);
            Assert.NotNull(captcha.CaptchBase64Data);
            Assert.Equal(code, captcha.CaptchaCode);
            Assert.Equal(width, image.Width);
            Assert.Equal(height, image.Height);
            Assert.True(width * height * 0.2 < backgroundHistogram);
        }
    }
}
