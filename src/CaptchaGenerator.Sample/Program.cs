var captchaGenerator = new CaptchaGenerator.CaptchaGenerator();
// Create path: "...\CaptchaGenerator\src\CaptchaGenerator.Sample\bin\debug\net6.0\captcha"
Directory.CreateDirectory("captcha"); 

for (int i = 0; i < 100; i++)
{
    var key = captchaGenerator.GenerateCaptchaCode();
    var result = captchaGenerator.GenerateCaptchaImage(200, 100, key);
    Console.WriteLine($"Captcha {i}: \n");
    Console.WriteLine(result.CaptchBase64Data);
    Console.WriteLine();

    File.WriteAllBytes($"captcha/captcha-{i}.png", result.CaptchaByteData);
}
