namespace KeyEventLogger.App.Forms;

public sealed class MainForm : Form
{
    private readonly Font titleFont;
    private readonly Font emphasisFont;

    public MainForm()
    {
        SuspendLayout();
        Text = "Key Event Logger";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(960, 720);
        MinimumSize = new Size(820, 680);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = SystemColors.Window;
        ForeColor = SystemColors.WindowText;
        AccessibleDescription = "Educational keyboard-event interface. Recording and export are unavailable in this preview.";

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

        var status = CreateLabel("recordingStatus",
            "Not recording — recording will be available in a later stage.");
        status.Font = emphasisFont;
        status.BackColor = SystemColors.Control;
        status.Padding = new Padding(12);
        status.Margin = new Padding(0, 12, 0, 12);
        status.AccessibleName = "Recording status";
        layout.Controls.Add(status, 0, 1);

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
        actions.Controls.Add(CreateUnavailableButton("startButton", "&Start", "Start recording"));
        actions.Controls.Add(CreateUnavailableButton("stopButton", "S&top", "Stop recording"));
        actions.Controls.Add(CreateUnavailableButton("clearButton", "&Clear", "Clear recorded events"));
        actions.Controls.Add(CreateUnavailableButton("exportButton", "Export T&XT", "Export TXT"));
        layout.Controls.Add(actions, 0, 2);

        var availability = CreateLabel("featureAvailability",
            "Start, Stop and Clear arrive in Stage 02. Export TXT arrives in Stage 03.");
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
        var practice = CreatePracticeArea(out var practiceText);
        practice.Margin = new Padding(0, 0, 8, 0);
        practice.TabIndex = 0;
        workspace.Controls.Add(practice, 0, 0);
        var events = CreateEventArea();
        events.Margin = new Padding(8, 0, 0, 0);
        events.TabIndex = 1;
        workspace.Controls.Add(events, 1, 0);
        layout.Controls.Add(workspace, 0, 4);

        var scope = CreateLabel("scopeNotice",
            "Future recording will observe only this application's active window. Losing focus will stop it; returning will require Start.");
        scope.Margin = new Padding(0, 16, 0, 0);
        layout.Controls.Add(scope, 0, 5);

        Controls.Add(layout);
        ActiveControl = practiceText;
        ResumeLayout(false);
        PerformLayout();
    }

    protected override void Dispose(bool disposing)
    {
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

    private static Button CreateUnavailableButton(string name, string text, string accessibleName) => new()
    {
        Name = name,
        Text = text,
        AccessibleName = accessibleName,
        AccessibleDescription = "Unavailable in this preview. See the stage availability note.",
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

        var label = CreateLabel("practiceLabel", "Practice &text");
        label.UseMnemonic = true;
        label.TabIndex = 0;
        content.Controls.Add(label, 0, 0);
        var hint = CreateLabel("practiceHint", "Type here to try the interface. Nothing is recorded or saved.");
        hint.TabIndex = 2;
        hint.Margin = new Padding(0, 0, 0, 8);
        content.Controls.Add(hint, 0, 1);
        practiceText = new TextBox
        {
            Name = "practiceText",
            AccessibleName = "Practice text",
            AccessibleDescription = "Temporary local text only. Recording is not implemented.",
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

    private static GroupBox CreateEventArea()
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
        var count = CreateLabel("eventCount", "0 events");
        count.AccessibleName = "Recorded event count";
        content.Controls.Add(count, 0, 0);
        var empty = CreateLabel("emptyState", "No events yet. Recording is not implemented.");
        empty.Margin = new Padding(0, 0, 0, 8);
        content.Controls.Add(empty, 0, 1);
        content.Controls.Add(new ListBox
        {
            Name = "eventList",
            AccessibleName = "Recorded events",
            AccessibleDescription = "Empty, noneditable event display. Recording is not implemented.",
            Dock = DockStyle.Fill,
            IntegralHeight = false,
            HorizontalScrollbar = true,
            Margin = Padding.Empty,
            TabIndex = 0
        }, 0, 2);
        group.Controls.Add(content);
        return group;
    }
}
