// See https://aka.ms/new-console-template for more information
using CaptchaGeneratorApp;

Console.WriteLine("Hello, World!");


var captchaGenerator = new CaptchaGenerator();
var code = captchaGenerator.GenerateCaptchaCode(Guid.NewGuid());
var result = captchaGenerator.GenerateCaptchaImage(200, 100, code);
Console.WriteLine(result.CaptchBase64Data);