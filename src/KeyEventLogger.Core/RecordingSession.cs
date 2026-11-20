namespace KeyEventLogger.Core;

public sealed class RecordingSession
{
    public const int DefaultCapacity = 10_000;
    private readonly List<KeyEventRecord> records = [];

    public RecordingSession(int capacity = DefaultCapacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        Capacity = capacity;
    }

    public int Capacity { get; }

    public bool IsRecording { get; private set; }

    public int Count => records.Count;

    public bool IsAtCapacity => Count >= Capacity;

    public void Start() => IsRecording = !IsAtCapacity;

    public void Stop() => IsRecording = false;

    public bool TryRecord(KeyEventRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        if (!IsRecording || IsAtCapacity)
        {
            return false;
        }

        records.Add(record);
        if (IsAtCapacity)
        {
            Stop();
        }
        return true;
    }

    public void Clear() => records.Clear();

    public IReadOnlyList<KeyEventRecord> GetSnapshot() => Array.AsReadOnly(records.ToArray());
}
