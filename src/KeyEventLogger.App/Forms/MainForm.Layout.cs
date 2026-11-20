namespace KeyEventLogger.App.Forms;

public sealed partial class MainForm
{
    private TableLayoutPanel CreateMainLayout()
    {
        var layout = new TableLayoutPanel
        {
            Name = "mainLayout",
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            ColumnCount = 1,
            RowCount = 7,
            TabStop = false
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (var row = 0; row < layout.RowCount; row++)
        {
            layout.RowStyles.Add(new RowStyle(row == 4 ? SizeType.Percent : SizeType.AutoSize, row == 4 ? 100 : 0));
        }
        return layout;
    }

    private TableLayoutPanel CreateHeader()
    {
        var header = CreateStack("header");
        var title = CreateLabel("heading", "Key Event Logger");
        title.Font = titleFont;
        header.Controls.Add(title, 0, 0);
        var purpose = CreateLabel("purpose", "Understand keyboard events in a private, local workspace.");
        purpose.ForeColor = UiTheme.Muted;
        header.Controls.Add(purpose, 0, 1);
        return header;
    }

    private FlowLayoutPanel CreateActions(out Button start, out Button stop, out Button clear, out Button export)
    {
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
        start = CreateActionButton("startButton", "&Start recording", "Start recording");
        start.BackColor = UiTheme.Accent;
        start.ForeColor = UiTheme.OnAccent;
        stop = CreateActionButton("stopButton", "S&top", "Stop recording");
        clear = CreateActionButton("clearButton", "&Clear events", "Clear recorded events");
        export = CreateActionButton("exportButton", "Export T&XT", "Export TXT");
        export.AccessibleDescription = "Stop recording and export event names/timestamps to a local TXT file. Practice text is not exported.";
        actions.Controls.AddRange([start, stop, clear, export]);
        return actions;
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
        Margin = new Padding(0, 0, 0, 6),
        UseMnemonic = false,
        TabStop = false
    };

    private Button CreateActionButton(string name, string text, string accessibleName) => new()
    {
        Name = name,
        Text = text,
        AccessibleName = accessibleName,
        AccessibleDescription = "Controls recording only in this application's practice area.",
        AutoSize = true,
        MinimumSize = new Size(112, 38),
        Padding = new Padding(12, 4, 12, 4),
        Margin = new Padding(0, 0, 10, 0),
        Font = emphasisFont,
        BackColor = UiTheme.Surface,
        ForeColor = UiTheme.Text,
        FlatStyle = FlatStyle.Flat,
        Enabled = false,
        UseVisualStyleBackColor = false
    };

    private static Panel CreateCard(string name, string accessibleName) => new()
    {
        Name = name,
        AccessibleName = accessibleName,
        AccessibleRole = AccessibleRole.Grouping,
        Dock = DockStyle.Fill,
        BackColor = UiTheme.Surface,
        Padding = new Padding(16)
    };

    private Panel CreatePracticeArea(out TextBox practice)
    {
        var card = CreateCard("practiceGroup", "Practice area");
        var content = CreateStack("practiceLayout");
        content.AutoSize = false;
        content.RowCount = 3;
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var label = CreateLabel("practiceLabel", "Pract&ice text");
        label.UseMnemonic = true;
        label.Font = emphasisFont;
        label.TabIndex = 0;
        content.Controls.Add(label, 0, 0);

        var hint = CreateLabel("practiceHint", "Use sample text. Clear events keeps this text; only event records are exported.");
        hint.ForeColor = UiTheme.Muted;
        hint.TabIndex = 2;
        hint.Margin = new Padding(0, 0, 0, 12);
        content.Controls.Add(hint, 0, 1);

        practice = new TextBox
        {
            Name = "practiceText",
            AccessibleName = "Practice text",
            AccessibleDescription = "Type sample text here. Start is required; only this focused area is recorded. Do not enter credentials.",
            PlaceholderText = "Type a short sample after pressing Start…",
            Multiline = true,
            AcceptsReturn = true,
            AcceptsTab = false,
            MaxLength = 4096,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = UiTheme.Surface,
            ForeColor = UiTheme.Text,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            TabIndex = 1
        };
        content.Controls.Add(practice, 0, 2);
        card.Controls.Add(content);
        return card;
    }

    private Panel CreateEventArea(out EventListBox events, out Label count, out Label empty)
    {
        var card = CreateCard("eventsGroup", "Recorded events");
        var content = CreateStack("eventsLayout");
        content.AutoSize = false;
        content.RowCount = 4;
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        count = CreateLabel("eventCount", "0 events");
        count.AccessibleName = "Recorded event count";
        count.Font = emphasisFont;
        content.Controls.Add(count, 0, 0);

        empty = CreateLabel("emptyState", "Your session is empty. Start recording, then type in Practice text.");
        empty.ForeColor = UiTheme.Muted;
        empty.Margin = new Padding(0, 0, 0, 12);
        content.Controls.Add(empty, 0, 1);

        var columns = new TableLayoutPanel
        {
            Name = "eventColumns",
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 3,
            RowCount = 1,
            Margin = Padding.Empty,
            TabStop = false
        };
        columns.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, EventListBox.TimestampWidth));
        columns.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, EventListBox.EventWidth));
        columns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        columns.Controls.Add(CreateLabel("timestampColumn", "Timestamp (local / UTC offset)"), 0, 0);
        columns.Controls.Add(CreateLabel("typeColumn", "Event"), 1, 0);
        columns.Controls.Add(CreateLabel("keyColumn", "Key"), 2, 0);
        content.Controls.Add(columns, 0, 2);

        events = new EventListBox
        {
            Name = "eventList",
            AccessibleName = "Recorded events",
            AccessibleDescription = "Read-only columns: local timestamp and UTC offset, KeyDown (press), and virtual-key name. A short press creates one row; release is not recorded.",
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            TabIndex = 0
        };
        content.Controls.Add(events, 0, 3);
        card.Controls.Add(content);
        return card;
    }
}
