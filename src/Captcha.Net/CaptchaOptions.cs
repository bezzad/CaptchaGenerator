using System.Runtime.InteropServices;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace Captcha.Net
{
    public class CaptchaOptions
    {
        public CaptchaOptions()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                FontFamilies = new[] { "DejaVu Serif", "DejaVu Sans Mono", "Liberation Sans" };
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                FontFamilies = new[] { "San Francisco", "Helvetica" };
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                FontFamilies = new[] { "Microsoft Sans Serif", "Arial", "Verdana", "Times New Roman" };
        }

        /// <summary>
        /// Default fonts are  "Arial", "Verdana", "Times New Roman" in Windows
        /// Linux guys have to set thier own fonts :)
        /// </summary>
        public string[] FontFamilies { get; set; }

        public Color[] TextColor { get; set; } =
        {
            Color.Blue, Color.Black, Color.Black, Color.DarkGray, Color.Brown, Color.Gray, Color.Gray, Color.Green,
            Color.SlateGray
        };

        public Color[] DrawLinesColor { get; set; } =
            { Color.Blue, Color.Black, Color.Red, Color.Brown, Color.Gray, Color.Green, Color.SlateGray };

        public float MinLineThickness { get; set; } = 0.7f;
        public float MaxLineThickness { get; set; } = 2.0f;
        public ushort Width { get; init; } = 180;
        public ushort Height { get; init; } = 50;
        public ushort NoiseRate { get; init; } = 100;
        public Color[] NoiseRateColor { get; set; } = { Color.Gray, Color.Black, Color.Red };
        public byte FontSize { get; init; } = 29;
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;
        public EncoderTypes EncoderType { get; init; } = EncoderTypes.Jpeg;
        public IImageEncoder Encoder => Extensions.GetEncoder(EncoderType);
        public byte DrawLines { get; init; } = 5;
        public byte MaxRotationDegrees { get; init; } = 5;
        public Color[] BackgroundColor { get; set; } = { Color.White };
        public float? RotationDegree { get; init; } = 3;
    }
}