# MSploit-X

## Chapters
* [Description](#description)
* [Features](#features)
* [Api](#api)
    * [Example plugin base](#example-plugin-base)
    * [Example plugin axaml](#example-plugin-axaml)
    * [Example plugin axaml.cs](#example-plugin-axamlcs)
    * [Example plugin viewmodel](#example-plugin-viewmodel)

## [Description](#description)
### MSploit-X is a revived version of [MSploit](https://github.com/gitmylo/MSploit) with even more features. Made in avalonia with MVVM.

## [Features](#features)
* Nmap scanner:
    * The new nmap scanner gui is even better than the one in MSploit, as it also logs text output, allowing the user to run script scans as well. It allows the user to specify specific ports, and it doesn't crash the program anymore when a scan is cancelled or fails.
* Simple Web/Ping fuzzer
* Exploit-DB exploit browser.
* Exploit-DB search.
* An in-memory web server for hosting RCE payloads and more (or as an ip logger).
* Reverse shell handler with Netcat/Ncat.
* A (admittedly slow) web password cracker.
* Local file inclusion remote code execution payloads (using data://text/plain;base64,).
* Custom plugins!
    * Unlike MSploit, MSploit-X contains a system for custom plugins, with their own custom UI, as long as it's in avalonia xaml format. Read more on the [Api](#api)
* Links to tools.

## [Api](#api)

To set up the api, create a new dotnet 5 project (Class library), then include MSploit-X.exe and MSploit-X.dll, and Avalonia 11.0.0-Preview4 in the project.

### [Example plugin base](#example-plugin-base)
```cs
public class ExampleModule : ModuleBase
{
    public override string GetName()
    {
        return "Test Module";
    }

    public override string GetDescription()
    {
        return "Test Description";
    }

    public override UserControl GetControl()
    {
        return new ExampleControl();
    }
}
```

### [Example plugin axaml](#example-plugin-axaml)
```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:vm="clr-namespace:ExampleModule"
             mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
             x:Class="ExampleModule.ExampleControl">
	<UserControl.DataContext>
		<vm:ExampleModuleViewModel/>
	</UserControl.DataContext>
	<StackPanel>
		<Label HorizontalAlignment="Center" Content="{Binding Slider.Value, StringFormat='Slider is at {0}!'}"/>
		<Button Content="{Binding Label.Value}" Command="{Binding ButtonCommand}"/>
		<Slider Minimum="0" Maximum="100" IsSnapToTickEnabled="True" TickFrequency="1" Value="{Binding Slider.Value}"></Slider>
		<ComboBox></ComboBox>
		<TextBox></TextBox>
		<NumericUpDown></NumericUpDown>
	</StackPanel>
</UserControl>

```

### [Example plugin axaml.cs](#example-plugin-axamlcs) (It's default)
```cs
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
```

### [Example plugin viewmodel](#example-plugin-viewmodel)
```cs
public class ExampleModuleViewModel : ViewModelBase
{
    public ObservableType<int> Slider { get; } = new(0);
    public ObservableType<string> Label { get; } = new("Button");


    public void ButtonCommand()
    {
        Label.Value = "You clicked me!";
    }
}
```

Then when you're done creating the code and compiling it, simply take the compiled .dll and put it in the "Modules" folder.

done!