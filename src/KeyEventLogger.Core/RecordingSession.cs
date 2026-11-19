namespace KeyEventLogger.Core;

public sealed class RecordingSession
{
    private readonly List<KeyEventRecord> records = [];

    public bool IsRecording { get; private set; }

    public int Count => records.Count;

    public void Start() => IsRecording = true;

    public void Stop() => IsRecording = false;

    public bool TryRecord(KeyEventRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        if (!IsRecording)
        {
            return false;
        }

        records.Add(record);
        return true;
    }

    public void Clear() => records.Clear();

    public IReadOnlyList<KeyEventRecord> GetSnapshot() => Array.AsReadOnly(records.ToArray());
}
