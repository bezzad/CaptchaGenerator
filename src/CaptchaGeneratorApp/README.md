# Captcha Generator
Captcha Generator is a simple cross-platform library for generating image captcha.

## Features

- Simple & Cross-Platform
- Compatible with Linux and Windows
- Compatible with Docker images based on Linux :)                                                          |

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