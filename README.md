# X.Serilog.Sinks.Telegram
X.Serilog.Sinks.Telegram is a Serilog sink that write events to [Telegram](https://telegram.org/) channel or chat.

## Table of Contents

- [X.Serilog.Sinks.Telegram](#xserilogsinkstelegram)
  - [Introduction](#introduction)
  - [Features](#features)
  - [Getting Started](#getting-started)
    - [Installation](#installation)
    - [Configuration](#configuration)
  - [Usage](#usage)
  - [Configuration Options](#configuration-options)
  - [Examples](#examples)
  - [Contributing](#contributing)
  - [License](#license)

## Current Statuses
[![Build status](https://ci.appveyor.com/api/projects/status/n4uj9qfuywrkdrhb/branch/main?svg=true)](https://ci.appveyor.com/project/Bardin08/x-serilog-sinks-telegram/branch/main)
[![NuGet Badge](https://buildstats.info/nuget/X.Serilog.Sinks.Telegram)](https://www.nuget.org/packages/X.Serilog.Sinks.Telegram/)

## Documentation
For more comprehensive and detailed documentation, please visit our [GitHub Wiki](https://github.com/Bardin08/X.Serilog.Sinks.Telegram/wiki/Overview) pages. Here, you'll find in-depth information about configuration options, usage examples, troubleshooting guides, and more to help you effectively integrate and utilize the X.Serilog.Sinks.Telegram sink in your projects.

## Getting Started

To begin using the X.Serilog.Sinks.Telegram sink, follow these steps:

1. **Install the Package**: You can install the sink package from NuGet using the following command:
 ```shell
   dotnet add package X.Serilog.Sinks.Telegram
```

2. **Configure the Sink**: In your application's configuration, set up the Telegram sink with the appropriate settings. Here's an example configuration in C#:
```c#
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
```

3. **Start Logging**: Once the sink is configured, you can start logging using Serilog as you normally would. Log events will be sent to your Telegram channel.

For more detailed configuration options, please refer to the [Configuration Wiki](https://github.com/Bardin08/X.Serilog.Sinks.Telegram/wiki/Configuration).

## Roadmap
Project's roadmap described at [Roadmap](./docs/roadmap.md).

## Contributing
Feel free to add any improvements you want via pull requests. All pull requests must be linked to an issue.
