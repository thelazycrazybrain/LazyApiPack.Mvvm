using LazyApiPack.Utils.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Model.BaseImpl
{
    public class ViewModelBase : NotifyObject
    {
        protected object? _parameter;
        protected object? _model;

        public object? Parameter
        {
            get => _parameter;
            set
            {
                SetPropertyValue(ref _parameter, value);
            }
        }

        public object? Model
        {
            get => _model;
            set
            {
                SetPropertyValue(ref _model, value);
            }
        }


    }
    public class ViewModelBase<TModel, TParameter> : ViewModelBase, ISupportModel<TModel>, ISupportParameter<TParameter>
    {
        public new TParameter? Parameter
        {
            get => (TParameter?)_parameter;
            set
            {
                SetPropertyValue(ref _parameter, value);
                OnParameterChanged(value);
            }
        }

       
        public new TModel? Model
        {
            get => (TModel?)_model;
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
