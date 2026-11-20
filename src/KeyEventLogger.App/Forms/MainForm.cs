using KeyEventLogger.App.Export;
using KeyEventLogger.Core;

namespace KeyEventLogger.App.Forms;

public sealed partial class MainForm : Form
{
    private readonly Font bodyFont;
    private readonly Font titleFont;
    private readonly Font emphasisFont;

    public MainForm() : this(TimeProvider.System)
    {
    }

    public MainForm(TimeProvider timeProvider) : this(timeProvider, new SaveFileDestinationPicker(), new TextLogExporter())
    {
    }

    public MainForm(TimeProvider timeProvider, IExportDestinationPicker destinationPicker, ITextLogExporter textExporter, int eventCapacity = RecordingSession.DefaultCapacity)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(destinationPicker);
        ArgumentNullException.ThrowIfNull(textExporter);
        session = new RecordingSession(eventCapacity);
        clock = timeProvider;
        this.destinationPicker = destinationPicker;
        this.textExporter = textExporter;

        SuspendLayout();
        Text = "Key Event Logger";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1040, 720);
        MinimumSize = new Size(900, 680);
        StartPosition = FormStartPosition.CenterScreen;
        bodyFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        Font = bodyFont;
        BackColor = UiTheme.Canvas;
        ForeColor = UiTheme.Text;
        AccessibleDescription = "Educational local keyboard-event recorder. Only the focused practice area is recorded after Start; export requires an explicit local action.";

        titleFont = new Font(Font.FontFamily, 22F, FontStyle.Bold);
        emphasisFont = new Font(Font, FontStyle.Bold);

        var layout = CreateMainLayout();
        var header = CreateHeader();
        layout.Controls.Add(header, 0, 0);

        recordingStatus = CreateLabel("recordingStatus", stoppedReason);
        recordingStatus.Font = emphasisFont;
        recordingStatus.BackColor = UiTheme.Surface;
        recordingStatus.Padding = new Padding(12);
        recordingStatus.Margin = new Padding(0, 16, 0, 12);
        recordingStatus.AccessibleName = "Recording status";
        layout.Controls.Add(recordingStatus, 0, 1);

        var actions = CreateActions(out startButton, out stopButton, out clearButton, out exportButton);
        layout.Controls.Add(actions, 0, 2);

        var availability = CreateLabel("featureAvailability",
            "One press usually creates KeyDown + KeyUp. Held keys may repeat. Export TXT stops recording before choosing a file.");
        availability.ForeColor = UiTheme.Muted;
        availability.Margin = new Padding(0, 10, 0, 18);
        layout.Controls.Add(availability, 0, 3);

        var workspace = new TableLayoutPanel
        {
            Name = "workspace",
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
            TabStop = false
        };
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        workspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var practice = CreatePracticeArea(out practiceText);
        practice.Margin = new Padding(0, 0, 10, 0);
        practice.TabIndex = 0;
        workspace.Controls.Add(practice, 0, 0);

        var events = CreateEventArea(out eventList, out eventCount, out emptyState);
        events.Margin = new Padding(0);
        events.TabIndex = 1;
        workspace.Controls.Add(events, 1, 0);
        layout.Controls.Add(workspace, 0, 4);

        exportFeedback = new Label
        {
            Name = "exportFeedback",
            Text = "No export yet. Events stay in memory until you choose Export TXT.",
            AccessibleName = "Export result",
            AutoEllipsis = true,
            Dock = DockStyle.Fill,
            Height = Font.Height * 2 + 16,
            Padding = new Padding(10, 6, 10, 6),
            BackColor = UiTheme.Surface,
            Margin = new Padding(0, 14, 0, 0),
            UseMnemonic = false
        };
        layout.Controls.Add(exportFeedback, 0, 5);

        var scope = CreateLabel("scopeNotice",
            System.FormattableString.Invariant($"LOCAL ONLY  •  active Practice text only  •  Leaving the window stops recording  •  {session.Capacity:N0}-event session limit"));
        scope.ForeColor = UiTheme.Muted;
        scope.Margin = new Padding(0, 10, 0, 0);
        layout.Controls.Add(scope, 0, 6);

        Controls.Add(layout);
        ActiveControl = practiceText;
        BindRecording();
        ResumeLayout(false);
        PerformLayout();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ReleaseRecording();
            exportFeedbackTip.Dispose();
        }
        base.Dispose(disposing);
        if (disposing)
        {
            titleFont?.Dispose();
            emphasisFont?.Dispose();
            bodyFont?.Dispose();
        }
    }
}
