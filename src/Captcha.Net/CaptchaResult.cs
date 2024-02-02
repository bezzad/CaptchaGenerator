using System;

namespace Captcha.Net
{
    public class CaptchaResult
    {
        public string CaptchaCode { get; init; }
        public byte[] CaptchaByteData { get; set; }

        public string CaptchBase64Data
        {
            get => Convert.ToBase64String(CaptchaByteData);
            set => CaptchaByteData = Convert.FromBase64String(value);
        }

        public DateTime Timestamp { get; set; }
    }
}