using System.Globalization;
using System.Runtime.InteropServices;
using KeyEventLogger.App.Forms;
using Xunit;

namespace KeyEventLogger.App.Tests;

public sealed class MainFormTests
{
    [Fact]
    public void InitialStateIsStoppedEmptyAndExportUnavailable()
    {
        WindowsUi.Run(form =>
        {
            Assert.True(Button(form, "startButton").Enabled);
            Assert.False(Button(form, "stopButton").Enabled);
            Assert.False(Button(form, "clearButton").Enabled);
            Assert.False(Button(form, "exportButton").Enabled);
            Assert.Contains("Stopped", Status(form));
            Assert.Equal("0 events", Count(form));
            Assert.Empty(Events(form).Items);
            Assert.True(WindowsUi.Find<Label>(form, "emptyState").Visible);
            Assert.Contains("Export TXT stops", WindowsUi.Find<Label>(form, "featureAvailability").Text);
            Assert.Contains("active", WindowsUi.Find<Label>(form, "scopeNotice").Text);

            foreach (var name in new[] { "startButton", "stopButton", "clearButton", "exportButton" })
            {
                Assert.False(string.IsNullOrWhiteSpace(Button(form, name).AccessibleName));
            }
        });
    }

    [Fact]
    public void NativeEditingWhileStoppedDoesNotRecordAndStartDoesNotBackfill()
    {
        WindowsUi.Run(form =>
        {
            var practice = Practice(form);
            Assert.True(practice.Multiline);
            Assert.True(practice.AcceptsReturn);
            Assert.False(practice.AcceptsTab);
            Assert.False(practice.ReadOnly);
            Assert.True(practice.Focus());
            SendPair(practice, Keys.A);
            SendCharacter(practice, 'a');
            Assert.Equal("a", practice.Text);
            Assert.Empty(Events(form).Items);

            Start(form);
            Assert.Equal("a", practice.Text);
            Assert.Empty(Events(form).Items);
            Assert.Equal("0 events", Count(form));
            Assert.Contains("Recording", Status(form));
            Assert.False(Button(form, "startButton").Enabled);
            Assert.True(Button(form, "stopButton").Enabled);
        });
    }

    [Fact]
    public void NativeDownUpUsesKeyNamesAndTimestampOffsetWithoutChangingEditing()
    {
        var clock = new FixedClock();
        WindowsUi.Run(form =>
        {
            Start(form);
            var practice = Practice(form);
            SendKey(practice, Keys.A, false);
            Assert.Equal("1 event", Count(form));
            Assert.True(Button(form, "clearButton").Enabled);
            Assert.False(WindowsUi.Find<Label>(form, "emptyState").Visible);
            SendCharacter(practice, 'a');
            SendKey(practice, Keys.A, true);

            var expectedTime = clock.GetLocalNow().ToString("yyyy-MM-dd HH:mm:ss.fff zzz", CultureInfo.InvariantCulture);
            Assert.Equal(new[]
            {
                expectedTime + "  |  KeyDown  |  A",
                expectedTime + "  |  KeyUp  |  A"
            }, Rows(form));
            Assert.Equal("a", practice.Text);
            Assert.Equal("2 events", Count(form));
            Assert.True(Button(form, "exportButton").Enabled);
        }, clock);
    }

    [Fact]
    public void RepeatsAndModifiersAreSeparateDeliveredEvents()
    {
        WindowsUi.Run(form =>
        {
            Start(form);
            var practice = Practice(form);
            SendKey(practice, Keys.ControlKey, false);
            SendKey(practice, Keys.A, false);
            SendKey(practice, Keys.A, false, repeat: true);
            SendKey(practice, Keys.A, true);
            SendKey(practice, Keys.ControlKey, true);

            Assert.Equal(new[]
            {
                "KeyDown  |  ControlKey", "KeyDown  |  A", "KeyDown  |  A",
                "KeyUp  |  A", "KeyUp  |  ControlKey"
            }, Rows(form).Select(row => row[(row.IndexOf("  |  ", StringComparison.Ordinal) + 5)..]));
            Assert.Equal("5 events", Count(form));
        });
    }

