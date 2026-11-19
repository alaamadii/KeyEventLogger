using System.Runtime.InteropServices;
using System.Globalization;
using System.Security;
using KeyEventLogger.App.Export;

namespace KeyEventLogger.App.Forms;

public sealed partial class MainForm
{
    private readonly IExportDestinationPicker destinationPicker;
    private readonly ITextLogExporter textExporter;
    private readonly Label exportFeedback;
    private readonly ToolTip exportFeedbackTip = new();
    private bool exportInProgress;

    private void ExportClicked(object? sender, EventArgs e)
    {
        if (exportInProgress || closing || IsDisposed || session.Count == 0)
        {
            return;
        }

        exportInProgress = true;
        StopRecording("Stopped — export requested. Press Start to resume.");
        var snapshot = session.GetSnapshot();
        try
        {
            var destination = destinationPicker.SelectDestination(this);
            if (closing || IsDisposed || Disposing)
            {
                return;
            }
            if (destination is null)
            {
                ShowExportFeedback("Export canceled. No file was written; records were kept.");
                return;
            }

            var savedPath = textExporter.Export(destination, snapshot);
            ShowExportFeedback(string.Create(CultureInfo.InvariantCulture,
                $"Exported {snapshot.Count} events to {savedPath}"));
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or ArgumentException
            or NotSupportedException or SecurityException or ExternalException)
        {
            if (!closing && !IsDisposed && !Disposing)
            {
                ShowExportFeedback($"Export failed: {error.Message} Records were kept. Choose another destination and retry.");
            }
        }
        finally
        {
            exportInProgress = false;
            if (!closing && !IsDisposed && !Disposing)
            {
                RefreshPresentation();
            }
        }
    }

    private void ShowExportFeedback(string message)
    {
        if (closing || IsDisposed || Disposing) { return; }
        exportFeedback.Text = message;
        exportFeedback.AccessibleDescription = message;
        exportFeedbackTip.SetToolTip(exportFeedback, message);
    }
}
