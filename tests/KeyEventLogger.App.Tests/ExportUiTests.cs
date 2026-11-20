using System.Runtime.InteropServices;
using KeyEventLogger.App.Export;
using KeyEventLogger.App.Forms;
using KeyEventLogger.Core;
using Xunit;

namespace KeyEventLogger.App.Tests;

public sealed class ExportUiTests
{
    [Fact]
    public void EmptyExportCannotOpenADialog()
    {
        var picker = new Picker(_ => throw new InvalidOperationException("Empty export must not open."));
        var exporter = new Exporter((_, _) => throw new InvalidOperationException("Must not write."));
        WindowsUi.Run(form =>
        {
            Assert.False(Button(form, "exportButton").Enabled);
            Button(form, "exportButton").PerformClick();
            Assert.Equal(0, picker.Calls);
            Assert.Equal(0, exporter.Calls);
        }, picker: picker, exporter: exporter);
    }

    [Fact]
    public void CancellationStopsBeforeSelectionAndPreservesAllData()
    {
        var picker = new Picker(owner =>
        {
            var form = Assert.IsType<MainForm>(owner);
            Assert.Contains("Stopped", Status(form));
            foreach (var name in new[] { "startButton", "stopButton", "clearButton", "exportButton" })
            {
                Assert.False(Button(form, name).Enabled);
                Button(form, name).PerformClick();
            }
            return null;
        });
        var exporter = new Exporter((_, _) => throw new InvalidOperationException("Cancel must not write."));
        WindowsUi.Run(form =>
        {
            Seed(form);
            var rows = Rows(form);
            Button(form, "exportButton").PerformClick();
            Assert.Equal(rows, Rows(form));
            Assert.Equal("demo", Practice(form).Text);
            Assert.Equal(2, Events(form).Items.Count);
            Assert.Contains("canceled", Feedback(form).Text);
            Assert.True(Button(form, "startButton").Enabled);
            Assert.False(Button(form, "stopButton").Enabled);
            Assert.True(Button(form, "exportButton").Enabled);
            Assert.Equal(1, picker.Calls);
            Assert.Equal(0, exporter.Calls);
            Practice(form).Focus();
            SendKey(Practice(form), Keys.B, false);
            Assert.Equal(rows, Rows(form));
        }, picker: picker, exporter: exporter);
    }

    [Fact]
    public void SuccessfulExportUsesTheModelSnapshotAndLeavesRecordingStopped()
    {
        using var directory = new ExportTestDirectory();
        var picker = new Picker(_ => directory.FilePath());
        WindowsUi.Run(form =>
        {
            Seed(form);
            var before = Rows(form);
            Button(form, "exportButton").PerformClick();
            Assert.Equal(before, Rows(form));
            Assert.Equal("demo", Practice(form).Text);
            Assert.Contains("Exported 2 events", Feedback(form).Text);
            Assert.Contains(directory.FilePath(), Feedback(form).Text);
            Assert.Contains("Events: 2\r\n", File.ReadAllText(directory.FilePath()));
            Assert.Contains("\tKeyDown\tA\r\n", File.ReadAllText(directory.FilePath()));
            Assert.Contains("Stopped", Status(form));
            Assert.False(Button(form, "stopButton").Enabled);
            Practice(form).Focus();
            SendKey(Practice(form), Keys.B, false);
            Assert.Equal(2, Events(form).Items.Count);
            Button(form, "startButton").PerformClick();
            SendKey(Practice(form), Keys.B, false);
            Assert.Equal(3, Events(form).Items.Count);
        }, picker: picker);
    }

    [Fact]
    public void SnapshotIsReadOnlyAndExportDoesNotRebuildFromDisplayedStrings()
    {
        IReadOnlyList<KeyEventRecord>? captured = null;
        var picker = new Picker(_ => @"C:\synthetic.keyevents.txt");
        var exporter = new Exporter((path, records) =>
        {
            captured = records;
            Assert.Equal(2, records.Count);
            Assert.Equal("A", records[0].KeyName);
            Assert.Equal(KeyEventType.KeyDown, records[0].EventType);
            Assert.Equal(KeyEventType.KeyUp, records[1].EventType);
            var list = Assert.IsAssignableFrom<IList<KeyEventRecord>>(records);
            Assert.Throws<NotSupportedException>(() => list.Clear());
            return path;
        });
        WindowsUi.Run(form =>
        {
            Seed(form);
            Button(form, "exportButton").PerformClick();
            Button(form, "clearButton").PerformClick();
            Assert.Empty(Events(form).Items);
            Assert.Equal(2, captured!.Count);
            Assert.Equal("demo", Practice(form).Text);
        }, picker: picker, exporter: exporter);
    }

