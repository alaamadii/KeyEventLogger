namespace KeyEventLogger.App.Forms;

// Use system colors in high contrast; state is always expressed in text too.
internal static class UiTheme
{
    private static bool HighContrast => SystemInformation.HighContrast;

    public static Color Canvas => HighContrast ? SystemColors.Control : Color.FromArgb(241, 245, 249);
    public static Color Surface => SystemColors.Window;
    public static Color Text => HighContrast ? SystemColors.WindowText : Color.FromArgb(25, 38, 60);
    public static Color Muted => HighContrast ? SystemColors.WindowText : Color.FromArgb(74, 90, 112);
    public static Color Accent => HighContrast ? SystemColors.Highlight : Color.FromArgb(30, 82, 185);
    public static Color OnAccent => SystemColors.HighlightText;
    public static Color Recording => HighContrast ? SystemColors.WindowText : Color.FromArgb(15, 100, 76);
    public static Color RecordingSurface => HighContrast ? SystemColors.Window : Color.FromArgb(225, 245, 236);
    public static Color AlternateRow => HighContrast ? SystemColors.Window : Color.FromArgb(246, 248, 252);
}
