using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Msploit_X.Models;
using Msploit_X.Models.nmap;
using ReactiveUI;

namespace Msploit_X.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private GHDB ghdb = new GHDB();
        private ExploitsSearch exploitsSearch = new ExploitsSearch();

        private Scan scan = new Scan();

        private ReverseShell reverseShell = new ReverseShell();

        public static Exploits exploits = new Exploits();

        public GHDB Ghdb
        {
            get => ghdb;
            set => this.RaiseAndSetIfChanged(ref ghdb, value);
        }

        public ExploitsSearch ExploitsSearch
        {
            get => exploitsSearch;
            set => this.RaiseAndSetIfChanged(ref exploitsSearch, value);
        }

        public Scan Scan
        {
            get => scan;
            set => this.RaiseAndSetIfChanged(ref scan, value);
        }
        
        public ReverseShell ReverseShell
        {
            get => reverseShell;
            set => this.RaiseAndSetIfChanged(ref reverseShell, value);
        }

        public Exploits Exploits
        {
            get => exploits;
            set => this.RaiseAndSetIfChanged(ref exploits, value);
        }

        public void open_link(string link)
        {
            Process.Start(new ProcessStartInfo(link) {UseShellExecute = true});
        }
    }
}