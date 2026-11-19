namespace KeyEventLogger.App.Forms;

public sealed class MainForm : Form
{
    public MainForm()
    {
        Text = "Key Event Logger";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(640, 360);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        AccessibleDescription = "Educational application scaffold. Recording is not implemented yet.";

        Controls.Add(new Label
        {
            Name = "initializationMessage",
            Text = "Recording is not implemented yet.",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(24),
            TabStop = false,
            AccessibleName = "Initialization status"
        });
    }
}
