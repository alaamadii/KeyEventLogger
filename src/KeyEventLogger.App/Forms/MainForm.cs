namespace KeyEventLogger.App.Forms;

public sealed partial class MainForm : Form
{
    private readonly Font titleFont;
    private readonly Font emphasisFont;

    public MainForm() : this(TimeProvider.System)
    {
    }

    public MainForm(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        clock = timeProvider;
        SuspendLayout();
        Text = "Key Event Logger";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(960, 720);
        MinimumSize = new Size(820, 680);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = SystemColors.Window;
        ForeColor = SystemColors.WindowText;
        AccessibleDescription = "Educational keyboard-event recorder. Only practice-area events are collected after Start. Export is not implemented.";

        titleFont = new Font(Font.FontFamily, 18F, FontStyle.Bold);
        emphasisFont = new Font(Font, FontStyle.Bold);

        var layout = new TableLayoutPanel
        {
            Name = "mainLayout",
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            ColumnCount = 1,
            RowCount = 6,
            TabStop = false
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (var row = 0; row < layout.RowCount; row++)
        {
            layout.RowStyles.Add(new RowStyle(row == 4 ? SizeType.Percent : SizeType.AutoSize, row == 4 ? 100 : 0));
        }

        var header = CreateStack("header");
        var title = CreateLabel("heading", "Key Event Logger");
        title.Font = titleFont;
        header.Controls.Add(title, 0, 0);
        header.Controls.Add(CreateLabel("purpose", "A focused workspace for understanding keyboard events."), 0, 1);
        layout.Controls.Add(header, 0, 0);

        recordingStatus = CreateLabel("recordingStatus", "Stopped — press Start to record practice-area keys.");
        recordingStatus.Font = emphasisFont;
        recordingStatus.BackColor = SystemColors.Control;
        recordingStatus.Padding = new Padding(12);
        recordingStatus.Margin = new Padding(0, 12, 0, 12);
        recordingStatus.AccessibleName = "Recording status";
        layout.Controls.Add(recordingStatus, 0, 1);

        var actions = new FlowLayoutPanel
        {
            Name = "recordingActions",
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = Padding.Empty,
            TabStop = false,
            WrapContents = true
        };
        startButton = CreateActionButton("startButton", "&Start", "Start recording");
        stopButton = CreateActionButton("stopButton", "S&top", "Stop recording");
        clearButton = CreateActionButton("clearButton", "&Clear", "Clear recorded events");
        exportButton = CreateActionButton("exportButton", "Export T&XT", "Export TXT");
        exportButton.AccessibleDescription = "Unavailable until Stage 03. No export or automatic saving.";
        actions.Controls.AddRange([startButton, stopButton, clearButton, exportButton]);
        layout.Controls.Add(actions, 0, 2);

        var availability = CreateLabel("featureAvailability",
            "Each delivered KeyDown / KeyUp is one event, including held-key repeats. Export TXT arrives in Stage 03.");
        availability.Margin = new Padding(0, 8, 0, 16);
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
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        workspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var practice = CreatePracticeArea(out practiceText);
        practice.Margin = new Padding(0, 0, 8, 0);
        practice.TabIndex = 0;
        workspace.Controls.Add(practice, 0, 0);
        var events = CreateEventArea(out eventList, out eventCount, out emptyState);
        events.Margin = new Padding(8, 0, 0, 0);
        events.TabIndex = 1;
        workspace.Controls.Add(events, 1, 0);
        layout.Controls.Add(workspace, 0, 4);

        var scope = CreateLabel("scopeNotice",
            "Recording is limited to Practice text while this window is active. Leaving the window stops it; returning requires Start.");
        scope.Margin = new Padding(0, 16, 0, 0);
        layout.Controls.Add(scope, 0, 5);

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
        }
        base.Dispose(disposing);
        if (disposing)
        {
            titleFont?.Dispose();
            emphasisFont?.Dispose();
        }
    }

    private static TableLayoutPanel CreateStack(string name) => new()
    {
        Name = name,
        Dock = DockStyle.Fill,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        ColumnCount = 1,
        Margin = Padding.Empty,
        TabStop = false
    };

    private static Label CreateLabel(string name, string text) => new()
    {
        Name = name,
        Text = text,
        AutoSize = true,
        Dock = DockStyle.Fill,
        Margin = new Padding(0, 0, 0, 4),
        UseMnemonic = false,
        TabStop = false
    };

    private static Button CreateActionButton(string name, string text, string accessibleName) => new()
    {
        Name = name,
        Text = text,
        AccessibleName = accessibleName,
        AccessibleDescription = "Controls recording only in this application's practice area.",
        AutoSize = true,
        MinimumSize = new Size(104, 36),
        Margin = new Padding(0, 0, 8, 0),
        Enabled = false,
        UseVisualStyleBackColor = true
    };

    private static GroupBox CreatePracticeArea(out TextBox practiceText)
    {
        var group = new GroupBox
        {
            Name = "practiceGroup",
            Text = "&Practice area",
            AccessibleName = "Practice area",
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };
        var content = CreateStack("practiceLayout");
        content.AutoSize = false;
        content.RowCount = 3;
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var label = CreateLabel("practiceLabel", "Pract&ice text");
        label.UseMnemonic = true;
        label.TabIndex = 0;
        content.Controls.Add(label, 0, 0);
        var hint = CreateLabel("practiceHint", "Press Start to record key events here. Nothing is saved.");
        hint.TabIndex = 2;
        hint.Margin = new Padding(0, 0, 0, 8);
        content.Controls.Add(hint, 0, 1);
        practiceText = new TextBox
        {
            Name = "practiceText",
            AccessibleName = "Practice text",
            AccessibleDescription = "Type here. Key events are recorded only after Start while this window is active.",
            Multiline = true,
            AcceptsReturn = true,
            AcceptsTab = false,
            ScrollBars = ScrollBars.Vertical,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            TabIndex = 1
        };
        content.Controls.Add(practiceText, 0, 2);
        group.Controls.Add(content);
        return group;
    }

    private static GroupBox CreateEventArea(out ListBox eventList, out Label eventCount, out Label emptyState)
    {
        var group = new GroupBox
        {
            Name = "eventsGroup",
            Text = "Recorded &events",
            AccessibleName = "Recorded events",
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };
        var content = CreateStack("eventsLayout");
        content.AutoSize = false;
        content.RowCount = 3;
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        eventCount = CreateLabel("eventCount", "0 events");
        eventCount.AccessibleName = "Recorded event count";
        content.Controls.Add(eventCount, 0, 0);
        emptyState = CreateLabel("emptyState", "No events yet. Press Start, then type in Practice text.");
        emptyState.Margin = new Padding(0, 0, 0, 8);
        content.Controls.Add(emptyState, 0, 1);
        eventList = new ListBox
        {
            Name = "eventList",
            AccessibleName = "Recorded events",
            AccessibleDescription = "Noneditable key event display showing timestamp, event type and key name.",
            Dock = DockStyle.Fill,
            IntegralHeight = false,
            HorizontalScrollbar = true,
            Margin = Padding.Empty,
            TabIndex = 0
        };
        content.Controls.Add(eventList, 0, 2);
        group.Controls.Add(content);
        return group;
    }
}
