using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.Linq;
using Xunit;

namespace Captcha.Net.Test.UnitTests;

public class ExtensionsTest
{
    [Fact]
    public void GetEncoderOfPngTest()
    {
        // arrange
        var encoderType = EncoderTypes.Png;

        // act
        var encoder = Extensions.GetEncoder(encoderType);

        // assert
        Assert.IsType<PngEncoder>(encoder);
    }

    [Fact]
    public void GetEncoderOfJpegTest()
    {
        // arrange
        var encoderType = EncoderTypes.Jpeg;

        // act
        var encoder = Extensions.GetEncoder(encoderType);

        // assert
        Assert.IsType<JpegEncoder>(encoder);
    }

    [Fact]
    public void GetUniqueKeyTest()
    {
        // arrange
        var len = 6;

        // action
        var key = Extensions.GetUniqueKey(len);

        // assert
        Assert.NotNull(key);
        Assert.Equal(len, key.Length);
    }

    [Fact]
    public void GetUniqueKeyWithSpecificCharsTest()
    {
        // arrange
        var len = 8;
        var chars = new char[] { '1', '2', '3', '4', '5' };

        // action
        var key = Extensions.GetUniqueKey(len, chars);

        // assert
        Assert.NotNull(key);
        Assert.Equal(len, key.Length);
        Assert.True(key.Select(k => chars.Contains(k)).All(i => i));
    }

    [Fact]
    public void GenerateNextFloatTest()
    {
        // arrange
        var min = 0;
        var max = 1;

        // action
        var randomNum = Extensions.GenerateNextFloat(min, max);

        // assert
        Assert.True(randomNum >= min, $"Random number is: {randomNum}");
        Assert.True(randomNum <= max, $"Random number is: {randomNum}");
    }
}