    [Fact]
    public void FailedExportKeepsDataAndCanBeRetried()
    {
        using var directory = new ExportTestDirectory();
        File.WriteAllText(directory.FilePath(), "Previous synthetic data");
        var picker = new Picker(_ => directory.FilePath());
        WindowsUi.Run(form =>
        {
            Seed(form);
            var before = Rows(form);
            File.SetAttributes(directory.FilePath(), FileAttributes.ReadOnly);
            Button(form, "exportButton").PerformClick();
            Assert.Contains("Export failed", Feedback(form).Text);
            Assert.Equal("Previous synthetic data", File.ReadAllText(directory.FilePath()));
            Assert.Equal(before, Rows(form));
            Assert.Equal("demo", Practice(form).Text);
            Assert.True(Button(form, "startButton").Enabled);
            Assert.False(Button(form, "stopButton").Enabled);
            File.SetAttributes(directory.FilePath(), FileAttributes.Normal);
            Button(form, "exportButton").PerformClick();
            Assert.Contains("Exported 2 events", Feedback(form).Text);
            Assert.Equal(before, Rows(form));
            Assert.Contains("Events: 2", File.ReadAllText(directory.FilePath()));
        }, picker: picker);
    }

    [Fact]
    public void DialogFailureIsReportedWithoutWritingOrResuming()
    {
        var picker = new Picker(_ => throw new IOException("Synthetic dialog failure"));
        var exporter = new Exporter((_, _) => throw new InvalidOperationException("Must not write."));
        WindowsUi.Run(form =>
        {
            Seed(form);
            Button(form, "exportButton").PerformClick();
            Assert.Contains("Export failed", Feedback(form).Text);
            Assert.DoesNotContain("Synthetic dialog failure", Feedback(form).Text);
            Assert.Contains("Records were kept", Feedback(form).Text);
            Assert.Equal(2, Events(form).Items.Count);
            Assert.Equal(0, exporter.Calls);
            Assert.False(Button(form, "stopButton").Enabled);
        }, picker: picker, exporter: exporter);
    }

