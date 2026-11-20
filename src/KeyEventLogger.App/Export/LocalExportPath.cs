namespace KeyEventLogger.App.Export;

internal static class LocalExportPath
{
    public static string Resolve(string destination, Func<string, DriveType> driveType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destination);
        if (!Path.IsPathFullyQualified(destination) || destination.StartsWith(@"\\", StringComparison.Ordinal))
        {
            throw new ArgumentException("Choose a fully qualified local drive path, not a network or device path.");
        }

        var fullPath = Path.GetFullPath(destination);
        var root = Path.GetPathRoot(fullPath)!;
        if (root.Length != 3 || root[1] != ':' || !char.IsAsciiLetter(root[0]))
        {
            throw new ArgumentException("Choose a normal local drive path.");
        }

        if (!string.Equals(Path.GetExtension(fullPath), ".txt", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Choose a destination ending in .txt.");
        }

        var parts = fullPath[root.Length..].Split(Path.DirectorySeparatorChar);
        foreach (var part in parts)
        {
            var device = part.Split('.')[0].TrimEnd(' ').ToUpperInvariant();
            if (part.Length == 0 || part.EndsWith(' ') || part.EndsWith('.') ||
                part.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                device is "CON" or "PRN" or "AUX" or "NUL" ||
                (device.Length == 4 && (device.StartsWith("COM", StringComparison.Ordinal) ||
                device.StartsWith("LPT", StringComparison.Ordinal)) && (device[3] is >= '1' and <= '9' or '¹' or '²' or '³')))
            {
                throw new ArgumentException("The destination contains an unsupported file or folder name.");
            }
        }

        if (driveType(root) is not (DriveType.Fixed or DriveType.Removable or DriveType.Ram))
        {
            throw new ArgumentException("Choose an available writable local drive. Network drives are not supported.");
        }
        RejectLink(File.GetAttributes(root));
        var directory = root;
        foreach (var part in parts[..^1])
        {
            directory = Path.Combine(directory, part);
            var attributes = File.GetAttributes(directory);
            if ((attributes & FileAttributes.Directory) == 0)
            {
                throw new DirectoryNotFoundException("The destination folder does not exist.");
            }
            RejectLink(attributes);
        }
        try
        {
            var attributes = File.GetAttributes(fullPath);
            RejectLink(attributes);
            if ((attributes & FileAttributes.Directory) != 0)
            {
                throw new IOException("Choose a text file, not an existing folder.");
            }
        }
        catch (FileNotFoundException)
        {
            // A new destination is expected; do not create it until staging succeeds.
        }
        return fullPath;
    }

    private static void RejectLink(FileAttributes attributes)
    {
        if ((attributes & FileAttributes.ReparsePoint) != 0)
        {
            throw new ArgumentException("Linked or redirected destinations are not supported. Choose a normal local folder.");
        }
    }
}
