using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using Xunit;

namespace CaptchaGenerator.Test.UnitTests
{
    public class CaptchaTest : Captcha
    {
        public CaptchaTest() : base(new CaptchaOptions()) { }

        [Fact]
        public void ImageSizeTest()
        {
            var width = 10;
            var height = 10;

            // act
            using var img = new Image<Rgba32>(width, height);

            // assert
            Assert.Equal(width, img.Width);
            Assert.Equal(height, img.Height);
        }

        [Fact]
        public void AddNoisePointTest()
        {
            // arrange
            var width = 10;
            var height = 10;
            var pixels = new Rgba32[width * height].AsSpan();
            var noiseColor = Color.Black.ToPixel<Rgba32>();
            var backgroundColor = Color.White.ToPixel<Rgba32>();
            var noiseHistogram = 0;
            var backgroundHistogram = 0;
            _options.NoiseRateColor = new[] { Color.Black };
            using var img = new Image<Rgba32>(width, height);

            // act
            img.Mutate(ctx => ctx.BackgroundColor(backgroundColor));
            AddNoisePoint(img);
            img.CopyPixelDataTo(pixels);
            foreach (var pixel in pixels)
            {
                if (pixel.Equals(noiseColor))
                {
                    noiseHistogram++;
                }
                else if (pixel.Equals(backgroundColor))
                {
                    backgroundHistogram++;
                }
            }

            // assert
            Assert.Equal(1, noiseHistogram);
            Assert.Equal(width * height - 1, backgroundHistogram);
        }

        [Fact]
        public void DrawNoiseLinesTest()
        {
            // arrange
            var width = 10;
            var height = 10;
            var pixels = new Rgba32[width * height].AsSpan();
            var noiseColor = Color.Black.ToPixel<Rgba32>();
            var backgroundColor = Color.White.ToPixel<Rgba32>();
            var noiseHistogram = 0;
            var backgroundHistogram = 0;
            _options.DrawLinesColor = new[] { Color.Black };
            _options.MinLineThickness = 1;
            _options.MaxLineThickness = 1;
            using var img = new Image<Rgba32>(width, height);

            // act
            img.Mutate(ctx => ctx.BackgroundColor(backgroundColor));
            DrawNoiseLines(img);
            img.CopyPixelDataTo(pixels);
            foreach (var pixel in pixels)
            {
                if (pixel.Equals(backgroundColor))
                {
                    backgroundHistogram++;
                }
                else
                {
                    noiseHistogram++;
                }
            }

            // assert
            Assert.True(2 <= noiseHistogram, "noise histogram is less than 2");
            Assert.Equal(width * height - noiseHistogram, backgroundHistogram);
            Assert.True(2 <= backgroundHistogram, "background histogram is less than 2");
        }
    }
}
