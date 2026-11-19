using System.Globalization;
using Xunit;

namespace KeyEventLogger.Core.Tests;

public sealed class TextLogFormatterTests
{
    private static readonly DateTimeOffset Time = new(2026, 11, 19, 12, 34, 56, 789, TimeSpan.FromHours(3));

    [Fact]
    public void ExactFormatIncludesHeaderCountTabsAndFinalCrLf()
    {
        var records = new[]
        {
            new KeyEventRecord("A", KeyEventType.KeyDown, Time),
            new KeyEventRecord("A", KeyEventType.KeyUp, Time.AddMilliseconds(1))
        };
        Assert.Equal("Key Event Logger\r\nEvents: 2\r\nTimestamp\tEventType\tKeyName\r\n" +
            "2026-11-19 12:34:56.789 +03:00\tKeyDown\tA\r\n" +
            "2026-11-19 12:34:56.790 +03:00\tKeyUp\tA\r\n", TextLogFormatter.Format(records));
    }

    [Fact]
    public void EmptySnapshotStillHasACompleteHeader()
    {
        Assert.Equal("Key Event Logger\r\nEvents: 0\r\nTimestamp\tEventType\tKeyName\r\n",
            TextLogFormatter.Format(Array.Empty<KeyEventRecord>()));
    }

    [Theory]
    [InlineData("ar-SA")]
    [InlineData("tr-TR")]
    public void CultureDoesNotChangeDigitsDateOrOffset(string culture)
    {
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            var record = new KeyEventRecord("Ö", KeyEventType.KeyDown, Time.ToOffset(TimeSpan.FromHours(-5.5)));
            Assert.EndsWith("2026-11-19 04:04:56.789 -05:30\tKeyDown\tÖ\r\n", TextLogFormatter.Format([record]));
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }

    [Fact]
    public void RepeatsUnicodeAndBackwardClockArrivalOrderArePreserved()
    {
        var first = new KeyEventRecord("Ö", KeyEventType.KeyDown, Time);
        var last = new KeyEventRecord("أ", KeyEventType.KeyUp, Time.AddMinutes(-1));
        var records = new[] { first, first, last };
        var lines = TextLogFormatter.Format(records).Split("\r\n");
        Assert.Equal(lines[3], lines[4]);
        Assert.EndsWith("\tKeyDown\tÖ", lines[3]);
        Assert.EndsWith("\tKeyUp\tأ", lines[5]);
        Assert.Contains("12:33:56.789", lines[5]);
        Assert.Equal(new[] { first, first, last }, records);
    }

    [Fact]
    public void NullSnapshotIsRejected()
    {
        Assert.Throws<ArgumentNullException>(() => TextLogFormatter.Format(null!));
    }

    [Fact]
    public void NullRecordIsRejected()
    {
        Assert.Throws<ArgumentNullException>(() => TextLogFormatter.Format(new KeyEventRecord[] { null! }));
    }
}
