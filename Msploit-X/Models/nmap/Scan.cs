using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using ReactiveUI;

namespace Msploit_X.Models.nmap
{
    public class Scan : ReactiveObject
    {
        private ObservableCollection<Result> results = new ObservableCollection<Result>();

        public ObservableCollection<Result> Results
        {
            get => results;
            set => this.RaiseAndSetIfChanged(ref results, value);
        }
        
        private Result selectedResult;

        public Result SelectedResult
        {
            get => selectedResult;
            set => this.RaiseAndSetIfChanged(ref selectedResult, value);
        }
        
        private Host selectedHost;

        public Host SelectedHost
        {
            get => selectedHost;
            set => this.RaiseAndSetIfChanged(ref selectedHost, value);
        }

        public Scan()
        {
            
        }
        
        public void scan()
        {
            Directory.CreateDirectory("scans");
            string command = ip + " -v" +
                             (Sv? " -sV": "") +
                             (osd? " -O": "") +
                             $" -T{Speed}" +
                             $" {customArgs}";
            
            string dir = $"{Directory.GetCurrentDirectory()}\\scans\\{ip}.xml";
            command = command + $" -oX \"{dir}\"";
            CanScan = false;
            new Thread(() =>
            {
                Util.runCmd("nmap", command);
                Results.Add(Util.updateHostFromScan(dir));
                CanScan = true;
            }).Start();
        }

        private bool canScan = true;

        public bool CanScan
        {
            get => canScan;
            set => this.RaiseAndSetIfChanged(ref canScan, value);
        }
        
        private string ip = "localhost";

        public string Ip
        {
            get => ip;
            set => this.RaiseAndSetIfChanged(ref ip, value);
        }
        
        private string customArgs = "";

        public string CustomArgs
        {
            get => customArgs;
            set => this.RaiseAndSetIfChanged(ref customArgs, value);
        }
        
        private bool sv = true;

        public bool Sv
        {
            get => sv;
            set => this.RaiseAndSetIfChanged(ref sv, value);
        }
        
        private bool osd = true;

        public bool Osd
        {
            get => osd;
            set => this.RaiseAndSetIfChanged(ref osd, value);
        }

        private int speed = 3;

        public int Speed
        {
            get => speed;
            set => this.RaiseAndSetIfChanged(ref speed, value);
        }
    }
}