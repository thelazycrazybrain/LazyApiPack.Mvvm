namespace LazyApiPack.Mvvm.Model
{
    /// <summary>
    /// Indicates, that the viewmodel exposes a parameter property
    /// </summary>
    public interface ISupportParameter
    {
        object? Parameter { get; set; }
    }

    /// <summary>
    /// Indicates, that the viewmodel exposes a parameter property
    /// </summary>
    public interface ISupportParameter<TParameter> : ISupportModel
    {
        TParameter? Parameter { get; set; }
    }
}
