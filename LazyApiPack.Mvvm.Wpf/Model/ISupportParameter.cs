using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Model
{
    public interface ISupportParameter
    {
        object? Parameter { get; set; }
    }
    public interface ISupportParameter<TParameter> : ISupportModel
    {
        TParameter? Parameter { get; set; }
    }
}
