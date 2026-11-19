namespace KeyEventLogger.App.Tests;

internal sealed class ExportTestDirectory : IDisposable
{
    private readonly string container = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../TestResults/export"));

    public ExportTestDirectory()
    {
        Root = Path.Combine(container, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Root);
    }

    public string Root { get; }

    public string FilePath(string name = "sample.keyevents.txt") => Path.Combine(Root, name);

    public void Dispose()
    {
        var resolved = Path.GetFullPath(Root);
        if (!resolved.StartsWith(container + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
            !Guid.TryParseExact(Path.GetFileName(resolved), "N", out _))
        {
            throw new InvalidOperationException("Refusing cleanup outside the isolated export directory.");
        }
        foreach (var path in Directory.GetFiles(resolved))
        {
            File.SetAttributes(path, FileAttributes.Normal);
        }
        Directory.Delete(resolved, recursive: true);
    }
}
