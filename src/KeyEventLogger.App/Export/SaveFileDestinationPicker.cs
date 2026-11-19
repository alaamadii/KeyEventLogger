namespace KeyEventLogger.App.Export;

public sealed class SaveFileDestinationPicker : IExportDestinationPicker
{
    public string? SelectDestination(IWin32Window owner)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Export recorded events",
            Filter = "Text files (*.txt)|*.txt",
            DefaultExt = "txt",
            FileName = "KeyEventLog.keyevents.txt",
            AddExtension = true,
            OverwritePrompt = true,
            CheckPathExists = true,
            RestoreDirectory = true,
            ValidateNames = true
        };
        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.FileName : null;
    }
}
