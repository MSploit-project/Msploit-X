﻿using System;
using System.Collections.Generic;
using System.Text;
using Msploit_X.Models;
using ReactiveUI;

namespace Msploit_X.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private GHDB ghdb = new GHDB();

        public GHDB Ghdb
        {
            get => ghdb;
            set => this.RaiseAndSetIfChanged(ref ghdb, value);
        }
    }
}