using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Msploit_X.Custom;
using Msploit_X.ViewModels;
using Msploit_X.ViewModels.Objects;
using ReactiveUI;

namespace Msploit_X.Models
{
    public class CustomModules : ReactiveObject
    {
        public ObservableCollection<ModuleBase> Items { get; } = new() { };
        public ObservableType<ModuleBase> SelectedItem { get; } = new();

        public CustomModules()
        {
            Directory.CreateDirectory("modules");
            foreach (var possibleMod in Directory.GetFiles("modules"))
            {
                if (possibleMod.EndsWith(".dll"))
                {
                    var dll = Assembly.LoadFile(Path.GetFullPath(possibleMod));
                    foreach (var type in dll.GetExportedTypes())
                    {
                        if (type.IsSubclassOf(typeof(ModuleBase)))
                        {
                            var gen = Activator.CreateInstance(type) as ModuleBase;
                            Items.Add(gen);
                        }
                    }
                }
            }
        }
    }
}