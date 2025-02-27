using System.Windows.Controls;

namespace Flow.Launcher.Plugin.WebDavQuickUploader.Views
{
    public partial class SettingsControl : UserControl
    {
        public Settings Settings { get; }

        public SettingsControl(Settings settings)
        {
            Settings = settings;
            InitializeComponent();
        }
    }
}
