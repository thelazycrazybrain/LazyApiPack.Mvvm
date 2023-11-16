using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Model
{
    public interface ISupportModel
    {
        object? Model { get; set; }
    }
    public interface ISupportModel<TModel> : ISupportModel
    {
        new TModel? Model { get; set; }
    }
}
