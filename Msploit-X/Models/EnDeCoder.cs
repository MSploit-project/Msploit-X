using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using Msploit_X.ViewModels;
using ReactiveUI;

namespace Msploit_X.Models
{
    public class EnDeCoder : ViewModelBase
    {
        public void Insert(string content)
        {
            string[] split = content.Split("|");
            content = split[0];
            int offsetPos = int.Parse(split[1]);
            if (inputEverFocussed)
            {
                InputFocussed = true;
                InputText = InputText.ReplaceAt(inputSelectionStart, inputSelectionEnd - inputSelectionStart, content);
                inputSelectionStart += offsetPos;
                inputSelectionEnd = inputSelectionStart;
                OutputText = currentMode.encode.Invoke(inputText == null? "":inputText);
            }
        }

        private int[] inputSelection = new int[2];
        public int inputSelectionStart
        {
            get => inputSelection[0];
            set => this.RaiseAndSetIfChanged(ref inputSelection[0], value);
        }
        public int inputSelectionEnd
        {
            get => inputSelection[1];
            set => this.RaiseAndSetIfChanged(ref inputSelection[1], value);
        }
        
        private string inputText;
        public string InputText
        {
            get => inputText;
            set
            {
                this.RaiseAndSetIfChanged(ref inputText, value);
                if (InputFocussed)
                    OutputText = CurrentMode.encode.Invoke(value);
            }
        }

        private string outputText;
        public string OutputText
        {
            get => outputText;
            set
            {
                this.RaiseAndSetIfChanged(ref outputText, value);
                if (OutputFocussed)
                    InputText = CurrentMode.decode.Invoke(value);
            }
        }

        private bool inputFocussed, outputFocussed, inputEverFocussed;
        public bool InputFocussed
        {
            get => inputFocussed;
            set
            {
                this.RaiseAndSetIfChanged(ref inputFocussed, value);
                inputEverFocussed = inputFocussed || inputEverFocussed;
            }
        }

        public bool OutputFocussed { get => outputFocussed; set => this.RaiseAndSetIfChanged(ref outputFocussed, value); }

        public class Mode : ViewModelBase
        {
            private string name;
            public string Name
            {
                get => name;
                set => this.RaiseAndSetIfChanged(ref name, value);
            }
            
            private string description;
            public string Description
            {
                get => description;
                set => this.RaiseAndSetIfChanged(ref description, value);
            }

            public Func<string, string> encode, decode;

            public Mode(string name, string description, Func<string, string> encode, Func<string, string> decode)
            {
                Name = name;
                Description = description;
                this.encode = encode;
                this.decode = decode;
            }
        }
        
        public ObservableCollection<Mode> AllModes { get; } = new()
        {
            new Mode("Base64", "Encode/Decode to/from base64", encodeString => encodeString.Base64Encode(), decodeString => decodeString.Base64Decode()),
            new Mode("UrlEncode", "Encode/Decode to/from text that can be put in a url GET parameter.", s => s.UrlEncode(), s => s.UrlDecode()),
            new Mode("HtmlEncode", "Encode/Decode to/from text that can be used in html.", s => s.HtmlEncode(), s => s.HtmlDecode()),
            new Mode("LfiRCE", "Encode/Decode to/from a local file inclusion payload", s => s.LFIEncode(), s => s.LFIDecode()),
            new Mode("LfiRCE With NullByte", "Encode/Decode to/from a local file inclusion payload with nullbyte at end", s => s.LFINBEncode(), s => s.LFINBDecode()),
            new Mode("Hex string", "Encode/Decode to/from a Hex string", s => s.HEXEncode(), s => s.HEXDecode()),
        };

        private Mode currentMode;
        public Mode CurrentMode
        {
            get => currentMode;
            set
            {
                this.RaiseAndSetIfChanged(ref currentMode, value);
                OutputText = value.encode.Invoke(inputText == null? "":inputText);
            }
        }

        public EnDeCoder()
        {
            CurrentMode = AllModes[0];
            inputSelectionStart = 0;
            inputSelectionEnd = 0;
            InputFocussed = true;
            InputText = "";
        }
    }

    public static class StringExtensions
    {
        public static string ReplaceAt(this string str, int index, int length, string replace)
        {
            return str.Remove(index, Math.Min(length, str.Length - index))
                .Insert(index, replace);
        }
        
        public static string Base64Encode(this string encode)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encode);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        
        public static string Base64Decode(this string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string UrlEncode(this string text)
        {
            return HttpUtility.UrlEncode(text);
        }
        
        public static string UrlDecode(this string text)
        {
            return HttpUtility.UrlDecode(text);
        }
        
        public static string HtmlEncode(this string text)
        {
            return HttpUtility.HtmlEncode(text);
        }
        
        public static string HtmlDecode(this string text)
        {
            return HttpUtility.HtmlDecode(text);
        }
        
        public static string LFIEncode(this string text)
        {
            return "data://text/plain;base64," + text.Base64Encode();
        }
        
        public static string LFIDecode(this string text)
        {
            if (text.StartsWith("data://text/plain;base64,"))
            {
                text = text.Substring("data://text/plain;base64,".Length);
            }

            return text.Base64Decode();
        }
        
        public static string LFINBEncode(this string text)
        {
            return "data://text/plain;base64," + text.Base64Encode() + "%00";
        }
        
        public static string LFINBDecode(this string text)
        {
            if (text.StartsWith("data://text/plain;base64,"))
            {
                text = text.Substring("data://text/plain;base64,".Length);
            }

            if (text.EndsWith("%00"))
            {
                text = text.Substring(0, text.Length - 3);
            }

            return Base64Decode(text);
        }
        
        public static string HEXEncode(this string text)
        {
            return BitConverter.ToString(Encoding.Default.GetBytes(text)).Replace("-", "");
        }
        
        public static string HEXDecode(this string text)
        {
            return Encoding.Default.GetString(Convert.FromHexString(text.Replace(" ", "").Replace("-", "")));
        }
        
    }
}