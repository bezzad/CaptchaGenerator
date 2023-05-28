using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.Runtime.InteropServices;

namespace Captcha.Net
{
    public class CaptchaOptions
    {
        /// <summary>
        /// Default fonts are  "Arial", "Verdana", "Times New Roman" in Windows
        /// Linux guys have to set thier own fonts :)
        /// </summary>
        public string[] FontFamilies { get; set; }
        public Color[] TextColor { get; set; } = new Color[] { Color.Blue, Color.Black, Color.Black, Color.DarkGray, Color.Brown, Color.Gray, Color.Gray, Color.Green, Color.SlateGray };
        public Color[] DrawLinesColor { get; set; } = new Color[] { Color.Blue, Color.Black, Color.Black, Color.Brown, Color.Gray, Color.Green, Color.SlateGray };
        public float MinLineThickness { get; set; } = 0.7f;
        public float MaxLineThickness { get; set; } = 2.0f;
        public ushort Width { get; set; } = 180;
        public ushort Height { get; set; } = 50;
        public ushort NoiseRate { get; set; } = 300;
        public Color[] NoiseRateColor { get; set; } = new Color[] { Color.Gray };
        public byte FontSize { get; set; } = 29;
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;
        public EncoderTypes EncoderType { get; set; } = EncoderTypes.Png;
        public IImageEncoder Encoder => Extensions.GetEncoder(EncoderType);
        public byte DrawLines { get; set; } = 5;
        public byte MaxRotationDegrees { get; set; } = 5;
        public Color[] BackgroundColor { get; set; } = new Color[] { Color.White };

        public CaptchaOptions()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                FontFamilies = new string[] { "DejaVu Serif", "DejaVu Sans Mono" };
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                FontFamilies = new string[] { "San Francisco", "Helvetica" };
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                FontFamilies = new string[] { "Arial", "Verdana", "Times New Roman" };
        }
    }
}
