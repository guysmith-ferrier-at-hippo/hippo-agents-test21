namespace CustomerGreetingService.BusinessLogic.Models;

public sealed class GetHelloWorldGreetingResponse
{
    public required string Message { get; init; }
    public required string Timestamp { get; init; }
}
