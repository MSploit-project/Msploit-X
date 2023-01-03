using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Msploit_X.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private string edbLink = "https://gitlab.com/exploit-database/exploitdb/-/archive/main/exploitdb-main.zip";

        private void open_link_EDB(object? sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(edbLink) {UseShellExecute = true});
        }
        private void open_link_DP(object? sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://cirt.net/passwords") {UseShellExecute = true});
        }
    }
}