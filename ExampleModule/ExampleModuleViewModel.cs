using Avalonia;
using Msploit_X.ViewModels;
using Msploit_X.ViewModels.Objects;

namespace ExampleModule
{
    public class ExampleModuleViewModel : ViewModelBase
    {
        public ObservableType<int> Slider { get; } = new ObservableType<int>(0);
        public ObservableType<string> Label { get; } = new ObservableType<string>("Button");


        public void ButtonCommand()
        {
            Label.Value = "You clicked me!";
        }
    }
}