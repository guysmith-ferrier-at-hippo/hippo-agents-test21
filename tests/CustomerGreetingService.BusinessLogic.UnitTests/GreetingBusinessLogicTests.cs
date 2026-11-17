using CustomerGreetingService.BusinessLogic.Models;
using CustomerGreetingService.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CustomerGreetingService.BusinessLogic.UnitTests;

[TestClass]
public sealed class GreetingBusinessLogicTests
{
    private sealed class FakeTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _utcNow;

        public FakeTimeProvider(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public override DateTimeOffset GetUtcNow() => _utcNow;
    }

    private Mock<IGreetingRepository> _mockGreetingRepository = null!;
    private FakeTimeProvider _fakeTimeProvider = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockGreetingRepository = new Mock<IGreetingRepository>(MockBehavior.Strict);
        _mockGreetingRepository.Setup(repo => repo.GetDefaultGreeting()).Returns("Hello, World!");
        _fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 3, 24, 12, 0, 0, TimeSpan.Zero));
    }

    [TestMethod]
    public void Constructor_WithNullTimeProvider_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new GreetingBusinessLogic(null!, _mockGreetingRepository.Object));
    }

    [TestMethod]
    public void Constructor_WithNullGreetingRepository_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new GreetingBusinessLogic(_fakeTimeProvider, null!));
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithNullName_ReturnsDefaultGreetingFromRepository()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);

        // Act
        var result = service.GetHelloWorldGreeting(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hello, World!", result.Message);
        Assert.AreEqual("2026-03-24T12:00:00.0000000Z", result.Timestamp);
        _mockGreetingRepository.Verify(repo => repo.GetDefaultGreeting(), Times.Once);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithDeterministicTimestamp_FormatsIso8601UtcCorrectly()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);

        // Act
        var result = service.GetHelloWorldGreeting(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("2026-03-24T12:00:00.0000000Z", result.Timestamp);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithValidName_ReturnsPersonalizedGreeting()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);

        // Act
        var result = service.GetHelloWorldGreeting("Alice");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hello, Alice!", result.Message);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithUnicodeName_PreservesCharacters()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);

        // Act
        var result = service.GetHelloWorldGreeting("Renée");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hello, Renée!", result.Message);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithEmptyName_ReturnsDefaultGreeting()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);

        // Act
        var result = service.GetHelloWorldGreeting(string.Empty);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hello, World!", result.Message);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithWhitespaceOnlyName_ReturnsDefaultGreeting()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);

        // Act
        var result = service.GetHelloWorldGreeting("   ");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hello, World!", result.Message);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithLeadingAndTrailingWhitespace_TrimsName()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);

        // Act
        var result = service.GetHelloWorldGreeting(" Bob ");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Hello, Bob!", result.Message);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WithBoundaryLengthFiftyCharacters_ReturnsPersonalizedGreeting()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);
        var boundaryName = new string('A', 50);

        // Act
        var result = service.GetHelloWorldGreeting(boundaryName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual($"Hello, {boundaryName}!", result.Message);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WhenNameExceedsFiftyCharacters_ThrowsArgumentException()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);
        var longName = "ThisNameIsFarTooLongAndExceedsTheFiftyCharacterLimitNow";

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => service.GetHelloWorldGreeting(longName));
        Assert.IsTrue(ex.Message.Contains("50 characters", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WhenNameContainsAsciiControlCharacter_ThrowsArgumentException()
    {
        // Arrange
        var service = new GreetingBusinessLogic(_fakeTimeProvider, _mockGreetingRepository.Object);
        var invalidName = "Alice\0Bob";

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => service.GetHelloWorldGreeting(invalidName));
        Assert.IsTrue(ex.Message.Contains("control character", StringComparison.OrdinalIgnoreCase));
    }
}
