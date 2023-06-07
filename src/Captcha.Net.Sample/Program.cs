using Captcha.Net;
using System.Diagnostics;

var captchaGenerator = new CaptchaGenerator();
var currentDirectory = Directory.CreateDirectory("captcha");

for (int i = 0; i < 1000; i++)
{
    var key = captchaGenerator.GenerateCaptchaCode();
    var result = captchaGenerator.GenerateCaptchaImage(200, 100, key);
    Console.WriteLine($"Captcha {i}: \n");
    Console.WriteLine(result.CaptchBase64Data);
    Console.WriteLine();

    File.WriteAllBytes($"{currentDirectory.FullName}/captcha-{i}.png", result.CaptchaByteData);
}

// open the directory after completion
if (OperatingSystem.IsWindows())
{ 
    Process.Start("explorer.exe", currentDirectory.FullName); 
}
else if (OperatingSystem.IsLinux())
{
    // Start a new process to open the folder
    Process.Start("xdg-open", currentDirectory.FullName);
}
