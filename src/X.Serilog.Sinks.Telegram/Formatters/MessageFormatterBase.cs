using System.Collections;
using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Formatters;

public abstract class MessageFormatterBase : IMessageFormatter
{
    /// <inheritdoc />
    public virtual string Format(ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, string> formatter = null)
    {
        if (!NotEmpty(logEntries))
        {
            throw new ArgumentException(null, nameof(logEntries));
        }

        if (formatter is null)
        {
            return string.Empty;
        }

        var message = formatter(logEntries, config);
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException(nameof(message));
        }

        return message;
    }

    protected virtual bool NotEmpty<T>(T value)
    {
        switch (value) {
            case null:
            case string s when string.IsNullOrWhiteSpace(s):
            case ICollection { Count: 0 }:
            case Array { Length: 0 }:
            case IEnumerable e when !e.Cast<object>().Any():
                return false;
        }

        return true;
    }

    protected virtual string ToString(LogEventLevel logLevel)
    {
        return logLevel switch
        {
            LogEventLevel.Verbose => "VRB",
            LogEventLevel.Debug => "DBG",
            LogEventLevel.Information => "INF",
            LogEventLevel.Warning => "WARN",
            LogEventLevel.Error => "ERR",
            LogEventLevel.Fatal => "FTL",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }

    protected virtual string ToEmoji(LogEventLevel logLevel)
    {
        return logLevel switch
        {
            LogEventLevel.Verbose => "📝",
            LogEventLevel.Debug => "📓",
            LogEventLevel.Information => "ℹ️",
            LogEventLevel.Warning => "⚠️",
            LogEventLevel.Error => "❗",
            LogEventLevel.Fatal => "☠️️",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }
}