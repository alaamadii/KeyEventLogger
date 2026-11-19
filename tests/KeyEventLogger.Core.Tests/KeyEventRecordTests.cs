using Xunit;

namespace KeyEventLogger.Core.Tests;

public sealed class KeyEventRecordTests
{
    [Fact]
    public void RecordPreservesItsValueAndOffset()
    {
        var timestamp = new DateTimeOffset(2026, 11, 19, 12, 34, 56, 789, TimeSpan.FromHours(3));
        var record = new KeyEventRecord("A", KeyEventType.KeyDown, timestamp);

        Assert.Equal("A", record.KeyName);
        Assert.Equal(KeyEventType.KeyDown, record.EventType);
        Assert.True(timestamp.EqualsExact(record.Timestamp));
        Assert.Equal(record, new KeyEventRecord("A", KeyEventType.KeyDown, timestamp));
        Assert.NotEqual(record, new KeyEventRecord("A", KeyEventType.KeyUp, timestamp));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" A")]
    [InlineData("A ")]
    [InlineData("A\nB")]
    [InlineData("A\tB")]
    public void InvalidNamesAreRejected(string? name)
    {
        Assert.ThrowsAny<ArgumentException>(() => new KeyEventRecord(name!, KeyEventType.KeyDown, DateTimeOffset.UtcNow));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void InvalidEventTypesAreRejected(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new KeyEventRecord("A", (KeyEventType)value, DateTimeOffset.UtcNow));
    }
}
