using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Avalonia;
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

        public ObservableCollection<ScanType> scanTypes { get; } = new ObservableCollection<ScanType>()
        {
            new("-sS", "SYN Half open scan", "Fast and stealthy"),
            new("-sT", "TCP connect() scan", "Makes a full tcp connection, slow"),
            new("-sU", "UDP scan", "Checks UDP ports, not very reliable"),
            new("-sA", "ACK scan", "Map firewall rulesets (probably don't use)"),
            new("-sW", "Window scan", "Same as ack, but with ability to detect open ports"),
            new("-sM", "Maimon scan", "same as NULL, FIN, and Xmas, but with FIN/ACK"),
            new("-sN", "TCP NULL scan", "Sends a null packet, even stealthier than SYN"),
            new("-sF", "TCP FIN scan", "Sends a FIN packet, stealthier than SYN"),
            new("-sX", "TCP Xmas scan", "Sends a random packet, stealthier than SYN"),
        };

        private ScanType selectedScanType;

        public ScanType SelectedScanType
        {
            get => selectedScanType;
            set => this.RaiseAndSetIfChanged(ref selectedScanType, value);
        }

        public Scan()
        {
            SelectedScanType = scanTypes[0];
        }
        
        public void scan()
        {
            Directory.CreateDirectory("scans");
            string command = ip + " -v" +
                             $" {SelectedScanType.flag}" +
                             $" {Ports}" +
                             (Sv? " -sV": "") +
                             (Pn? " -Pn": "") +
                             (Agressive? " -A": "") +
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
        
        private string ports = "";

        public string Ports
        {
            get => ports;
            set => this.RaiseAndSetIfChanged(ref ports, value);
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
        
        private bool aggressive = false;

        public bool Agressive
        {
            get => aggressive;
            set => this.RaiseAndSetIfChanged(ref aggressive, value);
        }
        
        private bool osd = true;

        public bool Osd
        {
            get => osd;
            set => this.RaiseAndSetIfChanged(ref osd, value);
        }
        
        private bool pn = false;

        public bool Pn
        {
            get => pn;
            set => this.RaiseAndSetIfChanged(ref pn, value);
        }

        private int speed = 3;

        public int Speed
        {
            get => speed;
            set => this.RaiseAndSetIfChanged(ref speed, value);
        }

        public void copySelectedIp()
        {
            Application.Current.Clipboard.SetTextAsync(selectedHost.ip);
        }
    }

    public class ScanType
    {
        public string flag { get; }
        public string description { get; }
        public string tooltip { get; }

        public ScanType(string flag, string description, string tooltip)
        {
            this.flag = flag;
            this.description = description;
            this.tooltip = tooltip;
        }
    }
}