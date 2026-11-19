namespace KeyEventLogger.App.Export;

public interface IExportDestinationPicker
{
    string? SelectDestination(IWin32Window owner);
}
