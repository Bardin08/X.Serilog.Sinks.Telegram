using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Formatters;

internal class DefaultLogFormatter : MessageFormatterBase
{
    /// <inheritdoc cref="MessageFormatterBase"/>
    /// <exception cref="ArgumentNullException">Throws when the log entry is null.</exception>
    /// <exception cref="ArgumentException">Throws when, after using the formatter, the message is null, empty, or whitespace.</exception>
    public override string Format(ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, string> formatter = null)
    {
        formatter ??= DefaultFormatter;
        return base.Format(logEntries, config, formatter);
    }

    private string DefaultFormatter(ICollection<LogEntry> logEntries, FormatterConfiguration config)
    {
        if (logEntries.Count > 1)
        {
            throw new ArgumentException("Formatter supports only single element collections", nameof(logEntries));
        }

        return FormatMessageInternal(logEntries.First(), config);
    }

    private string FormatMessageInternal(LogEntry logEntry, FormatterConfiguration config)
    {
        if (logEntry is null) throw new ArgumentNullException(nameof(logEntry));

        var level = config.UseEmoji ? ToEmoji(logEntry.Level) : ToString(logEntry.Level);

        var sb = new StringBuilder();

        sb.Append(level).Append(' ').Append("<em>[").Append($"{logEntry.UtcTimeStamp:G}").Append("]</em>").Append(' ')
            .Append(config.ReadableApplicationName);

        sb.AppendLine();
        sb.AppendLine();

        if (NotEmpty(logEntry.RenderedMessage))
        {
            sb.Append("<em>").Append("Message: ").Append("</em>").Append("<code>").Append(logEntry.RenderedMessage)
                .Append("</code>").AppendLine();
        }

        if (config.IncludeException &&
            NotEmpty(logEntry.Exception))
        {
            sb.Append("<em>").Append("Exception: ").Append("</em>").Append("<code>").Append(logEntry.Exception)
                .Append("</code>").AppendLine();
        }

        if (config.IncludeProperties &&
            NotEmpty(logEntry.Properties))
        {
            sb.Append("<em>").Append("Properties: ").Append("</em>").AppendLine()
                .Append("<code>").Append(logEntry.Properties).Append("</code>").AppendLine();
        }

        return sb.ToString();
    }
}