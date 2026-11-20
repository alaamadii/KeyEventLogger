using System.Text;
using KeyEventLogger.App.Export;
using KeyEventLogger.Core;
using Xunit;

namespace KeyEventLogger.App.Tests;

public sealed class TextLogExporterTests
{
    private static KeyEventRecord[] Records() =>
        [new("Ö", KeyEventType.KeyDown, new DateTimeOffset(2026, 11, 19, 12, 0, 0, TimeSpan.FromHours(3)))];

    [Fact]
    public void NewFileHasExactUtf8WithoutBomAndDoesNotMutateSession()
    {
        using var directory = new ExportTestDirectory();
        var session = new RecordingSession();
        session.Start();
        session.TryRecord(Records()[0]);
        var snapshot = session.GetSnapshot();
        var destination = new TextLogExporter().Export(directory.FilePath(), snapshot);

        Assert.Equal(directory.FilePath(), destination);
        var bytes = File.ReadAllBytes(destination);
        Assert.Equal(new UTF8Encoding(false, true).GetBytes(TextLogFormatter.Format(snapshot)), bytes);
        Assert.False(bytes.Take(3).SequenceEqual(new byte[] { 0xEF, 0xBB, 0xBF }));
        Assert.Contains("Ö", File.ReadAllText(destination, Encoding.UTF8));
        Assert.True(session.IsRecording);
        Assert.Equal(1, session.Count);
        Assert.Single(Directory.GetFiles(directory.Root));
    }

    [Fact]
    public void SuccessfulOverwriteCommitsCompleteSnapshot()
    {
        using var directory = new ExportTestDirectory();
        File.WriteAllText(directory.FilePath(), "Old synthetic content");
        new TextLogExporter().Export(directory.FilePath(), Records());
        Assert.Equal(TextLogFormatter.Format(Records()), File.ReadAllText(directory.FilePath()));
        Assert.Single(Directory.GetFiles(directory.Root));
    }

    [Fact]
    public void MissingDirectoryIsNotCreated()
    {
        using var directory = new ExportTestDirectory();
        var missing = Path.Combine(directory.Root, "missing");
        Assert.ThrowsAny<IOException>(() => new TextLogExporter().Export(Path.Combine(missing, "log.txt"), Records()));
        Assert.False(Directory.Exists(missing));
        Assert.Empty(Directory.GetFiles(directory.Root));
    }

    [Fact]
    public void LockedDestinationSurvivesReplacementFailureAndTemporaryIsRemoved()
    {
        using var directory = new ExportTestDirectory();
        File.WriteAllText(directory.FilePath(), "Previous synthetic data");
        using (var locked = new FileStream(directory.FilePath(), FileMode.Open, FileAccess.Read, FileShare.None))
        {
            var error = Record.Exception(() => new TextLogExporter().Export(directory.FilePath(), Records()));
            Assert.True(error is IOException or UnauthorizedAccessException);
        }
        Assert.Equal("Previous synthetic data", File.ReadAllText(directory.FilePath()));
        Assert.Single(Directory.GetFiles(directory.Root));
    }

    [Fact]
    public void ReadOnlyDestinationSurvivesAndCanBeRetried()
    {
        using var directory = new ExportTestDirectory();
        File.WriteAllText(directory.FilePath(), "Previous synthetic data");
        File.SetAttributes(directory.FilePath(), FileAttributes.ReadOnly);
        Assert.Throws<UnauthorizedAccessException>(() => new TextLogExporter().Export(directory.FilePath(), Records()));
        Assert.Equal("Previous synthetic data", File.ReadAllText(directory.FilePath()));
        Assert.Single(Directory.GetFiles(directory.Root));
        File.SetAttributes(directory.FilePath(), FileAttributes.Normal);
        new TextLogExporter().Export(directory.FilePath(), Records());
        Assert.Equal(TextLogFormatter.Format(Records()), File.ReadAllText(directory.FilePath()));
    }

    [Fact]
    public void EncodingFailurePreservesOldFileAndRemovesOwnedTemporary()
    {
        using var directory = new ExportTestDirectory();
        File.WriteAllText(directory.FilePath(), "Previous synthetic data");
        var record = new KeyEventRecord("\uD800", KeyEventType.KeyDown, DateTimeOffset.UtcNow);
        Assert.Throws<EncoderFallbackException>(() => new TextLogExporter().Export(directory.FilePath(), [record]));
        Assert.Equal("Previous synthetic data", File.ReadAllText(directory.FilePath()));
        Assert.Single(Directory.GetFiles(directory.Root));
    }

    [Fact]
    public void InvalidSnapshotDoesNotTouchDestination()
    {
        using var directory = new ExportTestDirectory();
        File.WriteAllText(directory.FilePath(), "Previous synthetic data");
        Assert.Throws<ArgumentNullException>(() => new TextLogExporter().Export(directory.FilePath(), [null!]));
        Assert.Throws<ArgumentNullException>(() => new TextLogExporter().Export(directory.FilePath(), null!));
        Assert.Equal("Previous synthetic data", File.ReadAllText(directory.FilePath()));
        Assert.Single(Directory.GetFiles(directory.Root));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("relative.txt")]
    [InlineData(@"\\server\share\log.txt")]
    [InlineData(@"\\?\C:\log.txt")]
    public void UnsupportedPathsAreRejectedBeforeDriveOrFileAccess(string? path)
    {
        var queriedDrive = false;
        var exporter = new TextLogExporter(_ => { queriedDrive = true; return DriveType.Fixed; });
        Assert.ThrowsAny<ArgumentException>(() => exporter.Export(path!, Records()));
        Assert.False(queriedDrive);
    }

    [Theory]
    [InlineData(DriveType.Network)]
    [InlineData(DriveType.NoRootDirectory)]
    public void UnsupportedDriveMetadataIsRejectedWithoutWriting(DriveType type)
    {
        using var directory = new ExportTestDirectory();
        var exporter = new TextLogExporter(_ => type);
        Assert.Throws<ArgumentException>(() => exporter.Export(directory.FilePath(), Records()));
        Assert.Empty(Directory.GetFiles(directory.Root));
    }

    [Theory]
    [InlineData("log.csv")]
    [InlineData("CON.txt")]
    [InlineData("log.txt:stream.txt")]
    [InlineData("bad|name.txt")]
    public void UnsupportedFileNamesAreRejected(string name)
    {
        using var directory = new ExportTestDirectory();
        Assert.Throws<ArgumentException>(() => new TextLogExporter().Export(directory.FilePath(name), Records()));
        Assert.Empty(Directory.GetFiles(directory.Root));
    }

    [Theory]
    [InlineData("COM¹.txt")]
    [InlineData("COM².txt")]
    [InlineData("COM³.txt")]
    [InlineData("LPT¹.txt")]
    [InlineData("LPT².txt")]
    [InlineData("LPT³.txt")]
    [InlineData("CON .txt")]
    [InlineData("LPT1 .txt")]
    public void ReservedDeviceAliasesAreRejectedBeforeDriveOrFileAccess(string name)
    {
        var queriedDrive = false;
        var exporter = new TextLogExporter(_ => { queriedDrive = true; return DriveType.Fixed; });
        Assert.Throws<ArgumentException>(() => exporter.Export(Path.Combine(@"C:\owned-demo", name), Records()));
        Assert.False(queriedDrive);
    }
}
