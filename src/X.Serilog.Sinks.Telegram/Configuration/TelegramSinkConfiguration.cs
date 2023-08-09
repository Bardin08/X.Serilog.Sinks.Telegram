using X.Serilog.Sinks.Telegram.Batch;

namespace X.Serilog.Sinks.Telegram.Configuration;

public class TelegramSinkConfiguration
{
    private int _batchPostingLimit = TelegramSinkDefaults.BatchPostingLimit;
    private string _chatId = null!;
    private string _token = null!;

    public TelegramSinkConfiguration(ILogsQueueAccessor logsAccessor)
    {
        LogsAccessor = logsAccessor;
    }

    public ILogsQueueAccessor LogsAccessor { get; }

    public string Token
    {
        get => _token;
        set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Invalid token! Token must be not null, empty or whitespace!");
            }

            _token = value;
        }
    }

    public string ChatId
    {
        get => _chatId;
        set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || !long.TryParse(value, out _))
            {
                throw new ArgumentException("Invalid chat id! It must be not null, empty or whitespace " +
                                            "and it's should be a number!");
            }

            _chatId = value;
        }
    }

    public int BatchPostingLimit
    {
        get => _batchPostingLimit;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Invalid batch posting limit! It must be greater than 0!");
            }

            _batchPostingLimit = value;
        }
    }

    public LoggingMode Mode { get; set; }

    public FormatterConfiguration FormatterConfiguration { get; set; } = null!;

    public BatchEmittingRulesConfiguration BatchEmittingRulesConfiguration { get; set; } = null!;

    public LogsFiltersConfiguration LogFiltersConfiguration { get; set; } = null!;

    public void Validate()
    {
        if (string.IsNullOrEmpty(Token) || string.IsNullOrWhiteSpace(Token))
        {
            throw new ArgumentException("Invalid token! Token must be not null, empty or whitespace!");
        }

        if (string.IsNullOrEmpty(ChatId) || string.IsNullOrWhiteSpace(ChatId) || !long.TryParse(ChatId, out _))
        {
            throw new ArgumentException("Invalid chat id! It must be not null, empty or whitespace " +
                                        "and it's should be a number!");
        }

        if (BatchPostingLimit <= 0)
        {
            throw new ArgumentException("Invalid batch posting limit! It must be greater than 0!");
        }

        if (FormatterConfiguration is null)
        {
            throw new ArgumentNullException(nameof(FormatterConfiguration));
        }
    }
}