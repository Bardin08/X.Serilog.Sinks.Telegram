using System.Threading;

namespace X.Serilog.Sinks.Telegram.Batch.Rules;

internal class BatchSizeRule : IRule
{
    private readonly ILogsQueueAccessor _accessContext;
    private readonly int _batchSize;

    public BatchSizeRule(ILogsQueueAccessor accessContext, int batchSize)
    {
        _batchSize = batchSize;
        _accessContext = accessContext;
    }

    public Task<bool> IsPassedAsync(CancellationToken cancellationToken)
    {
        var currentSize = _accessContext.GetSize();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("{0}: Current queue size: {1}, BatchSize: {2}", nameof(BatchSizeRule), currentSize,
            _batchSize);
        Console.ResetColor();

        var isPass = _accessContext.GetSize() >= _batchSize;
        return Task.FromResult(isPass);
    }
}