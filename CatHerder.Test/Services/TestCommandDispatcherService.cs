using NUnit.Framework;
using CatHerder.Services;

namespace CatHerder.Test.Services
{
    public class TestCommandDispatcherService
    {
        [TestCase("", 0UL, false, 0)]
        [TestCase("<@!123> a", 123UL, false, 8)]
        [TestCase("<@!123> !a", 123UL, true, 9)]
        [TestCase("<@&123> !a", 123UL, true, 9)]
        [TestCase("<@#123> !a", 123UL, false, 9)]
        [TestCase("<@!123>!a", 123UL, true, 8)]
        [TestCase("<@!123>  !a", 123UL, true, 10)]
        [TestCase("<@!456> !a", 123UL, false, 8)]
        public void IsCommandToUser(string content, ulong userId, bool expectedResult, int expectedArgPos)
        {
            (bool result, int argPos) = CommandDispatcherService.IsCommandToUser(content, userId);
            Assert.That(result, Is.EqualTo(expectedResult));
            if(expectedResult)
            {
                Assert.That(argPos, Is.EqualTo(expectedArgPos));
            }
        }
    }
}