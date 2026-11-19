using Xunit;

namespace KeyEventLogger.Core.Tests;

public sealed class RecordingSessionTests
{
    private static readonly DateTimeOffset Timestamp = new(2026, 11, 19, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void NewSessionIsStoppedAndEmpty()
    {
        var session = new RecordingSession();
        Assert.False(session.IsRecording);
        Assert.Equal(0, session.Count);
        Assert.Empty(session.GetSnapshot());
        Assert.False(session.TryRecord(Record("A")));
        Assert.Equal(0, session.Count);
    }

    [Fact]
    public void RepeatedStartAndStopPreserveExistingEvents()
    {
        var session = new RecordingSession();
        session.Stop();
        session.Start();
        session.Start();
        Assert.True(session.IsRecording);
        var first = Record("A");
        Assert.True(session.TryRecord(first));

        session.Stop();
        session.Stop();
        Assert.False(session.IsRecording);
        Assert.False(session.TryRecord(Record("B")));
        session.Start();
        Assert.True(session.TryRecord(Record("C")));
        Assert.Equal(new[] { "A", "C" }, session.GetSnapshot().Select(record => record.KeyName));
    }

    [Fact]
    public void ArrivalOrderAndRepeatedEventsSurviveClockChanges()
    {
        var session = new RecordingSession();
        session.Start();
        var down = Record("A");
        var earlier = new KeyEventRecord("A", KeyEventType.KeyUp, Timestamp.AddMinutes(-1));
        Assert.True(session.TryRecord(down));
        Assert.True(session.TryRecord(down));
        Assert.True(session.TryRecord(earlier));

        Assert.Equal(3, session.Count);
        Assert.Equal(new[] { down, down, earlier }, session.GetSnapshot());
        Assert.Equal(Timestamp.AddMinutes(-1), session.GetSnapshot()[2].Timestamp);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ClearRemovesEventsWithoutChangingRecordingState(bool recording)
    {
        var session = new RecordingSession();
        session.Start();
        session.TryRecord(Record("A"));
        if (!recording)
        {
            session.Stop();
        }

        session.Clear();
        session.Clear();
        Assert.Equal(recording, session.IsRecording);
        Assert.Equal(0, session.Count);
        Assert.Empty(session.GetSnapshot());
        Assert.Equal(recording, session.TryRecord(Record("B")));
        Assert.Equal(recording ? 1 : 0, session.Count);
    }

    [Fact]
    public void SnapshotIsStableAndCannotMutateTheSession()
    {
        var session = new RecordingSession();
        session.Start();
        var first = Record("A");
        session.TryRecord(first);
        var snapshot = session.GetSnapshot();
        var list = Assert.IsAssignableFrom<IList<KeyEventRecord>>(snapshot);

        Assert.True(list.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => list.Add(Record("B")));
        Assert.Throws<NotSupportedException>(() => list[0] = Record("C"));
        session.TryRecord(Record("D"));
        session.Clear();

        Assert.Equal(first, Assert.Single(snapshot));
        Assert.Empty(session.GetSnapshot());
        Assert.Equal(0, session.Count);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void NullRecordDoesNotPartiallyChangeState(bool recording)
    {
        var session = new RecordingSession();
        session.Start();
        session.TryRecord(Record("A"));
        if (!recording)
        {
            session.Stop();
        }
        var before = session.GetSnapshot();

        Assert.Throws<ArgumentNullException>(() => session.TryRecord(null!));
        Assert.Equal(before, session.GetSnapshot());
        Assert.Equal(recording, session.IsRecording);
        Assert.Equal(1, session.Count);
    }

    [Fact]
    public void InvalidRecordConstructionCannotChangeAnExistingSession()
    {
        var session = new RecordingSession();
        session.Start();
        session.TryRecord(Record("A"));

        Assert.Throws<ArgumentException>(() =>
            session.TryRecord(new KeyEventRecord("A\nB", KeyEventType.KeyDown, Timestamp)));
        Assert.Equal("A", Assert.Single(session.GetSnapshot()).KeyName);
        Assert.True(session.IsRecording);
    }

    private static KeyEventRecord Record(string key) => new(key, KeyEventType.KeyDown, Timestamp);
}
