namespace KeyEventLogger.App.Export;

internal static class ExportFailure
{
    public static string Describe(Exception error)
    {
        var reason = error switch
        {
            UnauthorizedAccessException => "Access was denied. Choose a writable local file or folder.",
            System.Security.SecurityException => "Windows blocked access to the selected destination.",
            ArgumentException or NotSupportedException => "Choose a supported local .txt destination.",
            System.Runtime.InteropServices.ExternalException => "Windows could not complete the save dialog.",
            _ => "Windows could not complete the write. Check the destination and close any application using the file."
        };
        return $"Export failed: {reason} Records were kept; recording remains stopped. Choose another destination and retry.";
    }
}
