[![Windows x64](https://github.com/bezzad/CaptchaGenerator/workflows/Windows%20x64/badge.svg)](https://github.com/bezzad/CaptchaGenerator/actions/workflows/dotnet-windows.yml)
[![Ubuntu x64](https://github.com/bezzad/CaptchaGenerator/workflows/Ubuntu%20x64/badge.svg)](https://github.com/bezzad/CaptchaGenerator/actions/workflows/dotnet-ubuntu.yml)
[![NuGet](https://img.shields.io/nuget/dt/Captcha.Net.svg)](https://www.nuget.org/packages/Captcha.Net)
[![NuGet](https://img.shields.io/nuget/vpre/Captcha.Net.svg)](https://www.nuget.org/packages/Captcha.Net)
[![License](https://img.shields.io/github/license/bezzad/CaptchaGenerator.svg)](https://github.com/bezzad/CaptchaGenerator/blob/master/LICENSE)
[![Generic badge](https://img.shields.io/badge/support-.Net_6-blue.svg)](https://github.com/bezzad/CaptchaGenerator)

# Captcha Generator .Net
Captcha Generator is a simple cross-platform library for generating image captcha.

## Features

- Simple & Cross-Platform
- Compatible with Linux and Windows
- Compatible with Docker images based on Linux :)      

## Installing via [NuGet](https://www.nuget.org/packages/Downloader)

    PM> Install-Package Captcha.Net

## Installing via the .NET Core command line interface

    dotnet add package Captcha.Net|

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