using System.Globalization;
using KeyEventLogger.Core;

namespace KeyEventLogger.App.Forms;

// Retain the assignment's native ListBox while presenting aligned, read-only columns.
internal sealed class EventListBox : ListBox
{
    public const int TimestampWidth = 232;
    public const int EventWidth = 88;

    public EventListBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
        IntegralHeight = false;
        HorizontalScrollbar = true;
        BorderStyle = BorderStyle.None;
        BackColor = UiTheme.Surface;
        ForeColor = UiTheme.Text;
        UpdateMetrics();
    }

    public void AddRecord(KeyEventRecord record)
    {
        Items.Add(new DisplayRow(record));
        TopIndex = Items.Count - 1;
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        UpdateMetrics();
    }

    protected override void OnDpiChangedAfterParent(EventArgs e)
    {
        base.OnDpiChangedAfterParent(e);
        UpdateMetrics();
    }

    private void UpdateMetrics()
    {
        ItemHeight = Math.Max(1, Font.Height + LogicalToDeviceUnits(6));
        HorizontalExtent = LogicalToDeviceUnits(TimestampWidth + EventWidth + 120);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= Items.Count || Items[e.Index] is not DisplayRow row)
        {
            return;
        }

        var selected = (e.State & DrawItemState.Selected) != 0;
        var background = selected ? SystemColors.Highlight
            : e.Index % 2 == 0 ? UiTheme.Surface : UiTheme.AlternateRow;
        var foreground = selected ? SystemColors.HighlightText : UiTheme.Text;
        using var brush = new SolidBrush(background);
        e.Graphics.FillRectangle(brush, e.Bounds);

        var timeWidth = LogicalToDeviceUnits(TimestampWidth);
        var eventWidth = LogicalToDeviceUnits(EventWidth);
        DrawCell(row.Record.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", CultureInfo.InvariantCulture),
            e.Bounds.X, timeWidth);
        DrawCell(row.Record.EventType.ToString(), e.Bounds.X + timeWidth, eventWidth);
        DrawCell(row.Record.KeyName, e.Bounds.X + timeWidth + eventWidth, LogicalToDeviceUnits(120));
        e.DrawFocusRectangle();

        void DrawCell(string text, int x, int width) =>
            TextRenderer.DrawText(e.Graphics, text, Font,
                new Rectangle(x, e.Bounds.Y, width, e.Bounds.Height), foreground,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
    }

    private sealed record DisplayRow(KeyEventRecord Record)
    {
        // Native ListBox accessibility uses GetItemText/ToString for each row.
        public override string ToString() => string.Create(CultureInfo.InvariantCulture,
            $"{Record.Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}  |  {Record.EventType}  |  {Record.KeyName}");
    }
}
