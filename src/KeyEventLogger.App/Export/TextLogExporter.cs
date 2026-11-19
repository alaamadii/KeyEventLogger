using System.Text;
using KeyEventLogger.Core;

namespace KeyEventLogger.App.Export;

public sealed class TextLogExporter : ITextLogExporter
{
    private readonly Func<string, DriveType> getDriveType;

    public TextLogExporter(Func<string, DriveType>? getDriveType = null)
    {
        this.getDriveType = getDriveType ?? (root => new DriveInfo(root).DriveType);
    }

    public string Export(string destination, IReadOnlyList<KeyEventRecord> snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        var path = LocalExportPath.Resolve(destination, getDriveType);
        var content = TextLogFormatter.Format(snapshot);
        var temporary = Path.Combine(Path.GetDirectoryName(path)!, $".KeyEventLogger-{Guid.NewGuid():N}.tmp");
        var ownsTemporary = false;
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                ownsTemporary = true;
                using var writer = new StreamWriter(stream, new UTF8Encoding(false, true), leaveOpen: true);
                writer.Write(content);
                writer.Flush();
                stream.Flush(flushToDisk: true);
            }
            File.Move(temporary, path, overwrite: true);
            ownsTemporary = false;
            return path;
        }
        finally
        {
            if (ownsTemporary)
            {
                // Delete only the file this operation created; never the destination.
                File.Delete(temporary);
            }
        }
    }
}
