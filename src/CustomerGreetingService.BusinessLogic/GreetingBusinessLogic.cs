using System.Globalization;
using CustomerGreetingService.BusinessLogic.Interfaces;
using CustomerGreetingService.BusinessLogic.Models;
using CustomerGreetingService.Repositories;

namespace CustomerGreetingService.BusinessLogic;

public class GreetingBusinessLogic : IGreetingBusinessLogic
{
    private const int MaxNameLength = 50;
    private readonly TimeProvider _timeProvider;
    private readonly IGreetingRepository _greetingRepository;

    public GreetingBusinessLogic(TimeProvider timeProvider, IGreetingRepository greetingRepository)
    {
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        _greetingRepository = greetingRepository ?? throw new ArgumentNullException(nameof(greetingRepository));
    }

    public GetHelloWorldGreetingResponse GetHelloWorldGreeting(string? name)
    {
        string message = _greetingRepository.GetDefaultGreeting();

        if (name is not null)
        {
            ValidateName(name);

            string trimmedName = name.Trim();
            if (!string.IsNullOrEmpty(trimmedName))
            {
                message = $"Hello, {trimmedName}!";
            }
        }

        string timestamp = _timeProvider.GetUtcNow().UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ", CultureInfo.InvariantCulture);

        return new GetHelloWorldGreetingResponse
        {
            Message = message,
            Timestamp = timestamp
        };
    }

    private static void ValidateName(string name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            if ((c >= 0 && c <= 0x1F) || c == 0x7F)
            {
                throw new ArgumentException("Recipient name contains invalid control characters.");
            }
        }

        if (name.Length > MaxNameLength)
        {
            throw new ArgumentException($"Recipient name must not exceed {MaxNameLength} characters.");
        }
    }
}
