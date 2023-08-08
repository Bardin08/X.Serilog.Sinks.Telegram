using System.Collections.Concurrent;

namespace X.Serilog.Sinks.Telegram.Batch;

internal class LogsQueueAccessContext : ILogsQueueAccessor
{
    private readonly ConcurrentQueue<string> _logs = new();

    public void Enqueue(string log)
    {
        _logs.Enqueue(log);
    }

    public Task<List<string>> DequeueSeveralAsync(int amount)
    {
        var logs = new List<string>();

        while (amount-- > 0 && !_logs.IsEmpty)
        {
            var isDequeued = _logs.TryDequeue(out var log);
            if (!isDequeued || log is null)
            {
                break;
            }

            logs.Add(log);
        }

        return Task.FromResult(logs);
    }

    public int GetSize()
    {
        return _logs.Count;
    }
}

internal interface ILogsQueueAccessor
{
    public void Enqueue(string log);
    public Task<List<string>> DequeueSeveralAsync(int amount);
    public int GetSize();
}