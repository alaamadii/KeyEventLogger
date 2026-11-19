namespace KeyEventLogger.Core;

public sealed record KeyEventRecord
{
    public KeyEventRecord(string keyName, KeyEventType eventType, DateTimeOffset timestamp)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(keyName);
        if (keyName != keyName.Trim() || keyName.Any(char.IsControl))
        {
            throw new ArgumentException("Use a trimmed key name without control characters.", nameof(keyName));
        }
        if (!Enum.IsDefined(eventType))
        {
            throw new ArgumentOutOfRangeException(nameof(eventType));
        }

        KeyName = keyName;
        EventType = eventType;
        Timestamp = timestamp;
    }

    public string KeyName { get; }

    public KeyEventType EventType { get; }

    public DateTimeOffset Timestamp { get; }
}
