using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace Msploit_X.Models
{
    public class WebServer : ReactiveObject
    {
        public WebServer()
        {
            
        }
        
        public HttpListener listener;

        private bool running = false;
        private int port = 1338;

        private string consoleLog = "";

        private ObservableCollection<WebPage> pages = new ObservableCollection<WebPage>();
        private WebPage selectedPage;

        public bool Running
        {
            get => running;
            set => this.RaiseAndSetIfChanged(ref running, value);
        }

        public int Port
        {
            get => port;
            set => this.RaiseAndSetIfChanged(ref port, value);
        }

        public ObservableCollection<WebPage> Pages
        {
            get => pages;
            set => this.RaiseAndSetIfChanged(ref pages, value);
        }

        public WebPage SelectedPage
        {
            get => selectedPage;
            set => this.RaiseAndSetIfChanged(ref selectedPage, value);
        }

        public string ConsoleLog
        {
            get => consoleLog;
            set => this.RaiseAndSetIfChanged(ref consoleLog, value);
        }

        public void start()
        {
            Running = true;
            ConsoleLog = "";
            new Thread(() =>
            {
                ConsoleLog += $"Starting http listener on 0.0.0.0:{Port}\n";
                // Create a Http server and start listening for incoming connections
                listener = new HttpListener();
                listener.Prefixes.Add($"http://+:{Port}/");
                try
                {
                    listener.Start();
                }
                catch (HttpListenerException e)
                {
                    ConsoleLog += "Failed to start, are you running as administrator?\n";
                    ConsoleLog += e.Message + "\n" + e.StackTrace + "\n";
                    Running = false;
                }
                

                // Handle requests
                Task listenTask = HandleIncomingConnections();
                listenTask.GetAwaiter().GetResult();

                // Close the listener
                listener.Close();
            }).Start();
        }

        public void stop()
        {
            Running = false;
        }

        public void removeSelected()
        {
            Pages.Remove(SelectedPage);
        }

        public void addPath()
        {
            pages.Add(new WebPage("/", "text/html", Encoding.UTF8.GetBytes("<b>Hello world!</b>")));
        }

        public async Task HandleIncomingConnections()
        {
            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (Running)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                ConsoleLog += $"{req.RemoteEndPoint.Address}:{req.RemoteEndPoint.Port} => {req.LocalEndPoint.Address}:{req.LocalEndPoint.Port}{req.Url.AbsolutePath}\n";

                foreach (var page in Pages)
                {
                    if (page.url.ToLower().Equals(req.Url.AbsolutePath.ToLower()))
                    {
                        // Write the response info
                        byte[] data = page.data;
                        resp.ContentType = page.contentType;
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;

                        // Write out to the response stream (asynchronously), then close it
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();
                        break;
                    }
                }
            }
        }
    }

    public class WebPage : ReactiveObject
    {
        public string url;
        public byte[] data;

        public string dataString
        {
            get => Encoding.UTF8.GetString(data);
            set => this.RaiseAndSetIfChanged(ref data, Encoding.UTF8.GetBytes(value));
        }

        public string contentType;

        public string ContentType
        {
            get => contentType;
            set => this.RaiseAndSetIfChanged(ref contentType, value);
        }
        
        public string Url
        {
            get => url;
            set => this.RaiseAndSetIfChanged(ref url, value);
        }

        public WebPage(string url, string contentType, byte[] data)
        {
            this.url = url;
            this.contentType = contentType;
            this.data = data;
        }
    }
}