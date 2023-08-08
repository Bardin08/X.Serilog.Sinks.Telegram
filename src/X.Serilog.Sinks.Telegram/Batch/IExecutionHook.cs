using System.Threading;

namespace X.Serilog.Sinks.Telegram.Batch;

public interface IExecutionHook
{
    Task OnAfterExecuteAsync(CancellationToken cancellationToken);
}