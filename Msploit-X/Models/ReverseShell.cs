using System;
using System.Runtime.InteropServices;
using System.Threading;
using ReactiveUI;

namespace Msploit_X.Models
{
    public class ReverseShell : ReactiveObject
    {
        private int port = 1337;

        public int Port
        {
            get => port;
            set => this.RaiseAndSetIfChanged(ref port, value);
        }

        public void openShell(int port)
        {
            string command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "ncat" : "nc";
            new Thread(() =>
            {
                Util.runCmd(command, $"-lvp {port}");
            }).Start();
        }
    }
}