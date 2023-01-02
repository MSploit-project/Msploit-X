using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reactive;
using System.Xml;
using System.Xml.Serialization;
using CsvHelper;
using ReactiveUI;

namespace Msploit_X.Models
{
    public class GHDB : ReactiveObject
    {
        public GHDB()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ghdb";
            xRoot.IsNullable = true;
            XmlSerializer reader = new XmlSerializer(typeof(DeSerializer), xRoot);
            StreamReader file = new StreamReader("exploitdb/ghdb.xml");
            DeSerializer itemsS = (DeSerializer)reader.Deserialize(file);
            file.Close();

            foreach (var item in itemsS.ghdb)
            {
                itemsB.Add(item);
            }

            items = itemsB;
        }

        private string search;

        public string Search
        {
            get => search;
            set
            {
                this.RaiseAndSetIfChanged(ref search, value);
                var itemsNew = new ObservableCollection<GHDBItem>();
                foreach (var item in ItemsB)
                {
                    if (item.ShortDescription.ToLower().Contains(value.ToLower()) || item.TextualDescription.ToLower().Contains(value.ToLower()))
                    {
                        itemsNew.Add(item);
                    }
                }

                Items = itemsNew;
            }
        }

        private ObservableCollection<GHDBItem> items = new ObservableCollection<GHDBItem>();

        public ObservableCollection<GHDBItem> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        private ObservableCollection<GHDBItem> itemsB = new ObservableCollection<GHDBItem>();

        public ObservableCollection<GHDBItem> ItemsB
        {
            get => itemsB;
            set => this.RaiseAndSetIfChanged(ref itemsB, value);
        }
    }

    public class GHDBItem
    {
        public GHDBItem()
        {
            
        }

        public void openLink()
        {
            Process.Start(new ProcessStartInfo(Link) {UseShellExecute = true});
        }
        
        [XmlElement("shortDescription")]
        public string shortDescription;
        [XmlElement("textualDescription")]
        public string textualDescription;
        [XmlElement("link")]
        public string link;

        public string ShortDescription
        {
            get => shortDescription;
        }

        public string TextualDescription
        {
            get => textualDescription;
        }

        public string Link
        {
            get => link;
        }
    }

    [XmlRoot("ghdb")]
    public class DeSerializer
    {
        [XmlElement("entry")]
        public List<GHDBItem> ghdb;
    }
}