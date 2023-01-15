using System;
using Avalonia.Controls;
using Msploit_X.Custom;

namespace ExampleModule
{
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
}