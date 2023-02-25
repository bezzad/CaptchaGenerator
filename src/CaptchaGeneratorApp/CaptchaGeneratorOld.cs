//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Text;

//namespace CaptchaGeneratorApp
//{
//    public class CaptchaGenerator
//    {
//        static Random Rand = new Random(DateTime.Now.GetHashCode());

//        public string GenerateCaptchaCode(Guid key)
//        {
//            StringBuilder sb = new StringBuilder();
//            int number = Rand.Next(1000, 9999);
//            sb.Append(number);
//            return sb.ToString();
//        }

//        public CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
//        {
//            using (var baseMap = new Bitmap(width, height))
//            using (Graphics graph = Graphics.FromImage(baseMap))
//            {
//                graph.Clear(Color.White);

//                DrawCaptchaCode(graph, width, height, captchaCode);
//                DrawDisorderLine(graph, width, height);
//                AdjustRippleEffect(baseMap);

//                MemoryStream ms = new MemoryStream();
//                baseMap.Save(ms, ImageFormat.Png);

//                return new CaptchaResult
//                {
//                    CaptchaCode = captchaCode,
//                    CaptchaByteData = ms.ToArray(),
//                    Timestamp = DateTime.Now
//                };
//            }
//        }

//        private void DrawCaptchaCode(Graphics graph, int width, int height, string captchaCode)
//        {
//            SolidBrush fontBrush = new SolidBrush(Color.Black);
//            int fontSize = GetFontSize(width, captchaCode.Length);
//            Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
//            for (int i = 0; i < captchaCode.Length; i++)
//            {
//                fontBrush.Color = GetRandomDeepColor();

//                int shiftPx = fontSize / 6;

//                float x = i * fontSize + Rand.Next(-shiftPx, shiftPx) + Rand.Next(-shiftPx, shiftPx);
//                if (x < 0)
//                {
//                    x = 0;
//                }
//                int maxY = height - fontSize;
//                if (maxY < 0) maxY = 0;
//                float y = Rand.Next(0, maxY);

//                graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
//            }
//        }

//        private void DrawDisorderLine(Graphics graph, int width, int height)
//        {
//            Pen linePen = new Pen(new SolidBrush(Color.Black), 1);
//            for (int i = 0; i < Rand.Next(3, 5); i++)
//            {
//                linePen.Color = GetRandomDeepColor();

//                Point startPoint = new Point(Rand.Next(0, width), Rand.Next(0, height));
//                Point endPoint = new Point(Rand.Next(0, width), Rand.Next(0, height));
//                graph.DrawLine(linePen, startPoint, endPoint);
//            }
//        }

//        private void AdjustRippleEffect(Bitmap baseMap)
//        {
//            short nWave = 6;
//            int nWidth = baseMap.Width;
//            int nHeight = baseMap.Height;

//            Point[,] pt = new Point[nWidth, nHeight];

//            for (int x = 0; x < nWidth; ++x)
//            {
//                for (int y = 0; y < nHeight; ++y)
//                {
//                    var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
//                    var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

//                    var newX = x + xo;
//                    var newY = y + yo;

//                    if (newX > 0 && newX < nWidth)
//                    {
//                        pt[x, y].X = (int)newX;
//                    }
//                    else
//                    {
//                        pt[x, y].X = 0;
//                    }


//                    if (newY > 0 && newY < nHeight)
//                    {
//                        pt[x, y].Y = (int)newY;
//                    }
//                    else
//                    {
//                        pt[x, y].Y = 0;
//                    }
//                }
//            }

//            Bitmap bSrc = (Bitmap)baseMap.Clone();

//            BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height),
//                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
//            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
//                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

//            int scanline = bitmapData.Stride;

//            IntPtr scan0 = bitmapData.Scan0;
//            IntPtr srcScan0 = bmSrc.Scan0;

//            unsafe
//            {
//                byte* p = (byte*)(void*)scan0;
//                byte* pSrc = (byte*)(void*)srcScan0;

//                int nOffset = bitmapData.Stride - baseMap.Width * 3;

//                for (int y = 0; y < nHeight; ++y)
//                {
//                    for (int x = 0; x < nWidth; ++x)
//                    {
//                        var xOffset = pt[x, y].X;
//                        var yOffset = pt[x, y].Y;

//                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
//                        {
//                            if (pSrc != null)
//                            {
//                                p[0] = pSrc[yOffset * scanline + xOffset * 3];
//                                p[1] = pSrc[yOffset * scanline + xOffset * 3 + 1];
//                                p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
//                            }
//                        }

//                        p += 3;
//                    }

//                    p += nOffset;
//                }
//            }

//            baseMap.UnlockBits(bitmapData);
//            bSrc.UnlockBits(bmSrc);
//            bSrc.Dispose();
//        }

//        private Color GetRandomDeepColor()
//        {
//            int redlow = 160, greenLow = 100, blueLow = 160;
//            return Color.FromArgb(Rand.Next(redlow), Rand.Next(greenLow), Rand.Next(blueLow));
//        }

//        private int GetFontSize(int imageWidth, int captchCodeLength)
//        {
//            var averageSize = imageWidth / captchCodeLength;
//            return Convert.ToInt32(averageSize);
//        }


//    }
//}
