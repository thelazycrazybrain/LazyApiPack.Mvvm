namespace LazyApiPack.Mvvm.Wpf.Application
{
    public interface IWindowTemplate
    {
        public bool IsVisible { get; }
        public void Close();
        public void Show();
        public bool? ShowDialog();

    }
}
