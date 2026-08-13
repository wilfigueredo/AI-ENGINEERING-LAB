using AiEngineeringLab.Plugins.DateTimeTools;
using Microsoft.Extensions.Logging.Abstractions;

namespace AiEngineeringLab.UnitTests.Plugins;

public sealed class DateTimePluginTests
{

    [Fact]
    public void CalculateDaysBetweenDates_ShouldReturnDifferenceInDays()
    {
        var plugin = new DateTimePlugin(
        NullLogger<DateTimePlugin>.Instance);

        var startDate = new DateTime(2026, 8, 1);
        var endDate = new DateTime(2026, 8, 10);

        var result = plugin.CalculateDaysBetweenDates(
            startDate,
            endDate);

        Assert.Equal(9, result);
    }

    [Fact]
    public void CalculateDaysBetweenDates_ShouldReturnAbsoluteDifference()
    {
        var plugin = new DateTimePlugin(
        NullLogger<DateTimePlugin>.Instance);

        var startDate = new DateTime(2026, 8, 10);
        var endDate = new DateTime(2026, 8, 1);

        var result = plugin.CalculateDaysBetweenDates(
            startDate,
            endDate);

        Assert.Equal(9, result);
    }
}
