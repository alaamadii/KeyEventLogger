using System.Globalization;
using System.Text;

namespace KeyEventLogger.Core;

public static class TextLogFormatter
{
    public static string Format(IReadOnlyList<KeyEventRecord> records)
    {
        ArgumentNullException.ThrowIfNull(records);
        var text = new StringBuilder("Key Event Logger\r\n");
        text.Append("Events: ").Append(records.Count.ToString(CultureInfo.InvariantCulture)).Append("\r\n");
        text.Append("Timestamp\tEventType\tKeyName\r\n");
        foreach (var record in records)
        {
            ArgumentNullException.ThrowIfNull(record);
            text.Append(record.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", CultureInfo.InvariantCulture))
                .Append('\t').Append(record.EventType).Append('\t').Append(record.KeyName).Append("\r\n");
        }
        return text.ToString();
    }
}
