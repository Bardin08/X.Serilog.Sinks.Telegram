using System.Threading;

namespace X.Serilog.Sinks.Telegram.Batch.Rules;

public interface IRule
{
    Task<bool> IsPassedAsync(CancellationToken cancellationToken);
}