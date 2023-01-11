using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Msploit_X.ViewModels.Objects;
using ReactiveUI;

namespace Msploit_X.Models.nmap
{
    public class Result : ReactiveObject
    {
        private string scanName = "";

        private string outputText = "";

        public string ScanName
        {
            get => scanName;
            set => this.RaiseAndSetIfChanged(ref scanName, value);
        }

        public string OutputText
        {
            get => outputText;
            set => this.RaiseAndSetIfChanged(ref outputText, value);
        }
        
        private ObservableCollection<Host> hosts = new ObservableCollection<Host>();

        public ObservableCollection<Host> Hosts
        {
            get => hosts;
            set => this.RaiseAndSetIfChanged(ref hosts, value);
        }
    }
}