using LazyApiPack.Mvvm.Showcaseworks.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Showcase.Module.SidebarViewModels
{
    public class SolutionExplorerViewModel
    {
        private SolutionStore _store;
        public SolutionExplorerViewModel(SolutionStore store)
        {
            _store = store;
        }
    }
}
