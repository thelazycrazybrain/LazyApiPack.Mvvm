﻿using LazyApiPack.Collections.Extensions;
using System.Collections.ObjectModel;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Regions {
    public interface IRegionAdapter {
        void AddView(object view);
        void RemoveView(object view);

    }
    public abstract class RegionAdapter<TPresenter> : IRegionAdapter
                                                        where TPresenter : UIElement {
        protected RegionAdapter(TPresenter presenterControl) {
            PresenterControl = presenterControl;
        }
        public abstract void AddView(object view);
        public abstract void RemoveView(object view);
        /// <summary>
        /// The item on the ui that is the navigation
        /// </summary>
        /// <param name="presenterControl"></param>
        protected TPresenter PresenterControl { get; private set; }

    }

   


}
