using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Model
{
    public interface ISupportParameter<TParameter>
    {
        TParameter? Parameter { get; set; }
    }
}
