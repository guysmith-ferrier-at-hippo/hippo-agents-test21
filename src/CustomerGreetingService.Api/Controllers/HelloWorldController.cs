using CustomerGreetingService.BusinessLogic.Interfaces;
using CustomerGreetingService.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerGreetingService.Api.Controllers;

[ApiController]
[Route("api/v1/hello-world")]
[Produces("application/json")]
public class HelloWorldController : ControllerBase
{
    private readonly IGreetingBusinessLogic _greetingBusinessLogic;

    public HelloWorldController(IGreetingBusinessLogic greetingBusinessLogic)
    {
        _greetingBusinessLogic = greetingBusinessLogic ?? throw new ArgumentNullException(nameof(greetingBusinessLogic));
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetHelloWorldGreetingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult GetHelloWorldGreeting([FromQuery] string? name)
    {
        try
        {
            var response = _greetingBusinessLogic.GetHelloWorldGreeting(name);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            string detail = ex.Message;
            int paramIndex = detail.IndexOf(" (Parameter", StringComparison.Ordinal);
            if (paramIndex >= 0)
            {
                detail = detail[..paramIndex];
            }

            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: detail,
                type: "https://tools.ietf.org/html/rfc7807"
            );
        }
    }

    [AcceptVerbs("POST", "PUT", "DELETE", "PATCH")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status405MethodNotAllowed)]
    public IActionResult HandleUnsupportedMethods()
    {
        return Problem(
            statusCode: StatusCodes.Status405MethodNotAllowed,
            title: "Method Not Allowed",
            detail: "HTTP method not supported on this route.",
            type: "https://tools.ietf.org/html/rfc7807"
        );
    }
}
