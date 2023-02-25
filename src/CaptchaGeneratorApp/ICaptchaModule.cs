namespace CaptchaGeneratorApp
{
    public interface ICaptchaModule
    {
        byte[] Generate(string stringText);
    }
}
