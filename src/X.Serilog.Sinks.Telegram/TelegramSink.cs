using Serilog.Core;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Channels;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using X.Serilog.Sinks.Telegram.Batch;
using X.Serilog.Sinks.Telegram.Batch.Rules;
using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram;

public class TelegramSink : ILogEventSink, IDisposable
{
    private readonly BatchCycleManager _batchCycleManager;

    private readonly ITelegramBotClient _botClient;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ChannelWriter<LogEvent> _channelWriter;
    private readonly ILogsQueueAccessor _logsQueueAccessor;
    private readonly IMessageFormatter _messageFormatter;
    private readonly TelegramSinkConfiguration _sinkConfiguration;

    public TelegramSink(
        ChannelWriter<LogEvent> channelWriter,
        ILogsQueueAccessor logsQueueAccessor,
        IImmutableList<IRule> batchPostingRules,
        IImmutableList<IExecutionHook> executionHooks,
        TelegramSinkConfiguration sinkConfiguration,
        IMessageFormatter messageFormatter)
    {
        _channelWriter = channelWriter;
        _logsQueueAccessor = logsQueueAccessor;
        _sinkConfiguration = sinkConfiguration;
        _messageFormatter =
            messageFormatter ?? TelegramSinkDefaults.GetDefaultMessageFormatter(_sinkConfiguration.Mode);

        _cancellationTokenSource = new CancellationTokenSource();
        _botClient = new TelegramBotClient(_sinkConfiguration.Token);
        _batchCycleManager = new BatchCycleManager(batchPostingRules, executionHooks);

        ExecuteLogsProcessingLoop(CancellationToken);
    }

    private CancellationToken CancellationToken => _cancellationTokenSource.Token;

    public void Dispose()
    {
    }

    public void Emit(LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        _channelWriter.TryWrite(logEvent);
    }

    private void ExecuteLogsProcessingLoop(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _batchCycleManager.WhenNextAvailableAsync(cancellationToken);
                await EmitBatchAsync(cancellationToken);
                await _batchCycleManager.OnBatchProcessedAsync(cancellationToken);
            }
        }, CancellationToken.None);
    }

    private async Task EmitBatchAsync(CancellationToken cancellationToken)
    {
        var batchSize = _sinkConfiguration.BatchPostingLimit;
        var logsBatch = await _logsQueueAccessor.DequeueSeveralAsync(batchSize);
        var events = logsBatch.Where(log => log is not null)
            .Select(LogEntry.MapFrom)
            .ToList();

        if (events.Any())
        {
            var messages = _messageFormatter.Format(
                events,
                _sinkConfiguration.FormatterConfiguration);

            foreach (var message in messages)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: _sinkConfiguration.ChatId,
                    text: message,
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
            }
        }
    }
}