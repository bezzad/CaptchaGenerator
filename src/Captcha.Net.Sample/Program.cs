using Captcha.Net;
using System.Diagnostics;

var captchaGenerator = new CaptchaGenerator();
var currentDirectory = Directory.CreateDirectory("captcha");
var count = 1000;
var times = new List<long>(count);
var stopWatch = new Stopwatch();

for (int i = 0; i < count; i++)
{
    stopWatch.Restart();
    var key = captchaGenerator.GenerateCaptchaCode();
    var result = captchaGenerator.GenerateCaptchaImage(200, 100, key);
    Console.WriteLine($"Captcha {i}: \n");
    Console.WriteLine(result.CaptchBase64Data);
    Console.WriteLine();
    stopWatch.Stop();
    times.Add(stopWatch.ElapsedMilliseconds);
    File.WriteAllBytes($"{currentDirectory.FullName}/captcha-{i}.png", result.CaptchaByteData);
}

Console.WriteLine($"Duration:  Max({times.Max()}ms),  Min({times.Min()}ms),  Average({times.Average()}ms)");    

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
