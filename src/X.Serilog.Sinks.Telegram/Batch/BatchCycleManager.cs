using System.Collections.Immutable;
using System.Threading;
using X.Serilog.Sinks.Telegram.Batch.Rules;

namespace X.Serilog.Sinks.Telegram.Batch;

internal class BatchCycleManager
{
    private readonly IImmutableList<IRule> _batchPositingRules;
    private readonly IImmutableList<IExecutionHook> _executionHooks;
    private readonly PeriodicTimer _timer;

    public BatchCycleManager(
        IImmutableList<IRule> batchPositingRules,
        IImmutableList<IExecutionHook> executionHooks)
    {
        _batchPositingRules = batchPositingRules;
        _executionHooks = executionHooks;

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    }

    internal async Task WhenNextAvailableAsync(CancellationToken cancellationToken)
    {
        while (await _timer.WaitForNextTickAsync(cancellationToken))
        {
            var isAtLeastOneRulePassed =
                (await Task.WhenAll(_batchPositingRules.Select(rule => rule.IsPassedAsync(cancellationToken))))
                .Any(ruleResponse => ruleResponse);

            if (isAtLeastOneRulePassed)
            {
                break;
            }
        }
    }

    internal async Task OnBatchProcessedAsync(CancellationToken cancellationToken)
    {
        await Task.WhenAll(_executionHooks.Select(hook => hook.OnAfterExecuteAsync(cancellationToken)));
    }
}