using System.Globalization;
using KeyEventLogger.Core;

namespace KeyEventLogger.App.Forms;

public sealed partial class MainForm
{
    private readonly RecordingSession session;
    private readonly TimeProvider clock;
    private readonly TextBox practiceText;
    private readonly ListView eventList;
    private readonly Label recordingStatus;
    private readonly Label eventCount;
    private readonly Label emptyState;
    private readonly Button startButton;
    private readonly Button stopButton;
    private readonly Button clearButton;
    private readonly Button exportButton;
    private bool recordingReady;
    private bool windowActive;
    private bool closing;
    private string stoppedReason = "Stopped — press Start to record practice-area keys.";

    private void BindRecording()
    {
        practiceText.KeyDown += PracticeKeyDown;
        practiceText.KeyUp += PracticeKeyUp;
        startButton.Click += StartClicked;
        stopButton.Click += StopClicked;
        clearButton.Click += ClearClicked;
        exportButton.Click += ExportClicked;
        recordingReady = true;
        RefreshPresentation();
    }

    private void StartClicked(object? sender, EventArgs e)
    {
        if (closing || exportInProgress || !windowActive || !ContainsFocus || WindowState == FormWindowState.Minimized)
        {
            return;
        }

        session.Start();
        RefreshPresentation();
        practiceText.Focus();
    }

    private void StopClicked(object? sender, EventArgs e) =>
        StopRecording("Stopped — press Start to resume. Previous events are kept.");

    private void ClearClicked(object? sender, EventArgs e)
    {
        if (exportInProgress || closing) { return; }
        var wasAtCapacity = session.IsAtCapacity;
        session.Clear();
        if (wasAtCapacity)
        {
            stoppedReason = "Stopped — events cleared. Press Start to record again.";
        }
        eventList.Items.Clear();
        RefreshPresentation();
        if (session.IsRecording)
        {
            practiceText.Focus();
        }
    }

    private void PracticeKeyDown(object? sender, KeyEventArgs e) => RecordEvent(e, KeyEventType.KeyDown);

    private void PracticeKeyUp(object? sender, KeyEventArgs e) => RecordEvent(e, KeyEventType.KeyUp);

    private void RecordEvent(KeyEventArgs e, KeyEventType eventType)
    {
        if (!recordingReady || closing || IsDisposed || Disposing || !session.IsRecording ||
            !windowActive || !ContainsFocus || WindowState == FormWindowState.Minimized || !practiceText.Focused)
        {
            return;
        }

        var record = new KeyEventRecord(e.KeyCode.ToString(), eventType, clock.GetLocalNow());
        if (session.TryRecord(record))
        {
            var row = new ListViewItem(record.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", CultureInfo.InvariantCulture))
            {
                Tag = record,
                BackColor = session.Count % 2 == 0 ? UiTheme.AlternateRow : UiTheme.Surface,
                ToolTipText = eventType == KeyEventType.KeyDown
                    ? "KeyDown: the key was pressed; held keys can repeat."
                    : "KeyUp: the key was released."
            };
            row.SubItems.Add(record.EventType.ToString());
            row.SubItems.Add(record.KeyName);
            eventList.Items.Add(row);
            row.EnsureVisible();
            if (session.IsAtCapacity)
            {
                stoppedReason = string.Create(CultureInfo.InvariantCulture,
                    $"Stopped — session limit reached ({session.Capacity:N0} events). Export or clear events before starting again.");
            }
            RefreshPresentation();
        }
    }

    private void StopRecording(string reason)
    {
        session.Stop();
        stoppedReason = reason;
        if (recordingReady)
        {
            RefreshPresentation();
        }
    }

    private void RefreshPresentation()
    {
        recordingStatus.Text = session.IsRecording
            ? "Recording — practice area only."
            : stoppedReason;
        recordingStatus.AccessibleDescription = recordingStatus.Text;
        recordingStatus.BackColor = session.IsRecording ? UiTheme.RecordingSurface : UiTheme.Surface;
        recordingStatus.ForeColor = session.IsRecording ? UiTheme.Recording : UiTheme.Text;
        startButton.Enabled = !session.IsRecording && !session.IsAtCapacity && !closing && !exportInProgress;
        startButton.BackColor = startButton.Enabled ? UiTheme.Accent : SystemColors.Control;
        startButton.ForeColor = startButton.Enabled ? UiTheme.OnAccent : SystemColors.GrayText;
        stopButton.Enabled = session.IsRecording && !closing && !exportInProgress;
        clearButton.Enabled = session.Count > 0 && !closing && !exportInProgress;
        exportButton.Enabled = session.Count > 0 && !closing && !exportInProgress;
        eventCount.Text = string.Create(CultureInfo.InvariantCulture, $"{session.Count} {(session.Count == 1 ? "event" : "events")}");
        emptyState.Visible = session.Count == 0;
    }

    protected override void OnActivated(EventArgs e)
    {
        windowActive = true;
        base.OnActivated(e);
    }

    protected override void OnDeactivate(EventArgs e)
    {
        windowActive = false;
        if (recordingReady)
        {
            StopRecording("Stopped — focus left this window. Press Start to resume.");
        }
        base.OnDeactivate(e);
    }

    protected override void OnResize(EventArgs e)
    {
        if (recordingReady && WindowState == FormWindowState.Minimized)
        {
            StopRecording("Stopped — window minimized. Press Start to resume.");
        }
        base.OnResize(e);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        closing = true;
        if (recordingReady)
        {
            StopRecording("Stopped — window is closing.");
        }
        base.OnFormClosing(e);
        if (e.Cancel)
        {
            closing = false;
            if (recordingReady)
            {
                RefreshPresentation();
            }
        }
    }

    private void ReleaseRecording()
    {
        closing = true;
        windowActive = false;
        session.Stop();
        if (!recordingReady)
        {
            return;
        }

        recordingReady = false;
        practiceText.KeyDown -= PracticeKeyDown;
        practiceText.KeyUp -= PracticeKeyUp;
        startButton.Click -= StartClicked;
        stopButton.Click -= StopClicked;
        clearButton.Click -= ClearClicked;
        exportButton.Click -= ExportClicked;
    }
}
