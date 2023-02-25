# CaptchaGenerator
CaptchaGenerator is a simple cross-platform NuGet package for generating image captcha.

## Features

- Simple & Cross-Platform
- Compatible with Linux and Windows
- Compatible with Docker images based on Linux :)

## CaptchaOptions

| Property           | Description                                                                                                  |
| ------------------ | ------------------------------------------------------------------------------------------------------------ |
| FontFamilies       | Characters fonts, default is "Arial", "Verdana", "Times New Roman"                                           |
| TextColor          | Characters colors, default is { Color.Blue, Color.Black, Color.Black, Color.Brown, Color.Gray, Color.Green } |
| DrawLinesColor     | Line colors, default is { Color.Blue, Color.Black, Color.Black, Color.Brown, Color.Gray, Color.Green }       |
| Width              | Width of image box, default is 180                                                                           |
| Height             | Height of image box, default is 50                                                                           |
| FontSize           | Font size, default is 29                                                                                     |
| FontStyle          | Font Style: Regular,Bold,Italic,BoldItalic                                                                   |
| EncoderType        | Result file formant: Jpeg,Png                                                                                |
| DrawLines          | Draw the random lines, default is 5                                                                          |
| MaxRotationDegrees | Rotation degrees, default is 5                                                                               |
| MinLineThickness   | Min Line Thickness, default is 0.7f                                                                          |
| MaxLineThickness   | Max Line Thickness, default is 2.0f                                                                          |
| NoiseRate          | Noise Rate, default is 800                                                                                   |
| NoiseRateColor     | Noise colors, default is { Color.Gray }                                                                      |
| BackgroundColor    | Background colors, default is { Color.White }                                                                |

 **FontFamilies Option**:
 `Notice: This default fonts working only on Windows, if you want to run it on Linux you have to use the Linux fonts`


## Usage:
```csharp
using CaptchaGenerator;

namespace ConsoleAppSample
{
	class Program
	{
		static void Main(string[] args)
		{
			var captchaGenerator = new CaptchaGenerator();
			var key = captchaGenerator.GenerateCaptchaCode();
			var result = captchaGenerator.GenerateCaptchaImage(200, 100, key);
			File.WriteAllBytes($"captcha.png", result.CaptchaByteData);
		}
	}
}

```