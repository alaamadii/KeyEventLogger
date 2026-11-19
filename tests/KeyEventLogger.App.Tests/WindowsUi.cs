using System.Runtime.ExceptionServices;
using KeyEventLogger.App.Forms;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace KeyEventLogger.App.Tests;

internal static class WindowsUi
{
    static WindowsUi()
    {
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
    }

    public static void Run(Action<MainForm> assertion)
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                using var form = new MainForm
                {
                    ShowInTaskbar = false,
                    Opacity = 0,
                    StartPosition = FormStartPosition.Manual,
                    Location = new Point(-10000, -10000)
                };
                form.Show();
                Application.DoEvents();
                assertion(form);
                form.Close();
            }
            catch (Exception exception)
            {
                failure = exception;
            }
        })
        {
            IsBackground = true
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        if (!thread.Join(TimeSpan.FromSeconds(20)))
        {
            throw new TimeoutException("Native UI test did not finish.");
        }

        if (failure is not null)
        {
            ExceptionDispatchInfo.Capture(failure).Throw();
        }
    }

    public static T Find<T>(Control root, string name) where T : Control =>
        Assert.IsType<T>(Assert.Single(root.Controls.Find(name, true)));
}
