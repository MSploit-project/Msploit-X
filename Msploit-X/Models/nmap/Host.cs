using System.Collections.Generic;
using System.Collections.ObjectModel;
using Msploit_X.ViewModels;
using ReactiveUI;

namespace Msploit_X.Models.nmap
{
    public class Host
    {
        public enum OS
        {
            win, osx, linux, other_unknown
        }
        public string ip { get; set; }
        public bool up { get; set; }
        public List<Port> ports { get; set; }

        public string lanIp { get; set; }
        public OS Os { get; set; }
        public string OsVer { get; set; }
        public string HostName { get; set; }

        public string OsString => Os switch
        {
            OS.linux => "Linux",
            OS.osx => "Mac Os",
            OS.win => "Windows",
            OS.other_unknown => "Other/Unknown",
            _ => ""
        } + " " + OsVer;

        public Host(string ip, bool up, string lanIp, OS Os)
        {
            this.ip = ip;
            this.up = up;
            this.lanIp = lanIp;
            this.Os = Os;
            ports = new();
        }
        
        public Host(string ip, bool up, OS Os)
        {
            this.ip = ip;
            this.up = up;
            lanIp = "xxx.xxx.xxx.xxx";
            this.Os = Os;
            ports = new();
        }
        
        public Host(string ip, bool up, string lanIp)
        {
            this.ip = ip;
            this.up = up;
            this.lanIp = lanIp;
            Os = OS.other_unknown;
            ports = new();
        }
        
        public Host(string ip, bool up)
        {
            this.ip = ip;
            this.up = up;
            lanIp = "xxx.xxx.xxx.xxx";
            Os = OS.other_unknown;
            ports = new();
        }

        public Host()
        {
            ports = new();
        }
    }

    public class Port
    {
        public string portNum { get; set; }
        public string? service { get; set; }
        public string? serviceProduct { get; set; }
        public string serviceVersion { get; set; }
        public string protocol { get; set; }
        public string state { get; set; }
        public string type { get; set; }

        public string asText
        {
            get => $"{type} - {portNum} - {state} - {service} - {serviceProduct} - {serviceVersion}";
        }
    }
}