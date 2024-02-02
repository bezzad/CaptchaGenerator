using System;
using System.Security.Cryptography;
using System.Text;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace Captcha.Net
{
    public static class Extensions
    {
        private static readonly Random Rand = new(DateTime.Now.GetHashCode());

        private static readonly char[] Chars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVXYZW23456789".ToCharArray();

        public static IImageEncoder GetEncoder(EncoderTypes encoderType)
        {
            IImageEncoder encoder = encoderType switch
            {
                EncoderTypes.Png => new PngEncoder(),
                EncoderTypes.Jpeg => new JpegEncoder(),
                _ => throw new ArgumentException($"Encoder '{encoderType}' not found!")
            };
            return encoder;
        }

        public static string GetUniqueKey(int size)
        {
            return GetUniqueKey(size, Chars);
        }

        public static string GetUniqueKey(int size, char[] chars)
        {
            var data = new byte[4 * size];
            RandomNumberGenerator.Fill(data);
            var result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;
                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        public static float GenerateNextFloat(double min = double.MinValue, double max = double.MaxValue)
        {
            var range = max - min;
            var sample = Rand.NextDouble();
            var scaled = sample * range + min;
            var result = (float)scaled;
            return result;
        }
    }
}