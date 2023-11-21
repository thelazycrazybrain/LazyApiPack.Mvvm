using LazyApiPack.Utils.ComponentModel;
using System;
namespace LazyApiPack.Mvvm.Model.BaseImpl
{
    /// <summary>
    /// Base class for a viewmodel.
    /// </summary>
    public class ViewModelBase : NotifyObject
    {
        protected object? _parameter;
        protected object? _model;
        /// <summary>
        /// The parameter passed to the viewmodel.
        /// </summary>
        public object? Parameter
        {
            get => _parameter;
            set
            {
                SetPropertyValue(ref _parameter, value);
            }
        }
        /// <summary>
        /// The model passed to the viewmodel.
        /// </summary>
        public object? Model
        {
            get => _model;
            set
            {
                SetPropertyValue(ref _model, value);
            }
        }


    }

    /// <summary>
    /// Generic baseclass for a viewmodel
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    /// <typeparam name="TParameter">Type of the parameter.</typeparam>
    public class ViewModelBase<TModel, TParameter> : ViewModelBase, ISupportModel<TModel>, ISupportParameter<TParameter>
    {        
        /// <inheritdoc/>
        public new TParameter? Parameter
        {
            get => (TParameter?)_parameter;
            set
            {
                SetPropertyValue(ref _parameter, value);
                OnParameterChanged(value);
            }
        }

        /// <inheritdoc/>
        public new TModel? Model
        {
            get => (TModel?)_model;
            set
            {
                SetPropertyValue(ref _model, value);
                OnModelChanged(value);
            }
        }
        /// <summary>
        /// Invoked, when the model has changed.
        /// </summary>
        /// <param name="model"></param>
        protected virtual void OnModelChanged(TModel? model)
        {

        }

        /// <summary>
        /// Invoked, when the parameter has changed.
        /// </summary>
        /// <param name="parameter"></param>
        protected virtual void OnParameterChanged(TParameter? parameter)
        {

        }
    }
}
