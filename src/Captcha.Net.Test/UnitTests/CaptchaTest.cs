using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System;
using Xunit;

namespace Captcha.Net.Test.UnitTests
{
    public class CaptchaTest : Captcha
    {
        public CaptchaTest() : base(new CaptchaOptions()) { }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 100)]
        [InlineData(200, 100)]
        [InlineData(100, 300)]
        public void ImageSizeTest(int width, int height)
        {
            // act
            using var img = new Image<Rgba32>(width, height);

            // assert
            Assert.Equal(width, img.Width);
            Assert.Equal(height, img.Height);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 100)]
        [InlineData(200, 100)]
        [InlineData(100, 300)]
        public void AddNoisePointTest(int width, int height)
        {
            // arrange
            var pixels = new Rgba32[width * height].AsSpan();
            var noiseColor = Color.Black.ToPixel<Rgba32>();
            var backgroundColor = Color.White.ToPixel<Rgba32>();
            var noiseHistogram = 0;
            var backgroundHistogram = 0;
            _options.NoiseRateColor = new[] { Color.Black };
            using var img = new Image<Rgba32>(width, height);

            // act
            img.Mutate(ctx => ctx.BackgroundColor(backgroundColor));
            img.Mutate(ctx => AddNoisePoint(ctx, width, height));
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

        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 100)]
        [InlineData(200, 100)]
        [InlineData(100, 300)]
        public void DrawNoiseLinesTest(int width, int height)
        {
            // arrange
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
            img.Mutate(ctx => DrawNoiseLines(ctx, width, height));
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


        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 100)]
        public void GetRotationTest(int width, int height)
        {
            // arrange
            var thickness = width % 2 == 0 ? 2 : 3;
            var backgroundColor = Color.White.ToPixel<Rgba32>();
            var lineColor = Color.Black.ToPixel<Rgba32>();
            var middleOffsetOfWidth = width / 2;
            var middleOffsetOfHeight = height / 2;
            var originCenter = new PointF(middleOffsetOfWidth, middleOffsetOfHeight);
            var rotationDegrees = 90f;
            using var img = new Image<Rgba32>(width, height);

            img.Mutate(ctx => ctx.BackgroundColor(backgroundColor));
            img.Mutate(ctx => ctx.DrawLine(lineColor, thickness, new PointF[] { new PointF(middleOffsetOfWidth, 0), new PointF(middleOffsetOfWidth, height) }));

            // act:  rotate 90 degree to a horizontal line
            AffineTransformBuilder rotation = GetRotation(rotationDegrees, new PointF(middleOffsetOfWidth, middleOffsetOfHeight));
            img.Mutate(ctx => ctx.Transform(rotation)); // now the line is vertical
            var middleRowPixels = img.DangerousGetPixelRowMemory(middleOffsetOfWidth).Span;

            // assert
            foreach (var pixel in middleRowPixels)
                Assert.True(pixel.Rgb == lineColor.Rgb, "There is no line color!");
        }
    }
}
