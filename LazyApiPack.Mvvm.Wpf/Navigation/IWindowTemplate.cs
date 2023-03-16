namespace LazyApiPack.Mvvm.Wpf.Navigation
{
    public interface IWindowTemplate
    {
        public void Close();
        public void Show();
        public bool? ShowDialog();

    }
}
