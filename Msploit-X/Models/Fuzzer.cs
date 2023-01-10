using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Msploit_X.ViewModels;
using Msploit_X.Views;
using ReactiveUI;

namespace Msploit_X.Models
{
    public class Fuzzer : ReactiveObject
    {
        private Thread fuzzThread;

        private string target = "http://localhost/FUZZ";
        private string selectedFile = "Select File";
        private bool running = false;
        private int delay = 50;
        private int progress = 0;
        private int total = 0;
        private int mode = 0;
        private ObservableCollection<result> results = new ObservableCollection<result>();

        public string Target
        {
            get => target;
            set => this.RaiseAndSetIfChanged(ref target, value);
        }
        public string SelectedFile
        {
            get => selectedFile;
            set => this.RaiseAndSetIfChanged(ref selectedFile, value);
        }

        public bool Running
        {
            get => running;
            set => this.RaiseAndSetIfChanged(ref running, value);
        }

        public int Delay
        {
            get => delay;
            set => this.RaiseAndSetIfChanged(ref delay, value);
        }

        public int Progress
        {
            get => progress;
            set => this.RaiseAndSetIfChanged(ref progress, value);
        }
        
        public int Total
        {
            get => total;
            set => this.RaiseAndSetIfChanged(ref total, value);
        }

        public int Mode
        {
            get => mode;
            set => this.RaiseAndSetIfChanged(ref mode, value);
        }

        public ObservableCollection<result> Results
        {
            get => results;
            set => this.RaiseAndSetIfChanged(ref results, value);
        }

        public void openFileDialog()
        {
            try
            {
                Task<string[]> fileDialog = new OpenFileDialog().ShowAsync(MainWindow.instance);
                fileDialog.Wait();
                SelectedFile = fileDialog.Result[0];
            }
            catch (Exception e)
            {
            }
            
        }

        public void start()
        {
            if (SelectedFile == "Select File") return;
            Running = true;
            fuzzThread = new Thread(fuzz);
            fuzzThread.Start();
        }

        public void stop()
        {
            Running = false;
        }

        public void fuzz()
        {
            Results.Clear();
            string[] wordlistItems = File.ReadAllLines(selectedFile);
            Total = wordlistItems.Length;
            Progress = 0;

            ConcurrentBag<result> resultsBag = new ConcurrentBag<result>();
            Parallel.For(0, total, new ParallelOptions() {MaxDegreeOfParallelism = 50}, i =>
            {
                if (!Running) return;
                
                var item = wordlistItems[i];
                string targetUrl = target.Replace("FUZZ", item);
                
                if (checkUrl(targetUrl)) resultsBag.Add(new result() {Url = targetUrl});

                ++Progress;
            });

            foreach (var result in resultsBag)
            {
                Results.Add(result);
            }

            Progress = Total;

            Running = false;
        }

        private List<HttpStatusCode> ignoreWhen = new List<HttpStatusCode>()
        {
            HttpStatusCode.NotFound,
            HttpStatusCode.BadGateway,
            HttpStatusCode.BadRequest
        };

        public bool checkUrl(string url)
        {
            try
            {
                switch (Mode)
                {
                    case 0://WEB
                        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                        request.Timeout = 500;
                        request.Method = "GET";
                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        var statusCode = response.StatusCode;
                        response.Close();
                        return !ignoreWhen.Contains(statusCode);
                    case 1://PING
                        var pinger = new Ping();
                        PingReply reply = pinger.Send(url);
                        return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }
    }

    public class result
    {
        public string Url { get; set; }
    }
}