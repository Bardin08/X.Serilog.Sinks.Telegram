using System.Collections.Immutable;
using X.Serilog.Sinks.Telegram.Batch;
using X.Serilog.Sinks.Telegram.Batch.Rules;

namespace X.Serilog.Sinks.Telegram.Configuration;

public class BatchEmittingRulesConfiguration
{
    private readonly TimeSpan _rulesCheckPeriod = TelegramSinkDefaults.RulesCheckPeriod;

    public TimeSpan RuleCheckPeriod
    {
        get => _rulesCheckPeriod;
        init
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentException(
                    "Invalid batch emit rules check period! It must be greater than TimeSpan.Zero!");
            }

            _rulesCheckPeriod = value;
        }
    }

    public IImmutableList<IRule> BatchProcessingRules { get; set; } = null!;

    public IImmutableList<IExecutionHook> BatchProcessingExecutionHooks
        => BatchProcessingRules
            .Select(rule =>
            {
                if (rule is IExecutionHook hook)
                {
                    return hook;
                }

                return null;
            })
            .Where(hook => hook != null)
            .ToImmutableList()!;
}