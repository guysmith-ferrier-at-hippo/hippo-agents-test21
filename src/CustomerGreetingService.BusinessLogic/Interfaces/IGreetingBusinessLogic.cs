using CustomerGreetingService.BusinessLogic.Models;

namespace CustomerGreetingService.BusinessLogic.Interfaces;

public interface IGreetingBusinessLogic
{
    GetHelloWorldGreetingResponse GetHelloWorldGreeting(string? name);
}
