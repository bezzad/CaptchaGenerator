using System.Linq;
using Xunit;

namespace CaptchaGenerator.Test.IntegrationTests
{
    public class CaptchaGeneratorTest
    {
        [Fact]
        public void GenerateCaptchaCodeWithPureNumericalLettersTest()
        {
            // arrange
            var codeLetters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var generator = new CaptchaGenerator();

            // act
            var code = generator.GenerateCaptchaCode(8);

            // assert
            Assert.Equal(8, code.Length);
            Assert.True(code.Select(k => codeLetters.Contains(k)).All(i => i));
        }
    }
}