    [Fact]
    public void StopRejectsFurtherEventsAndRestartKeepsEarlierRows()
    {
        WindowsUi.Run(form =>
        {
            Start(form);
            SendPair(Practice(form), Keys.A);
            var first = Rows(form);
            Button(form, "stopButton").PerformClick();
            Assert.Contains("Stopped", Status(form));
            Assert.True(Button(form, "startButton").Enabled);
            Assert.False(Button(form, "stopButton").Enabled);
            Practice(form).Focus();
            SendPair(Practice(form), Keys.B);
            Assert.Equal(first, Rows(form));

            Start(form);
            SendPair(Practice(form), Keys.C);
            Assert.Equal(4, Events(form).Items.Count);
            Assert.Equal(first, Rows(form).Take(2));
            Assert.EndsWith("KeyUp  |  C", Rows(form)[3]);
        });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ClearPreservesStateAndPracticeText(bool recording)
    {
        WindowsUi.Run(form =>
        {
            Practice(form).Text = "Synthetic practice";
            Start(form);
            SendPair(Practice(form), Keys.A);
            if (!recording)
            {
                Button(form, "stopButton").PerformClick();
            }
            Button(form, "clearButton").PerformClick();

            Assert.Empty(Events(form).Items);
            Assert.Equal("0 events", Count(form));
            Assert.Equal("Synthetic practice", Practice(form).Text);
            Assert.True(WindowsUi.Find<Label>(form, "emptyState").Visible);
            Assert.False(Button(form, "clearButton").Enabled);
            Assert.Equal(recording, Button(form, "stopButton").Enabled);
            Assert.Equal(!recording, Button(form, "startButton").Enabled);
            Practice(form).Focus();
            SendKey(Practice(form), Keys.B, true);
            Assert.Equal(recording ? 1 : 0, Events(form).Items.Count);
        });
    }

    [Fact]
    public void FocusWithinFormDoesNotStopButOnlyPracticeEventsAreAccepted()
    {
        WindowsUi.Run(form =>
        {
            Start(form);
            Assert.True(Events(form).Focus());
            SendPair(Events(form), Keys.B);
            SendPair(Practice(form), Keys.C);

            Assert.Empty(Events(form).Items);
            Assert.True(Button(form, "stopButton").Enabled);
            Assert.Contains("Recording", Status(form));
            Practice(form).Focus();
            SendPair(Practice(form), Keys.D);
            Assert.Equal(2, Events(form).Items.Count);
        });
    }

    [Fact]
    public void DeactivationStopsAndInactiveStartOrInputIsRejected()
    {
        WindowsUi.Run(form =>
        {
            Start(form);
            SendPair(Practice(form), Keys.A);
            var before = Rows(form);
            using var other = new Form
            {
                ShowInTaskbar = false,
                Opacity = 0,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(-10000, -10000)
            };
            other.Show();
            WindowsUi.Activate(other);
            Application.DoEvents();
            Assert.Equal(other.Handle, WindowsUi.ActiveWindow);
            Assert.Contains("Stopped", Status(form));
            Assert.False(Button(form, "stopButton").Enabled);
            Button(form, "startButton").PerformClick();
            SendPair(Practice(form), Keys.B);
            Assert.Equal(before, Rows(form));
            Assert.False(Button(form, "stopButton").Enabled);

            other.Close();
            WindowsUi.Activate(form);
            Practice(form).Focus();
            Application.DoEvents();
            SendPair(Practice(form), Keys.C);
            Assert.Equal(before, Rows(form));
            Start(form);
            SendPair(Practice(form), Keys.D);
            Assert.Equal(4, Events(form).Items.Count);
        });
    }

    [Fact]
    public void MinimizeRestoreRequiresExplicitStart()
    {
        WindowsUi.Run(form =>
        {
            Start(form);
            SendPair(Practice(form), Keys.A);
            form.WindowState = FormWindowState.Minimized;
            Application.DoEvents();
            SendPair(Practice(form), Keys.B);
            Assert.Contains("Stopped", Status(form));
            Assert.False(Button(form, "stopButton").Enabled);
            Assert.Equal(2, Events(form).Items.Count);

            form.WindowState = FormWindowState.Normal;
            WindowsUi.Activate(form);
            Practice(form).Focus();
            Application.DoEvents();
            SendPair(Practice(form), Keys.C);
            Assert.Equal(2, Events(form).Items.Count);
            Assert.True(Button(form, "startButton").Enabled);
            Start(form);
            SendPair(Practice(form), Keys.D);
            Assert.Equal(4, Events(form).Items.Count);
        });
    }

    [Fact]
    public void KeyboardButtonActivationDoesNotBecomeAPracticeEvent()
    {
        WindowsUi.Run(form =>
        {
            var start = Button(form, "startButton");
            Assert.True(start.Focus());
            SendKey(start, Keys.Space, false);
            SendKey(start, Keys.Space, true);
            Application.DoEvents();
            Assert.Contains("Recording", Status(form));
            Assert.True(Practice(form).Focused);
            Assert.Empty(Events(form).Items);

            var stop = Button(form, "stopButton");
            Assert.True(stop.Focus());
            SendKey(stop, Keys.Space, false);
            SendKey(stop, Keys.Space, true);
            Application.DoEvents();
            Assert.Contains("Stopped", Status(form));
            Assert.Empty(Events(form).Items);
        });
    }

    [Fact]
    public void NativeTabRemainsNavigationAndEmptyExportStaysDisabled()
    {
        WindowsUi.Run(form =>
        {
            Start(form);
            Assert.True(PostMessage(Practice(form).Handle, 0x0100, (IntPtr)Keys.Tab, (IntPtr)1));
            Assert.True(PostMessage(Practice(form).Handle, 0x0101, (IntPtr)Keys.Tab, (IntPtr)(-1073741823L)));
            Application.DoEvents();
            Assert.True(Events(form).Focused);
            Assert.Empty(Events(form).Items);
            Assert.True(Button(form, "stopButton").Enabled);
            Assert.False(Button(form, "exportButton").Enabled);
            Assert.True(form.SelectNextControl(Events(form), false, true, true, true));
            Assert.True(Practice(form).Focused);
        });
    }

    [Fact]
    public void RepeatedRecordingCyclesKeepDisplayAndCountConsistent()
    {
        WindowsUi.Run(form =>
        {
            for (var cycle = 0; cycle < 5; cycle++)
            {
                Start(form);
                SendPair(Practice(form), Keys.A);
                Button(form, "stopButton").PerformClick();
                Assert.Equal(2 * (cycle + 1), Events(form).Items.Count);
                Assert.Equal($"{2 * (cycle + 1)} events", Count(form));
            }
            Button(form, "clearButton").PerformClick();
            Assert.Empty(Events(form).Items);
            Assert.Equal("0 events", Count(form));
        });
    }

    [Fact]
    public void CanceledCloseStopsRecordingAndAllowsExplicitStartAgain()
    {
        WindowsUi.Run(form =>
        {
            form.FormClosing += (_, e) => e.Cancel = true;
            Start(form);
            SendPair(Practice(form), Keys.A);
            form.Close();
            Assert.False(form.IsDisposed);
            Assert.Contains("Stopped", Status(form));
            Assert.True(Button(form, "startButton").Enabled);
            Start(form);
            Assert.Contains("Recording", Status(form));
            Assert.Equal(2, Events(form).Items.Count);
        });
    }

    [Fact]
    public void FreshWindowDoesNotRestoreTextEventsOrRecordingState()
    {
        WindowsUi.Run(form =>
        {
            Practice(form).Text = "Synthetic practice";
            Start(form);
            SendPair(Practice(form), Keys.A);
        });
        WindowsUi.Run(form =>
        {
            Assert.Empty(Practice(form).Text);
            Assert.Empty(Events(form).Items);
            Assert.Contains("Stopped", Status(form));
        });
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(240, 0)]
    [InlineData(0, 160)]
    [InlineData(320, 200)]
    public void MinimumAndResizedLayoutsKeepStatesReadable(int extraWidth, int extraHeight)
    {
        WindowsUi.Run(form =>
        {
            form.Size = new Size(form.MinimumSize.Width + extraWidth, form.MinimumSize.Height + extraHeight);
            AssertLayout(form);
            Start(form);
            SendPair(Practice(form), Keys.A);
            AssertLayout(form);
            Button(form, "stopButton").PerformClick();
            AssertLayout(form);
            Button(form, "clearButton").PerformClick();
            AssertLayout(form);
        });
    }

    private static void AssertLayout(MainForm form)
    {
        form.PerformLayout();
        Application.DoEvents();
        foreach (var control in Descendants(form).Where(control => control.Visible))
        {
            var bounds = new Rectangle(form.PointToClient(control.PointToScreen(Point.Empty)), control.ClientSize);
            Assert.True(bounds.Width > 0 && bounds.Height > 0, $"{control.Name} has no usable area.");
            Assert.True(form.ClientRectangle.Contains(bounds), $"{control.Name} is outside the client area.");
            if (control is Label label)
            {
                Assert.True(label.Height >= label.GetPreferredSize(new Size(label.Width, 0)).Height,
                    $"{label.Name} clips its text.");
            }
        }
        Assert.True(Practice(form).ClientSize.Height >= 120);
        Assert.True(Events(form).ClientSize.Height >= 120);
        Assert.False(WindowsUi.Find<Panel>(form, "practiceGroup").Bounds
            .IntersectsWith(WindowsUi.Find<Panel>(form, "eventsGroup").Bounds));
    }

    private static void Start(MainForm form)
    {
        WindowsUi.Activate(form);
        Application.DoEvents();
        Assert.Equal(form.Handle, WindowsUi.ActiveWindow);
        Assert.True(Button(form, "startButton").Enabled);
        Button(form, "startButton").PerformClick();
        Assert.True(Practice(form).Focused);
        Assert.True(Button(form, "stopButton").Enabled);
    }

    private static TextBox Practice(MainForm form) => WindowsUi.Find<TextBox>(form, "practiceText");
    private static ListBox Events(MainForm form) => Assert.IsAssignableFrom<ListBox>(Assert.Single(form.Controls.Find("eventList", true)));
    private static Button Button(MainForm form, string name) => WindowsUi.Find<Button>(form, name);
    private static string Status(MainForm form) => WindowsUi.Find<Label>(form, "recordingStatus").Text;
    private static string Count(MainForm form) => WindowsUi.Find<Label>(form, "eventCount").Text;
    private static string[] Rows(MainForm form) => Events(form).Items.Cast<object>().Select(item => item.ToString()!).ToArray();

    private static void SendPair(Control control, Keys key)
    {
        SendKey(control, key, false);
        SendKey(control, key, true);
    }

    private static void SendKey(Control control, Keys key, bool up, bool repeat = false) =>
        SendMessage(control.Handle, up ? 0x0101u : 0x0100u, (IntPtr)key,
            (IntPtr)(up ? -1073741823L : repeat ? 0x40000001L : 1L));

    private static void SendCharacter(Control control, char character) =>
        SendMessage(control.Handle, 0x0102, (IntPtr)character, IntPtr.Zero);

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

    private sealed class FixedClock : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(2026, 11, 19, 9, 34, 56, 789, TimeSpan.Zero);
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.CreateCustomTimeZone("Test +03", TimeSpan.FromHours(3), "Test +03", "Test +03");
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr handle, uint message, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool PostMessage(IntPtr handle, uint message, IntPtr wParam, IntPtr lParam);

    [Theory]
    [InlineData(Keys.Space, ' ', "x ")]
    [InlineData(Keys.Back, '\b', "")]
    public void SpaceAndBackspaceEditOnceAndDisplayDownAndUpSeparately(Keys key, char character, string expectedText)
    {
        WindowsUi.Run(form =>
        {
            Practice(form).Text = "x";
            Start(form);
            Practice(form).SelectionStart = Practice(form).TextLength;
            SendKey(Practice(form), key, false);
            SendCharacter(Practice(form), character);
            SendKey(Practice(form), key, true);

            Assert.Equal(expectedText, Practice(form).Text);
            Assert.Equal("2 events", Count(form));
            Assert.EndsWith("KeyDown  |  " + key, Rows(form)[0]);
            Assert.EndsWith("KeyUp  |  " + key, Rows(form)[1]);
            Assert.Equal(DrawMode.OwnerDrawFixed, Events(form).DrawMode);
            Assert.Contains("One press", WindowsUi.Find<Label>(form, "featureAvailability").Text);
        });
    }

    [Fact]
    public void FullSessionStopsWithoutLosingRowsAndClearAllowsExplicitRestart()
    {
        WindowsUi.Run(form =>
        {
            Practice(form).Text = "sample";
            Start(form);
            SendPair(Practice(form), Keys.A);
            SendPair(Practice(form), Keys.B);
            Assert.Equal(3, Events(form).Items.Count);
            Assert.Equal("3 events", Count(form));
            Assert.Contains("limit reached (3 events)", Status(form));
            Assert.False(Button(form, "startButton").Enabled);
            Assert.False(Button(form, "stopButton").Enabled);
            Assert.True(Button(form, "exportButton").Enabled);
            Button(form, "startButton").PerformClick();
            SendPair(Practice(form), Keys.C);
            Assert.Equal(3, Events(form).Items.Count);

            Button(form, "clearButton").PerformClick();
            Assert.Empty(Events(form).Items);
            Assert.Equal("sample", Practice(form).Text);
            Assert.True(Button(form, "startButton").Enabled);
            Assert.False(Button(form, "stopButton").Enabled);
            Assert.DoesNotContain("limit reached", Status(form));
            Start(form);
            SendPair(Practice(form), Keys.D);
            Assert.Equal(2, Events(form).Items.Count);
        }, capacity: 3);
    }
}
