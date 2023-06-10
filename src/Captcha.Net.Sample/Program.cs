using Captcha.Net;
using System.Diagnostics;

var captchaGenerator = new CaptchaGenerator();
var currentDirectory = Directory.CreateDirectory("captcha");
var count = 10000;
var times = new List<long>(count);
var stopWatch = new Stopwatch();

for (int i = 0; i < count; i++)
{
    stopWatch.Restart();
    var key = captchaGenerator.GenerateCaptchaCode();
    var result = captchaGenerator.GenerateCaptchaImage(145, 56, key);
    stopWatch.Stop();
    Console.WriteLine($"Captcha {i}: \n");
    Console.WriteLine(result.CaptchBase64Data);
    Console.WriteLine();
    times.Add(stopWatch.ElapsedMilliseconds);
    File.WriteAllBytes($"{currentDirectory.FullName}/captcha-{i}.jpg", result.CaptchaByteData);
}

var maxDuration = times.Max();
Console.WriteLine($"Duration:  Max({maxDuration}ms),  Min({times.Min()}ms),  Average({times.Average()}ms)");
for (int i = 1, q = 1; i < maxDuration + q; i += q)
{
    var timeCount = times.Count(t => t <= i && t > i - q);

    if (timeCount != 0)
    {
        Console.WriteLine($"Time[{i - q}ms ~ {i}ms]: {timeCount}");
    }

    if (i == 10)
        q = 10;
    else if (i == 100)
        q = 100;
    else if (i == 1000)
        q = 1000;
}

try
{
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
}
catch { }