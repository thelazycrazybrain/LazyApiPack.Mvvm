namespace LazyApiPack.Mvvm.Wpf.Model
{
    /// <summary>
    /// Indicates, that the viewmodel exposes a model property
    /// </summary>
    public interface ISupportModel
    {
        object? Model { get; set; }
    }

    /// <summary>
    /// Indicates, that the viewmodel exposes a model property
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface ISupportModel<TModel> : ISupportModel
    {
        new TModel? Model { get; set; }
    }
}
