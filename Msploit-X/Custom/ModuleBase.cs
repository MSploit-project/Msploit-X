using System.Reflection;
using Avalonia.Controls;
using ReactiveUI;

namespace Msploit_X.Custom
{
    [Obfuscation(Exclude = true)]
    public abstract class ModuleBase : ReactiveObject
    {
        public ModuleBase()
        {
            this.RaisePropertyChanged(nameof(Name));
            this.RaisePropertyChanged(nameof(Description));
            this.RaisePropertyChanged(nameof(Control));
        }
        public string Name => GetName();
        public string Description => GetDescription();
        public UserControl Control => GetControl();
        
        public abstract string GetName();
        public abstract string GetDescription();
        public abstract UserControl GetControl();
    }
}