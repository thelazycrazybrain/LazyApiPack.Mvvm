using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Model
{
    public interface ISupportModel<TModel>
    {
        TModel? Model { get; set; }
    }
}
