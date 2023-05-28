using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Captcha.Net
{
    public static class Extensions
    {
        private static Random Rand = new Random(DateTime.Now.GetHashCode());

        private static readonly char[] Chars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVXYZW23456789".ToCharArray();

        public static IImageEncoder GetEncoder(EncoderTypes encoderType)
        {
            IImageEncoder encoder;
            switch (encoderType)
            {
                case EncoderTypes.Png:
                    encoder = new PngEncoder();
                    break;
                case EncoderTypes.Jpeg:
                    encoder = new JpegEncoder();
                    break;
                default:
                    throw new ArgumentException($"Encoder '{encoderType}' not found!");
            };
            return encoder;
        }

        public static string GetUniqueKey(int size)
        {
            return GetUniqueKey(size, Chars);
        }

        public static string GetUniqueKey(int size, char[] chars)
        {
            byte[] data = new byte[4 * size];
            RandomNumberGenerator.Fill(data);
            StringBuilder result = new StringBuilder(size);
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
            double range = max - min;
            double sample = Rand.NextDouble();
            double scaled = sample * range + min;
            float result = (float)scaled;
            return result;
        }
    }
}
