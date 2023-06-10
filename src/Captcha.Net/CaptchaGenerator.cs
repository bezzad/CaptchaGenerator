using System;
using System.Collections.Concurrent;

namespace Captcha.Net
{
    public class CaptchaGenerator
    {
        private static char[] CodeLetters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static ConcurrentDictionary<string, Captcha> CaptchaKeeper = new ConcurrentDictionary<string, Captcha>();

        public string GenerateCaptchaCode(int keyDigit = 4)
        {
            return Extensions.GetUniqueKey(keyDigit, CodeLetters);
        }

        public CaptchaResult GenerateCaptchaImage(ushort width, ushort height, string captchaCode)
        {
            var key = $"{width}w_{height}h_{captchaCode.Length}d";
            var captcha = CaptchaKeeper.GetOrAdd(key, k =>
                new Captcha(new CaptchaOptions
                {
                    Width = width,
                    Height = height,
                    MaxRotationDegrees = 15,
                    RotationDegree = 10,
                    NoiseRate = 50,
                    DrawLines = 4,
                    FontSize = GetFontSize(width, captchaCode.Length),
                    EncoderType = EncoderTypes.Jpeg
                }));

            var captchaCodeBytes = captcha.Generate(captchaCode);

            return new CaptchaResult
            {
                CaptchaCode = captchaCode,
                CaptchaByteData = captchaCodeBytes,
                Timestamp = DateTime.Now
            };
        }

        private byte GetFontSize(int imageWidth, int captchCodeLength)
        {
            var averageSize = imageWidth / captchCodeLength * 1.2;
            return Convert.ToByte(averageSize);
        }
    }
}
