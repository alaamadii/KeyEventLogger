using System.Runtime.InteropServices;
using KeyEventLogger.App.Forms;
using Xunit;

namespace KeyEventLogger.App.Tests;

public sealed class MainFormTests
{
    [Fact]
    public void PreviewClearlyDisablesAllFutureActions()
    {
        WindowsUi.Run(form =>
        {
            foreach (var name in new[] { "startButton", "stopButton", "clearButton", "exportButton" })
            {
                var button = WindowsUi.Find<Button>(form, name);
                Assert.False(button.Enabled);
                Assert.True(button.Visible);
                Assert.False(string.IsNullOrWhiteSpace(button.AccessibleName));
            }

            Assert.Contains("Not recording", WindowsUi.Find<Label>(form, "recordingStatus").Text);
            Assert.Contains("later stage", WindowsUi.Find<Label>(form, "recordingStatus").Text);
            var availability = WindowsUi.Find<Label>(form, "featureAvailability");
            Assert.True(availability.Visible);
            Assert.Contains("Stage 02", availability.Text);
            Assert.Contains("Stage 03", availability.Text);
            Assert.Contains("active window", WindowsUi.Find<Label>(form, "scopeNotice").Text);
        });
    }

    [Fact]
    public void PracticeEditingDoesNotPopulateOrActivateTheEventDisplay()
    {
        WindowsUi.Run(form =>
        {
            var practice = WindowsUi.Find<TextBox>(form, "practiceText");
            var events = WindowsUi.Find<ListBox>(form, "eventList");
            Assert.True(practice.Multiline);
            Assert.True(practice.AcceptsReturn);
            Assert.False(practice.AcceptsTab);
            Assert.False(practice.ReadOnly);
            Assert.Empty(practice.Text);
            Assert.Empty(events.Items);
            Assert.True(practice.Focus());

            foreach (var character in "UI sample")
            {
                SendMessage(practice.Handle, 0x0102, (IntPtr)character, IntPtr.Zero);
            }
            Application.DoEvents();

            Assert.Equal("UI sample", practice.Text);
            Assert.Empty(events.Items);
            Assert.Equal("0 events", WindowsUi.Find<Label>(form, "eventCount").Text);
            Assert.Contains("Not recording", WindowsUi.Find<Label>(form, "recordingStatus").Text);
            Assert.False(WindowsUi.Find<Button>(form, "startButton").Enabled);
        });
    }

    [Fact]
    public void EventDisplayCannotBeEditedWithCharacterMessages()
    {
        WindowsUi.Run(form =>
        {
            var events = WindowsUi.Find<ListBox>(form, "eventList");
            SendMessage(events.Handle, 0x0102, (IntPtr)'X', IntPtr.Zero);
            Application.DoEvents();
            Assert.Empty(events.Items);
            Assert.Equal("0 events", WindowsUi.Find<Label>(form, "eventCount").Text);
            Assert.Contains("No events", WindowsUi.Find<Label>(form, "emptyState").Text);
        });
    }

    [Fact]
    public void TabNavigationMovesBetweenTheTwoUsableContentControls()
    {
        WindowsUi.Run(form =>
        {
            var practice = WindowsUi.Find<TextBox>(form, "practiceText");
            var events = WindowsUi.Find<ListBox>(form, "eventList");
            Assert.True(practice.Focus());
            Assert.True(form.SelectNextControl(practice, true, true, true, true));
            Assert.True(events.Focused);
            Assert.True(form.SelectNextControl(events, false, true, true, true));
            Assert.True(practice.Focused);
            Assert.False(string.IsNullOrWhiteSpace(practice.AccessibleName));
            Assert.False(string.IsNullOrWhiteSpace(events.AccessibleName));
        });
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(240, 0)]
    [InlineData(0, 160)]
    [InlineData(320, 200)]
    public void MinimumAndResizedLayoutsKeepEssentialContentVisible(int extraWidth, int extraHeight)
    {
        WindowsUi.Run(form =>
        {
            form.Size = new Size(form.MinimumSize.Width + extraWidth, form.MinimumSize.Height + extraHeight);
            form.PerformLayout();
            Application.DoEvents();

            foreach (var control in Descendants(form).Where(control => control.Visible))
            {
                var bounds = new Rectangle(form.PointToClient(control.PointToScreen(Point.Empty)), control.ClientSize);
                Assert.True(bounds.Width > 0 && bounds.Height > 0, $"{control.Name} has no usable area.");
                Assert.True(form.ClientRectangle.Contains(bounds), $"{control.Name} is outside the client area: {bounds}.");
                if (control is Label label)
                {
                    var requiredHeight = label.GetPreferredSize(new Size(label.Width, 0)).Height;
                    Assert.True(label.Height >= requiredHeight, $"{label.Name} clips its text.");
                }
            }

            Assert.True(WindowsUi.Find<TextBox>(form, "practiceText").ClientSize.Height >= 120);
            Assert.True(WindowsUi.Find<ListBox>(form, "eventList").ClientSize.Height >= 120);
            var practice = WindowsUi.Find<GroupBox>(form, "practiceGroup");
            var events = WindowsUi.Find<GroupBox>(form, "eventsGroup");
            Assert.False(practice.Bounds.IntersectsWith(events.Bounds));
        });
    }

    [Fact]
    public void FreshWindowDoesNotRestorePracticeText()
    {
        WindowsUi.Run(form => WindowsUi.Find<TextBox>(form, "practiceText").Text = "Synthetic preview text");
        WindowsUi.Run(form =>
        {
            Assert.Empty(WindowsUi.Find<TextBox>(form, "practiceText").Text);
            Assert.Empty(WindowsUi.Find<ListBox>(form, "eventList").Items);
        });
    }

    private static IEnumerable<Control> Descendants(Control root)
    {
        foreach (Control child in root.Controls)
        {
            yield return child;
            foreach (var descendant in Descendants(child))
            {
                yield return descendant;
            }
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr handle, uint message, IntPtr wParam, IntPtr lParam);
}
