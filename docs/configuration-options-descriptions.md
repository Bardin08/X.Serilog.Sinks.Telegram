# Documentation: Telegram Sink Configuration Settings

In the context of the Telegram Sink for logging, several configuration classes and enums are available. These classes provide a way to configure the sink according to specific needs.

## Table of Contents

1. [TelegramSinkConfiguration](#telegramsinkconfiguration)
2. [BatchEmittingRulesConfiguration](#batchemittingrulesconfiguration)
3. [FormatterConfiguration](#formatterconfiguration)
4. [LoggingMode](#loggingmode)
5. [LogsFiltersConfiguration](#logsfiltersconfiguration)
6. [TelegramSinkDefaults](#telegramsinkdefaults)
7. [Usage Examples](#usage-examples)

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

<a name="telegramsinkdefaults"></a>
## TelegramSinkDefaults

Contains the default configuration values for the Telegram Sink.

<a name="usage-examples"></a>
## Usage Examples

Here is an example of how to use these classes:

```C#
var logsAccessor = new LogsQueueAccessor();
var telegramSinkConfiguration = new TelegramSinkConfiguration(logsAccessor)
{
    Token = "Your_Telegram_Token",
    ChatId = "Your_Chat_Id",
    BatchPostingLimit = 10,
    Mode = LoggingMode.Logs,
    FormatterConfiguration = new FormatterConfiguration
    {
        UseEmoji = true,
        ReadableApplicationName = "My Application",
        IncludeException = true,
        IncludeProperties = true
    },
    // Set the rest of the properties ...
};

// Validate the configuration
Validate()
```

This code configures the TelegramSinkConfiguration instance and then creates a new TelegramSink instance by passing the configuration to its constructor.
Please note that actual usage might require setting other properties on the TelegramSinkConfiguration instance as well depending upon the requirements of your logging strategy.<br/>
**Important:** Always remember to validate the configuration towards the end, before using it to instantiate an object. This is to ensure that the provided settings are valid and appropriate for the operation of the Telegram sink.