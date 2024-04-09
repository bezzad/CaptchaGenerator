using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Captcha.Net;

public class Captcha : ICaptchaModule
{
    protected static readonly Random Rand = new Random(DateTime.Now.GetHashCode());
    protected static readonly ConcurrentDictionary<int, Image<Rgba32>> ImagesBuffer = new ConcurrentDictionary<int, Image<Rgba32>>();
    protected static readonly byte CharPadding = (byte)Rand.Next(5, 10);
    protected static float? CharWidth;
    protected static float? LineThickness;
    protected static Color? BackgroundColor;
    protected static Font _font;
    protected static AffineTransformBuilder RotationBuilder;
    protected readonly CaptchaOptions _options;

    public Captcha(CaptchaOptions options)
    {
        _options = options;
        if (_font == null)
        {
            foreach (var name in _options.FontFamilies)
                if (SystemFonts.Collection.TryGet(name, out var fontFamily))
                {
                    _font = fontFamily.CreateFont(_options.FontSize, _options.FontStyle);
                    break;
                }

            if (_font == null)
            {
                var fontFamily = SystemFonts.Families.FirstOrDefault();
                _font = fontFamily.CreateFont(_options.FontSize, _options.FontStyle);
            }
        }

        BackgroundColor ??= _options.BackgroundColor[Rand.Next(0, _options.BackgroundColor.Length)];
        LineThickness ??= Extensions.GenerateNextFloat(_options.MinLineThickness, _options.MaxLineThickness);
        CharWidth ??= TextMeasurer.MeasureAdvance("8", new TextOptions(_font)).Width;
        RotationBuilder ??= GetRotation();
    }

    public byte[] Generate(string text)
    {
        using var imgText = new Image<Rgba32>(_options.Width, _options.Height);
        imgText.Mutate(ctx => DrawText(ctx, _font, text));

        // add the dynamic image to original image
        using var img = GetImageBackground(text.Length);
        img.Mutate(ctx => Draw(ctx, img.Width, img.Height, imgText));

        using var ms = new MemoryStream();
        img.Save(ms, _options.Encoder);
        return ms.ToArray();
    }

    protected Image<Rgba32> GetImageBackground(int textLength)
    {
        return ImagesBuffer.GetOrAdd(textLength, len =>
        {
            var sampleText = "".PadRight(textLength, '8');
            var size = (ushort)TextMeasurer.MeasureAdvance(sampleText, new TextOptions(_font)).Width;
            return new Image<Rgba32>(size + 15, _options.Height);
        }).Clone();
    }

    protected void DrawText(IImageProcessingContext imgContext, Font font, string text)
    {
        var position = 5f;
        imgContext.BackgroundColor(BackgroundColor.Value);

        foreach (char ch in text)
        {
            var location = new PointF(CharPadding + position, Rand.Next(6, Math.Abs(_options.Height - _options.FontSize - 5)));
            imgContext.DrawText(ch.ToString(), font, _options.TextColor[Rand.Next(0, _options.TextColor.Length)], location);
            position += CharWidth.Value;
        }

        // add rotation            
        imgContext.Transform(RotationBuilder);
    }

    protected void Draw(IImageProcessingContext imgContext, int width, int height, Image<Rgba32> imgText)
    {
        imgContext.BackgroundColor(_options.BackgroundColor[Rand.Next(0, _options.BackgroundColor.Length)]);
        imgContext.DrawImage(imgText, 0.80f);

        for (var i = 0; i < _options.DrawLines; i++)
            DrawNoiseLines(imgContext, width, height);

        for (var i = 0; i < _options.NoiseRate; i++)
            AddNoisePoint(imgContext, width, height);

        imgContext.Resize(_options.Width, _options.Height);
    }

    protected void AddNoisePoint(IImageProcessingContext ctx, int width, int height)
    {
        int x0 = Rand.Next(0, width);
        int y0 = Rand.Next(0, height);
        var color = _options.NoiseRateColor[Rand.Next(0, _options.NoiseRateColor.Length)];
        ctx.DrawLine(color, 1F, new PointF[] { new Vector2(x0, y0), new Vector2(x0, y0) });
    }

    protected void DrawNoiseLines(IImageProcessingContext ctx, int width, int height)
    {
        int x0 = Rand.Next(0, Rand.Next(0, Math.Min(30, width)));
        int y0 = Rand.Next(Math.Min(10, height), height);
        int x1 = Rand.Next(width - Rand.Next(0, (int)(width * 0.25)), width);
        int y1 = Rand.Next(0, height);
        var lineColor = _options.DrawLinesColor[Rand.Next(0, _options.DrawLinesColor.Length)];
        ctx.DrawLine(lineColor, LineThickness.Value, new PointF[] { new PointF(x0, y0), new PointF(x1, y1) });
    }

    protected AffineTransformBuilder GetRotation()
    {
        var width = Rand.Next(Math.Min((ushort)10u, _options.Width), _options.Width);
        var height = Rand.Next(Math.Min((ushort)10u, _options.Height), _options.Height);
        var pointF = new PointF(width, height);
        var rotationDegrees = _options.RotationDegree.HasValue && _options.RotationDegree <= _options.MaxRotationDegrees
            ? _options.RotationDegree.Value
            : Rand.Next(0, _options.MaxRotationDegrees);
        var result = GetRotation(rotationDegrees, pointF);
        return result;
    }

    protected AffineTransformBuilder GetRotation(float degrees, Vector2 origin)
    {
        return new AffineTransformBuilder().PrependRotationDegrees(degrees, origin);
    }
}
