﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Application
{
    public interface IView
    {
        object DataContext { get; set; }
    }
}
