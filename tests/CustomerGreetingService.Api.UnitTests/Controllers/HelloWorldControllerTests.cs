using CustomerGreetingService.Api.Controllers;
using CustomerGreetingService.BusinessLogic.Interfaces;
using CustomerGreetingService.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CustomerGreetingService.Api.UnitTests.Controllers;

[TestClass]
public sealed class HelloWorldControllerTests
{
    private Mock<IGreetingBusinessLogic> _mockGreetingLogic = null!;
    private HelloWorldController _controller = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockGreetingLogic = new Mock<IGreetingBusinessLogic>(MockBehavior.Strict);
        _controller = new HelloWorldController(_mockGreetingLogic.Object);
    }

    [TestMethod]
    public void Constructor_WithNullGreetingLogic_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new HelloWorldController(null!));
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WhenLogicSucceeds_ReturnsOkWithGetHelloWorldGreetingResponse()
    {
        // Arrange
        var expectedResponse = new GetHelloWorldGreetingResponse
        {
            Message = "Hello, World!",
            Timestamp = "2026-03-24T12:00:00.0000000Z"
        };

        _mockGreetingLogic
            .Setup(logic => logic.GetHelloWorldGreeting("World"))
            .Returns(expectedResponse);

        // Act
        var actionResult = _controller.GetHelloWorldGreeting("World");

        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        var okResult = (OkObjectResult)actionResult;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreSame(expectedResponse, okResult.Value);
        _mockGreetingLogic.Verify(logic => logic.GetHelloWorldGreeting("World"), Times.Once);
    }

    [TestMethod]
    public void GetHelloWorldGreeting_WhenLogicThrowsArgumentException_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        const string errorMessage = "Recipient name must not exceed 50 characters.";
        _mockGreetingLogic
            .Setup(logic => logic.GetHelloWorldGreeting(It.IsAny<string?>()))
            .Throws(new ArgumentException(errorMessage, "name"));

        // Act
        var actionResult = _controller.GetHelloWorldGreeting("LongName");

        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
        var objectResult = (ObjectResult)actionResult;
        Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);

        Assert.IsInstanceOfType(objectResult.Value, typeof(ProblemDetails));
        var problemDetails = (ProblemDetails)objectResult.Value;
        Assert.AreEqual(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.AreEqual("Bad Request", problemDetails.Title);
        Assert.AreEqual(errorMessage, problemDetails.Detail);
    }

    [TestMethod]
    public void HandleUnsupportedMethods_ReturnsMethodNotAllowedProblemDetails()
    {
        // Act
        var actionResult = _controller.HandleUnsupportedMethods();

        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(ObjectResult));
        var objectResult = (ObjectResult)actionResult;
        Assert.AreEqual(StatusCodes.Status405MethodNotAllowed, objectResult.StatusCode);

        Assert.IsInstanceOfType(objectResult.Value, typeof(ProblemDetails));
        var problemDetails = (ProblemDetails)objectResult.Value;
        Assert.AreEqual(StatusCodes.Status405MethodNotAllowed, problemDetails.Status);
        Assert.AreEqual("Method Not Allowed", problemDetails.Title);
        Assert.AreEqual("HTTP method not supported on this route.", problemDetails.Detail);
    }
}
