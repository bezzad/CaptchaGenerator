using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Captcha.Net
{
    public static class ShuffleList
    {
        private static readonly Random random = new();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }

    public class CaptchaGenerator
    {
        private static char[] _codeLetters = Array.Empty<char>();
        private static readonly ConcurrentDictionary<string, Captcha> CaptchaKeeper = new();

        public string GenerateCaptchaCode(int keyDigit = 6)
        {
            var lowercaseLatinLetters = Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c);
            var uppercaseLatinLetters = Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c);
            var digits = Enumerable.Range('0', '9' - '0' + 1).Select(c => (char)c);
            var specialCharacters = "!@#$%^&*()-_+={}[];:'<>,./?".ToCharArray();

            var lowercaseCyrillicLetters = Enumerable.Range('а', 'я' - 'а' + 1).Select(c => (char)c);
            var uppercaseCyrillicLetters = Enumerable.Range('А', 'Я' - 'А' + 1).Select(c => (char)c);

            _codeLetters = lowercaseLatinLetters
                .Concat(uppercaseLatinLetters)
                .Concat(digits)
                .Concat(specialCharacters)
                .Concat(lowercaseCyrillicLetters)
                .Concat(uppercaseCyrillicLetters)
                .ToArray();

            _codeLetters.Shuffle();

            return Extensions.GetUniqueKey(keyDigit, _codeLetters);
        }

        public CaptchaResult GenerateCaptchaImage(ushort width, ushort height, string captchaCode)
        {
            var key = $"{width}w_{height}h_{captchaCode.Length}d";
            var captcha = CaptchaKeeper.GetOrAdd(key, k =>
                new Captcha(new CaptchaOptions
                {
                    Width = width,
                    Height = height,
                    MaxRotationDegrees = (byte)Random.Shared.Next(10, 15),
                    RotationDegree = (byte)Random.Shared.Next(2, 7),
                    NoiseRate = (byte)Random.Shared.Next(20, 50),
                    DrawLines = (byte)Random.Shared.Next(2, 7),
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