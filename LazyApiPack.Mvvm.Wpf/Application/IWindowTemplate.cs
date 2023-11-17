namespace LazyApiPack.Mvvm.Wpf.Application
{
    public interface IWindowTemplate
    {
        event EventHandler Closed;
        public bool IsVisible { get; }
        public void Close();
        public void Show();
        public bool? ShowDialog();
        public object Content { get; set; }
        
    }
}
