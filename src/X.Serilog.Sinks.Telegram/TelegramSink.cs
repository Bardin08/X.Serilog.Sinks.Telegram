using Serilog.Sinks.PeriodicBatching;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram;

public class TelegramSink : TelegramSinkBase
{
    public TelegramSink(IMessageFormatter messageFormatter, TelegramSinkConfiguration config)
        : base(messageFormatter, config)
    {
    }

    protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
    {
        await SendLog(events.Select(LogEntry.MapFrom));
    }
}

public class TelegramSinkBase : PeriodicBatchingSink
{
    private readonly IMessageFormatter _messageFormatter;
    private readonly TelegramSinkConfiguration _config;
    private readonly ITelegramBotClient _botClient;

    protected TelegramSinkBase(IMessageFormatter messageFormatter, TelegramSinkConfiguration config)
        : base(config.BatchPostingLimit, config.BatchPeriod)
    {
        _config = config;
        _messageFormatter = messageFormatter ?? TelegramSinkDefaults.GetDefaultMessageFormatter(_config.Mode);
        _botClient = new TelegramBotClient(_config.Token);
    }

    protected async Task SendLog<T>(IEnumerable<T> logEntries) where T: LogEntry
    {
        await Task.Run(() =>
        {
            var messages = GetMessages(logEntries.ToList());
            foreach (var message in messages)
            {
                _botClient.SendTextMessageAsync(_config.ChatId, message, ParseMode.Html);
            }
        });
    }

    private List<string> GetMessages(IReadOnlyCollection<LogEntry> entries)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (_config.Mode)
        {
            case LoggingMode.Logs:
            case LoggingMode.Notifications:
                var messages = new List<string>(entries.Count);
                messages.AddRange(entries.Select(entry =>
                    _messageFormatter.Format(entry, _config.FormatterConfiguration)));

                return messages;
            case LoggingMode.AggregatedNotifications:
                return new List<string>(1)
                {
                    _messageFormatter.Format(entries, _config.FormatterConfiguration),
                };
        }

        return new List<string>();
    }
}