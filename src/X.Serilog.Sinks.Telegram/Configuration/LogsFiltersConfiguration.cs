using System.Collections.Immutable;
using X.Serilog.Sinks.Telegram.Filters;

namespace X.Serilog.Sinks.Telegram.Configuration;

public class LogsFiltersConfiguration
{
    public bool ApplyLogFilters { get; init; }

    public LogFiltersOperator FiltersOperator { get; init; } = LogFiltersOperator.And;

    public IImmutableList<IFilter>? Filters { get; init; }
}