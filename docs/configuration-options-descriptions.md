# Documentation: Telegram Sink Configuration Settings

In the context of the Telegram Sink for logging, several configuration classes and enums are available. These classes provide a way to configure the sink according to specific needs.

## Table of Contents

1. [TelegramSinkConfiguration](#telegramsinkconfiguration)
2. [BatchEmittingRulesConfiguration](#batchemittingrulesconfiguration)
3. [FormatterConfiguration](#formatterconfiguration)
4. [LoggingMode](#loggingmode)
5. [LogsFiltersConfiguration](#logsfiltersconfiguration)
6. [LogFiltersOperator](#logfiltersoperator)
7. [TelegramSinkDefaults](#telegramsinkdefaults)
8. [Usage Examples](#usage-examples)

<a name="telegramsinkconfiguration"></a>
## TelegramSinkConfiguration

This is the primary configuration class which contains settings required for the operation of the Telegram Sink.

| Property                        | Type                            | Description                                               |
|---------------------------------|---------------------------------|-----------------------------------------------------------|
| Token                           | string                          | Token for Telegram API access.                            |
| ChatId                          | string                          | Identifies the chat to which log messages will be posted. |
| BatchPostingLimit               | int                             | The maximum number of events to post in a single batch.   |
| Mode                            | LoggingMode                     | Sets the logging mode.                                    |
| FormatterConfiguration          | FormatterConfiguration          | Configuration for formatting logs.                        |
| BatchEmittingRulesConfiguration | BatchEmittingRulesConfiguration | Configuration for rules on emitting batches.              |
| LogFiltersConfiguration         | LogsFiltersConfiguration        | Configuration for filtering logs.                         |

<a name="batchemittingrulesconfiguration"></a>
## BatchEmittingRulesConfiguration

Configuration class for managing batch emitting rules.

| Property             | Type                  | Description                               |
|----------------------|-----------------------|-------------------------------------------|
| RuleCheckPeriod      | TimeSpan              | Check period for batch emit rules.        |
| BatchProcessingRules | IImmutableList<IRule> | The batch processing rules to be applied. |

<a name="formatterconfiguration"></a>
## FormatterConfiguration

Settings for formatting log messages.

| Property                | Type   | Description                                                   |
|-------------------------|--------|---------------------------------------------------------------|
| UseEmoji                | bool   | Indicates whether to use emojis in the output.                |
| ReadableApplicationName | string | User-friendly name of the application.                        |
| IncludeException        | bool   | Indicates whether to include exception details in the output. |
| IncludeProperties       | bool   | Indicates whether to include property details in the output.  |

<a name="loggingmode"></a>
## LoggingMode

An enumeration that allows choosing between two ways of logging.

A. Logs: Log messages will be published to the specified Telegram channel.<br/>
B. AggregatedNotifications: Messages will contain an info about all notifications which were received during a batch period or batch limit.

<a name="logsfiltersconfiguration"></a>
## LogsFiltersConfiguration

Configuration class for applying logs filters.

| Property        | Type                    | Description                             |
|-----------------|-------------------------|-----------------------------------------|
| ApplyLogFilters | bool                    | Indicates whether to apply log filters. |
| FiltersOperator | LogFiltersOperator      | Operator used for combining filters.    |
| Filters         | IImmutableList<IFilter> | List of filters to be applied.          |

<a name="logfiltersoperator"></a>
## LogFiltersOperator

An enumeration that allows choosing a logical operator for combining filters.

A. And: All filters have to pass.
B. Or: At least one of the filters has to pass.

<a name="telegramsinkdefaults"></a>
## TelegramSinkDefaults

Contains the default configuration values for the Telegram Sink.

<a name="usage-examples"></a>
## Usage Examples

Here is an example of how to configure the Telegram Sink using an extension method on the LoggerConfiguration class:

```csharp
var logger = new LoggerConfiguration()
    .Telegram(config =>
    {
        config.Token = "your_telegram_bot_token";
        config.ChatId = "your_chat_id";
        config.BatchPostingLimit = 10;
        config.Mode = LoggingMode.Logs;
        config.FormatterConfiguration = new FormatterConfiguration
        {
            UseEmoji = true,
            ReadableApplicationName = "MyTestApp",
            IncludeException = true,
            IncludeProperties = true
        };
        config.BatchEmittingRulesConfiguration = new BatchEmittingRulesConfiguration
        {
            // Batch Emitting rules configuration here...
        };
        config.LogFiltersConfiguration = new LogsFiltersConfiguration
        {
            ApplyLogFilters = true,
            FiltersOperator = LogFiltersOperator.Or,
            Filters = new List<IFilter> {
                // Your filters here...
            }
        };
    }, null, LogEventLevel.Debug)
    .CreateLogger();

logger.Information("This is a test log message");
```

This code configures the TelegramSinkConfiguration instance through a delegate and an extension method. This
provides  more fluent API and makes the configuration step more clear and straightforward. You would add further
sinks and then create the logger from your loggerConfiguration.

In this example, `new MyCustomMessageFormatter()` is used for message formatting. You should replace 
`MyCustomMessageFormatter` with your actual implementation of the `IMessageFormatte`r or leave it `null` to use one of the 
default one. 

Also, add your own batch emitting rules and filters as per your requirements.
Make sure to replace `your_telegram_bot_token` and `your_chat_id` with your actual Telegram bot token and chat ID.


**Important:** Always remember to validate the configuration towards the end, before using it to instantiate an 
object. This is to ensure that the provided settings are valid and appropriate for the operation of the Telegram sink.