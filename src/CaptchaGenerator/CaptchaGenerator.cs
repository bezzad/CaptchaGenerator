using SixLabors.ImageSharp;
using System.Runtime.InteropServices;

namespace CaptchaGenerator
{
    public class CaptchaGenerator
    {
        private static char[] CodeLetters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public string GenerateCaptchaCode()
        {
            return Extensions.GetUniqueKey(4, CodeLetters);
        }

        public CaptchaResult GenerateCaptchaImage(ushort width, ushort height, string captchaCode)
        {
            var opt = new CaptchaOptions
            {
                DrawLines = 5,
                TextColor = new Color[] { Color.DarkGray, Color.Gray, Color.LightGray, Color.SlateGray, Color.Black },
                DrawLinesColor = new Color[] { Color.Gray, Color.Black, Color.SlateGray },
                NoiseRate = 100,
                BackgroundColor = new Color[] { Color.White },
                Width = width,
                Height = height,
                MaxRotationDegrees = 15,
                FontSize = GetFontSize(width, captchaCode.Length)
            };
            var slc = new CaptchaModule(opt);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                opt.FontFamilies = new string[] { "DejaVu Serif", "DejaVu Sans Mono" };
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                opt.FontFamilies = new string[] { "San Francisco", "Helvetica" };
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                opt.FontFamilies = new string[] { "Arial", "Verdana", "Times New Roman" };

            var captchaCodeBytes = slc.Generate(captchaCode);

            return new CaptchaResult
            {
                CaptchaCode = captchaCode,
                CaptchaByteData = captchaCodeBytes,
                Timestamp = DateTime.Now
            };
        }

        private byte GetFontSize(int imageWidth, int captchCodeLength)
        {
            var averageSize = imageWidth / captchCodeLength;
            return Convert.ToByte(averageSize);
        }
    }
}
