using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Msploit_X.Models.nmap;
using Msploit_X.ViewModels;
using NmapXmlParser;

namespace Msploit_X
{
    public class Util
    {
        public static void runCmd(string program, string? command)
        {
            ProcessStartInfo cmdsi = new ProcessStartInfo(program);
            if (command != null) cmdsi.Arguments = command;
            cmdsi.UseShellExecute = false;
            Process? cmd = Process.Start(cmdsi);
            if (cmd != null) cmd.WaitForExit();
        }
        
        public static Result updateHostFromScan(string resultFile)
        {
            if (!File.Exists(resultFile))
            {
                return new Result() {Hosts = new ObservableCollection<Host>(), ScanName = DateTime.Now.ToShortTimeString()};
            }
            var xmlSerializer = new XmlSerializer(typeof(nmaprun));
            var result = default(nmaprun);
            ObservableCollection<Host> hosts = new ObservableCollection<Host>();
            try
            {
                using (var xmlStream = new StreamReader(resultFile))
            {
                result = xmlSerializer.Deserialize(xmlStream) as nmaprun;
                foreach (var host1var in result.Items)
                {
                    if (host1var != null && host1var.GetType() == typeof(host))
                    {
                        NmapXmlParser.host host1 = (host) host1var;
                        Host found = new Host(host1.address.addr, true);

                        found.ip = host1.address.addr;
                        found.up = host1.status.state == statusState.up;
                        
                        if (host1.Items == null) continue;
                        
                        foreach (var item in host1.Items)
                        {
                            Type type = item.GetType();
                            switch (item)
                            {
                                case os osInfo:
                                    if (osInfo.osmatch != null)
                                    {
                                        foreach (var match in osInfo.osmatch)
                                        {
                                            String s = "";
                                            foreach (osclass osClass in match.osclass)
                                            {
                                                found.Os = osClass.osfamily.ToLower() switch
                                                {
                                                    "linux" => Host.OS.linux,
                                                    "windows" => Host.OS.win,
                                                    "macos" => Host.OS.osx,
                                                    _ => Host.OS.other_unknown
                                                };
                                                found.OsVer = osClass.osgen;
                                                break;
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                case ports portInfo:
                                    found.ports = new();
                                    if (portInfo.port != null)
                                    {
                                        foreach (port portFound in portInfo.port)
                                        {
                                            Port addport = new Port();
                                            addport.type = portFound.protocol.ToString();
                                            addport.portNum = portFound.portid;
                                            addport.state = portFound.state.state1;
                                            addport.protocol = portFound.protocol.ToString();
                                            addport.service = portFound.service.name;
                                            addport.serviceProduct = portFound.service.product;
                                            addport.serviceVersion = portFound.service.version;
                                            found.ports.Add(addport);
                                        }
                                    }
                                    break;
                                case address addr:
                                    if (addr.addrtype == addressAddrtype.ipv4)
                                    {
                                        found.ip = addr.addr;
                                    }
                                    break;
                            }
                        }
                        hosts.Add(found);
                    }
                }
            }
            }
            catch (Exception e)
            {
                return new Result() {Hosts = hosts, ScanName = DateTime.Now.ToShortTimeString() + " -Cancelled"};
            }
            return new Result() {Hosts = hosts, ScanName = DateTime.Now.ToShortTimeString()};
        }
    }
}