namespace CustomerGreetingService.Repositories;

public class GreetingRepository : IGreetingRepository
{
    private const string DefaultGreeting = "Hello, World!";

    public string GetDefaultGreeting()
    {
        return DefaultGreeting;
    }
}
