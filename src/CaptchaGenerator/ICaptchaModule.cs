namespace CaptchaGenerator
{
    public interface ICaptchaModule
    {
        byte[] Generate(string stringText);
    }
}
