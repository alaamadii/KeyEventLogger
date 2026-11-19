using KeyEventLogger.Core;

namespace KeyEventLogger.App.Export;

public interface ITextLogExporter
{
    string Export(string destination, IReadOnlyList<KeyEventRecord> snapshot);
}
