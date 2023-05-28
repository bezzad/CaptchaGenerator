namespace Captcha.Net
{
    public interface ICaptchaModule
    {
        byte[] Generate(string text);
    }
}
