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

    protected async Task SendLog<T>(IEnumerable<T> logEntries) where T : LogEntry
    {
        var messages = GetMessages((ICollection<LogEntry>)logEntries.ToList());
        foreach (var message in messages)
        {
            await _botClient.SendTextMessageAsync(_config.ChatId, message, ParseMode.Html);
        }
    }

    private List<string> GetMessages(ICollection<LogEntry> entries)
    {
        var messages = new List<string>();

        switch (_config.Mode)
        {
            case LoggingMode.Logs:
                var formattedMessages = entries
                    .Select(entry => _messageFormatter.Format(new[] { entry }, _config.FormatterConfiguration))
                    .ToList();
                messages.AddRange(formattedMessages);
                break;
            case LoggingMode.AggregatedNotifications:
                var message = _messageFormatter
                    .Format(entries, _config.FormatterConfiguration);
                messages.Add(message);
                break;
        }

        return messages;
    }
}