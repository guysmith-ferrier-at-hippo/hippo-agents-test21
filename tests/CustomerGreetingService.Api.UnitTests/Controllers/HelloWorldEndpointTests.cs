using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CustomerGreetingService.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomerGreetingService.Api.UnitTests.Controllers;

[TestClass]
public sealed class HelloWorldEndpointTests
{
    private static WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _client.Dispose();
    }

    [TestMethod]
    public async Task GetHelloWorld_DefaultGreeting_Returns200WithExpectedPayload()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json", response.Content.Headers.ContentType?.MediaType);

        var payload = await response.Content.ReadFromJsonAsync<GetHelloWorldGreetingResponse>();
        Assert.IsNotNull(payload);
        Assert.AreEqual("Hello, World!", payload.Message);
        Assert.IsFalse(string.IsNullOrWhiteSpace(payload.Timestamp));
        Assert.IsTrue(DateTimeOffset.TryParse(payload.Timestamp, out _));
    }

    [TestMethod]
    public async Task GetHelloWorld_WithRecipientName_Returns200WithPersonalizedGreeting()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world?name=Alice");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<GetHelloWorldGreetingResponse>();
        Assert.IsNotNull(payload);
        Assert.AreEqual("Hello, Alice!", payload.Message);
    }

    [TestMethod]
    public async Task GetHelloWorld_WithUnicodeRecipientName_Returns200WithPreservedCharacters()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world?name=Ren%C3%A9e");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<GetHelloWorldGreetingResponse>();
        Assert.IsNotNull(payload);
        Assert.AreEqual("Hello, Renée!", payload.Message);
    }

    [TestMethod]
    public async Task GetHelloWorld_WithEmptyName_Returns200WithDefaultGreeting()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world?name=");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<GetHelloWorldGreetingResponse>();
        Assert.IsNotNull(payload);
        Assert.AreEqual("Hello, World!", payload.Message);
    }

    [TestMethod]
    public async Task GetHelloWorld_WithWhitespaceOnlyName_Returns200WithDefaultGreeting()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world?name=%20%20%20");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<GetHelloWorldGreetingResponse>();
        Assert.IsNotNull(payload);
        Assert.AreEqual("Hello, World!", payload.Message);
    }

    [TestMethod]
    public async Task GetHelloWorld_WithLeadingAndTrailingWhitespace_Returns200WithTrimmedName()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world?name=%20Bob%20");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<GetHelloWorldGreetingResponse>();
        Assert.IsNotNull(payload);
        Assert.AreEqual("Hello, Bob!", payload.Message);
    }

    [TestMethod]
    public async Task GetHelloWorld_WithBoundaryLengthFiftyCharacters_Returns200()
    {
        // Arrange
        var fiftyCharName = new string('A', 50);

        // Act
        var response = await _client.GetAsync($"/api/v1/hello-world?name={fiftyCharName}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<GetHelloWorldGreetingResponse>();
        Assert.IsNotNull(payload);
        Assert.AreEqual($"Hello, {fiftyCharName}!", payload.Message);
    }

    [TestMethod]
    public async Task GetHelloWorld_WhenNameExceedsFiftyCharacters_Returns400ProblemDetails()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world?name=ThisNameIsFarTooLongAndExceedsTheFiftyCharacterLimitNow");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.IsNotNull(problemDetails);
        Assert.AreEqual(400, problemDetails.Status);
        Assert.AreEqual("Bad Request", problemDetails.Title);
        Assert.IsTrue(problemDetails.Detail?.Contains("50 characters") == true);
    }

    [TestMethod]
    public async Task GetHelloWorld_WhenNameContainsControlCharacters_Returns400ProblemDetails()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/hello-world?name=Alice%00Bob");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.IsNotNull(problemDetails);
        Assert.AreEqual(400, problemDetails.Status);
        Assert.AreEqual("Bad Request", problemDetails.Title);
        Assert.IsTrue(problemDetails.Detail?.Contains("control character") == true);
    }

    [TestMethod]
    public async Task PostHelloWorld_Returns405MethodNotAllowed()
    {
        // Act
        var response = await _client.PostAsync("/api/v1/hello-world", new StringContent(string.Empty));

        // Assert
        Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.IsNotNull(problemDetails);
        Assert.AreEqual(405, problemDetails.Status);
        Assert.AreEqual("Method Not Allowed", problemDetails.Title);
        Assert.AreEqual("HTTP method not supported on this route.", problemDetails.Detail);
    }

    [TestMethod]
    public async Task PutHelloWorld_Returns405MethodNotAllowed()
    {
        // Act
        var response = await _client.PutAsync("/api/v1/hello-world", new StringContent(string.Empty));

        // Assert
        Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.IsNotNull(problemDetails);
        Assert.AreEqual(405, problemDetails.Status);
        Assert.AreEqual("Method Not Allowed", problemDetails.Title);
        Assert.AreEqual("HTTP method not supported on this route.", problemDetails.Detail);
    }

    [TestMethod]
    public async Task DeleteHelloWorld_Returns405MethodNotAllowed()
    {
        // Act
        var response = await _client.DeleteAsync("/api/v1/hello-world");

        // Assert
        Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.IsNotNull(problemDetails);
        Assert.AreEqual(405, problemDetails.Status);
        Assert.AreEqual("Method Not Allowed", problemDetails.Title);
        Assert.AreEqual("HTTP method not supported on this route.", problemDetails.Detail);
    }

    [TestMethod]
    public async Task GetSwaggerDocumentation_Returns200WithOpenApiJson()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsFalse(string.IsNullOrWhiteSpace(content));
        using var jsonDoc = JsonDocument.Parse(content);
        var root = jsonDoc.RootElement;
        Assert.IsTrue(root.TryGetProperty("paths", out var paths));
        Assert.IsTrue(paths.TryGetProperty("/api/v1/hello-world", out _));
    }

    [TestMethod]
    public async Task GetSwaggerUi_Returns200WithHtml()
    {
        // Act
        var response = await _client.GetAsync("/swagger/index.html");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("text/html", response.Content.Headers.ContentType?.MediaType);
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains("swagger-ui", StringComparison.OrdinalIgnoreCase));
    }
}
