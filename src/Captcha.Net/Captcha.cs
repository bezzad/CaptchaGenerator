using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Numerics;

namespace Captcha.Net
{
    public class Captcha : ICaptchaModule
    {
        private static Random Rand = new Random(DateTime.Now.GetHashCode());
        protected readonly CaptchaOptions _options;

        public Captcha(CaptchaOptions options)
        {
            _options = options;
        }

        public byte[] Generate(string text)
        {
            byte[] result;
            var position = 5f;
            var charPadding = (byte)Rand.Next(5, 10);
            var fontName = _options.FontFamilies[Rand.Next(0, _options.FontFamilies.Length)];
            var font = SystemFonts.CreateFont(fontName, _options.FontSize, _options.FontStyle);
            using var imgText = new Image<Rgba32>(_options.Width, _options.Height);
            imgText.Mutate(ctx => ctx.BackgroundColor(_options.BackgroundColor[Rand.Next(0, _options.BackgroundColor.Length)]));

            foreach (char ch in text)
            {
                var location = new PointF(charPadding + position, Rand.Next(6, Math.Abs(_options.Height - _options.FontSize - 5)));
                imgText.Mutate(ctx => ctx.DrawText(ch.ToString(), font, _options.TextColor[Rand.Next(0, _options.TextColor.Length)], location));
                position += TextMeasurer.Measure(ch.ToString(), new TextOptions(font)).Width;
            }

            // add rotation
            AffineTransformBuilder rotation = GetRotation();
            imgText.Mutate(ctx => ctx.Transform(rotation));

            // add the dynamic image to original image
            var size = (ushort)TextMeasurer.Measure(text, new TextOptions(font)).Width;
            using var img = new Image<Rgba32>(size + 15, _options.Height);
            img.Mutate(ctx => ctx.BackgroundColor(_options.BackgroundColor[Rand.Next(0, _options.BackgroundColor.Length)]));
            img.Mutate(ctx => ctx.DrawImage(imgText, 0.80f));

            for (var i = 0; i < _options.DrawLines; i++)
                DrawNoiseLines(img);

            for (var i = 0; i < _options.NoiseRate; i++)
                AddNoisePoint(img);

            img.Mutate(x => x.Resize(_options.Width, _options.Height));

            using var ms = new MemoryStream();
            img.Save(ms, _options.Encoder);
            result = ms.ToArray();

            return result;
        }

        protected void AddNoisePoint(Image img)
        {
            int x0 = Rand.Next(0, img.Width);
            int y0 = Rand.Next(0, img.Height);
            img.Mutate(ctx => ctx.DrawLines(_options.NoiseRateColor[Rand.Next(0, _options.NoiseRateColor.Length)],
                    1, new PointF[] { new Vector2(x0, y0), new Vector2(x0, y0) }));
        }

        protected void DrawNoiseLines(Image img)
        {
            int x0 = Rand.Next(0, Rand.Next(0, Math.Min(30, img.Width)));
            int y0 = Rand.Next(Math.Min(10, img.Height), img.Height);
            int x1 = Rand.Next(img.Width - Rand.Next(0, (int)(img.Width * 0.25)), img.Width);
            int y1 = Rand.Next(0, img.Height);
            var thickness = Extensions.GenerateNextFloat(_options.MinLineThickness, _options.MaxLineThickness);
            var lineColor = _options.DrawLinesColor[Rand.Next(0, _options.DrawLinesColor.Length)];
            img.Mutate(ctx => ctx.DrawLines(lineColor, thickness,
                         new PointF[] { new PointF(x0, y0), new PointF(x1, y1) }));
        }

        protected AffineTransformBuilder GetRotation()
        {
            var width = Rand.Next(Math.Min((ushort)10, _options.Width), _options.Width);
            var height = Rand.Next(Math.Min((ushort)10, _options.Height), _options.Height);
            var pointF = new PointF(width, height);
            var rotationDegrees = Rand.Next(0, _options.MaxRotationDegrees);
            var result = GetRotation(rotationDegrees, pointF);
            return result;
        }

        protected AffineTransformBuilder GetRotation(float degrees, Vector2 origin)
        {
            return new AffineTransformBuilder().PrependRotationDegrees(degrees, origin);
        }
    }
}
