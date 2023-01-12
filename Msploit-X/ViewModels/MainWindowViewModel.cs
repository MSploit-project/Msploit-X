using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Msploit_X.Models;
using Msploit_X.Models.nmap;
using Msploit_X.ViewModels.Objects;
using ReactiveUI;

namespace Msploit_X.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public bool False => false;

        public static MainWindowViewModel instance;

        public MainWindowViewModel()
        {
            instance = this;
        }
        
        private GHDB ghdb = new GHDB();
        private ExploitsSearch exploitsSearch = new ExploitsSearch();

        private Scan scan = new Scan();

        private ReverseShell reverseShell = new ReverseShell();

        public static Exploits exploits = new Exploits();

        private Fuzzer fuzzer = new Fuzzer();

        private WebServer webServer = new WebServer();

        private Cracker cracker = new Cracker();
        
        private EnDeCoder enDeCoder = new EnDeCoder();

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

        public Fuzzer Fuzzer
        {
            get => fuzzer;
            set => this.RaiseAndSetIfChanged(ref fuzzer, value);
        }

        public WebServer WebServer
        {
            get => webServer;
            set => this.RaiseAndSetIfChanged(ref webServer, value);
        }

        public Cracker Cracker
        {
            get => cracker;
            set => this.RaiseAndSetIfChanged(ref cracker, value);
        }

        public EnDeCoder EnDeCoder
        {
            get => enDeCoder; 
            set => this.RaiseAndSetIfChanged(ref enDeCoder, value);
        }

        public void open_link(string link)
        {
            Process.Start(new ProcessStartInfo(link) {UseShellExecute = true});
        }
    }
}