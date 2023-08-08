using System.Threading;

namespace X.Serilog.Sinks.Telegram.Batch.Rules;

public class OncePerTimeRule : IRule, IExecutionHook
{
    private readonly TimeSpan _delay;

    private DateTime _nextExecution;

    public OncePerTimeRule(TimeSpan delay)
    {
        _delay = delay;
        _nextExecution = Now + delay;
    }

    private static DateTime Now => DateTime.Now;

    public Task OnAfterExecuteAsync(CancellationToken cancellationToken)
    {
        _nextExecution = Now + _delay;
        return Task.CompletedTask;
    }

    public Task<bool> IsPassedAsync(CancellationToken cancellationToken)
    {
        var now = Now;

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("{0}: Time till next execution: {1}", nameof(OncePerTimeRule), _nextExecution - now);
        Console.ResetColor();

        var isPassed = now >= _nextExecution;
        if (isPassed)
        {
            _nextExecution = now + _delay;
        }

        return Task.FromResult(isPassed);
    }
}