    [Fact]
    public void ClosingDuringSelectionDoesNotWriteOrUpdateDisposedControls()
    {
        var picker = new Picker(owner =>
        {
            Assert.IsType<MainForm>(owner).Close();
            return @"C:\never-written.keyevents.txt";
        });
        var exporter = new Exporter((_, _) => throw new InvalidOperationException("Closed owner must not write."));
        WindowsUi.Run(form =>
        {
            Seed(form);
            Button(form, "exportButton").PerformClick();
            Assert.True(form.IsDisposed);
            Assert.Equal(0, exporter.Calls);
        }, picker: picker, exporter: exporter);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void LongSuccessOrErrorFeedbackDoesNotExpandTheLayout(bool error)
    {
        var longPath = @"C:\" + new string('x', 500) + ".txt";
        var picker = new Picker(_ => longPath);
        var exporter = new Exporter((path, _) => error ? throw new IOException("Synthetic failure: " + path) : path);
        WindowsUi.Run(form =>
        {
            form.Size = form.MinimumSize;
            Seed(form);
            Button(form, "exportButton").PerformClick();
            form.PerformLayout();
            Application.DoEvents();
            var label = Feedback(form);
            Assert.True(label.AutoEllipsis);
            if (error)
            {
                Assert.DoesNotContain(longPath, label.AccessibleDescription);
                Assert.DoesNotContain("Synthetic failure", label.Text);
            }
            else
            {
                Assert.Contains(longPath, label.AccessibleDescription);
            }
            foreach (var name in new[] { "exportFeedback", "recordingStatus", "practiceText", "eventList", "scopeNotice" })
            {
                var control = Assert.Single(form.Controls.Find(name, true));
                var bounds = new Rectangle(form.PointToClient(control.PointToScreen(Point.Empty)), control.ClientSize);
                Assert.True(form.ClientRectangle.Contains(bounds), name + " is clipped.");
                Assert.True(bounds.Height > 0);
            }
        }, picker: picker, exporter: exporter);
    }

    private static void Seed(MainForm form)
    {
        Practice(form).Text = "demo";
        WindowsUi.Activate(form);
        Assert.True(Practice(form).Focus());
        Assert.Equal(form.Handle, WindowsUi.ActiveWindow);
        Button(form, "startButton").PerformClick();
        Assert.True(Button(form, "stopButton").Enabled);
        SendKey(Practice(form), Keys.A, false);
        SendKey(Practice(form), Keys.A, true);
        Assert.Equal(2, Events(form).Items.Count);
        Assert.True(Button(form, "exportButton").Enabled);
    }

    private static Button Button(MainForm form, string name) => WindowsUi.Find<Button>(form, name);
    private static TextBox Practice(MainForm form) => WindowsUi.Find<TextBox>(form, "practiceText");
    private static ListBox Events(MainForm form) => Assert.IsAssignableFrom<ListBox>(Assert.Single(form.Controls.Find("eventList", true)));
    private static Label Feedback(MainForm form) => WindowsUi.Find<Label>(form, "exportFeedback");
    private static string Status(MainForm form) => WindowsUi.Find<Label>(form, "recordingStatus").Text;
    private static string[] Rows(MainForm form) => Events(form).Items.Cast<object>().Select(item => item.ToString()!).ToArray();

    private static void SendKey(Control control, Keys key, bool up) =>
        SendMessage(control.Handle, up ? 0x0101u : 0x0100u, (IntPtr)key, (IntPtr)(up ? -1073741823L : 1L));

    private sealed class Picker(Func<IWin32Window, string?> select) : IExportDestinationPicker
    {
        public int Calls { get; private set; }
        public string? SelectDestination(IWin32Window owner)
        {
            Calls++;
            return select(owner);
        }
    }

    private sealed class Exporter(Func<string, IReadOnlyList<KeyEventRecord>, string> write) : ITextLogExporter
    {
        public int Calls { get; private set; }
        public string Export(string destination, IReadOnlyList<KeyEventRecord> snapshot)
        {
            Calls++;
            return write(destination, snapshot);
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr handle, uint message, IntPtr wParam, IntPtr lParam);

    [Theory]
    [InlineData("access")]
    [InlineData("argument")]
    [InlineData("security")]
    [InlineData("dialog")]
    public void RecoverableErrorsNeverExposeRawExceptionDetails(string category)
    {
        const string privateDetail = @"private-demo-C:\owner-data\not-for-feedback";
        Exception error = category switch
        {
            "access" => new UnauthorizedAccessException(privateDetail),
            "argument" => new ArgumentException(privateDetail),
            "security" => new System.Security.SecurityException(privateDetail),
            _ => new ExternalException(privateDetail)
        };
        var picker = new Picker(_ => throw error);
        WindowsUi.Run(form =>
        {
            Seed(form);
            var before = Rows(form);
            Button(form, "exportButton").PerformClick();
            Assert.Contains("Export failed:", Feedback(form).Text);
            Assert.Contains("Records were kept", Feedback(form).Text);
            Assert.DoesNotContain(privateDetail, Feedback(form).Text);
            Assert.DoesNotContain(privateDetail, Feedback(form).AccessibleDescription);
            Assert.Equal(before, Rows(form));
            Assert.Equal("demo", Practice(form).Text);
            Assert.False(Button(form, "stopButton").Enabled);
            Assert.True(Button(form, "exportButton").Enabled);
        }, picker: picker);
    }

    [Fact]
    public void ExportAtCapacityKeepsTheLimitExplanationAndStartDisabled()
    {
        var picker = new Picker(_ => null);
        WindowsUi.Run(form =>
        {
            Seed(form);
            Assert.Contains("limit reached (2 events)", Status(form));
            var before = Rows(form);
            Button(form, "exportButton").PerformClick();
            Assert.Contains("canceled", Feedback(form).Text);
            Assert.Contains("limit reached (2 events)", Status(form));
            Assert.Equal(before, Rows(form));
            Assert.Equal("demo", Practice(form).Text);
            Assert.False(Button(form, "startButton").Enabled);
            Assert.False(Button(form, "stopButton").Enabled);
            Assert.True(Button(form, "exportButton").Enabled);
            Button(form, "clearButton").PerformClick();
            Assert.True(Button(form, "startButton").Enabled);
            Assert.DoesNotContain("limit reached", Status(form));
        }, picker: picker, capacity: 2);
    }
}
