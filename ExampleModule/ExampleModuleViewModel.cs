using Avalonia;
using Msploit_X.ViewModels;
using Msploit_X.ViewModels.Objects;

namespace ExampleModule
{
    public class ExampleModuleViewModel : ViewModelBase
    {
        public ObservableType<int> Slider { get; } = new(0);
        public ObservableType<string> Label { get; } = new("Button");


        public void ButtonCommand()
        {
            Label.Value = "You clicked me!";
        }
    }
}