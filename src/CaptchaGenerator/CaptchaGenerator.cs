using System;

namespace CaptchaGenerator
{
    public class CaptchaGenerator
    {
        private static char[] CodeLetters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public string GenerateCaptchaCode(int keyDigit = 4)
        {
            return Extensions.GetUniqueKey(keyDigit, CodeLetters);
        }

        public CaptchaResult GenerateCaptchaImage(ushort width, ushort height, string captchaCode)
        {
            var slc = new Captcha(new CaptchaOptions
            {
                Width = width,
                Height = height,
                MaxRotationDegrees = 15,
                FontSize = GetFontSize(width, captchaCode.Length)
            });
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
            var averageSize = imageWidth / captchCodeLength * 1.2;
            return Convert.ToByte(averageSize);
        }
    }
}
