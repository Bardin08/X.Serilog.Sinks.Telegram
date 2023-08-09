﻿using System.Threading;
using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Batch.Rules;

/// <summary>
/// Emit logs batch when in the logs queue is not less than <see cref="TelegramSinkConfiguration.BatchPeriod"/> entries.
/// </summary>
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

        var isPassed = now >= _nextExecution;
        if (isPassed)
        {
            _nextExecution = now + _delay;
        }

        return Task.FromResult(isPassed);
    }
}