using LazyApiPack.Mvvm.Wpf.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Model.BaseImpl
{
    public class ViewModelBase<TModel, TParameter> : NotifyObject, ISupportModel<TModel>, ISupportParameter<TParameter>
    {
        private TParameter? _parameter;
        public TParameter? Parameter
        {
            get => _parameter;
            set
            {
                SetPropertyValue(ref _parameter, value);
                OnParameterChanged(value);
            }
        }

        private TModel? _model;
        public TModel? Model
        {
            get => _model;
            set
            {
                SetPropertyValue(ref _model, value);
                OnModelChanged(value);
            }
        }

        protected virtual void OnModelChanged(TModel? model)
        {

        }
        protected virtual void OnParameterChanged(TParameter? parameter)
        {

        }
    }
}
