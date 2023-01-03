using System;
using System.Collections.Generic;
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
    }
}