using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ExampleModule
{
    public partial class ExampleControl : UserControl
    {
        public ExampleControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}