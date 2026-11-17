using CustomerGreetingService.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomerGreetingService.Repositories.UnitTests;

[TestClass]
public sealed class GreetingRepositoryTests
{
    [TestMethod]
    public void GetDefaultGreeting_ReturnsHelloWorld()
    {
        // Arrange
        var repository = new GreetingRepository();

        // Act
        var result = repository.GetDefaultGreeting();

        // Assert
        Assert.AreEqual("Hello, World!", result);
    }
}
