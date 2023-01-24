namespace Brainstorm.Mvvm
{
    public class Navigation : IDisposable
    {
        private Navigation()
        {

        }
        public static void Start(string[] viewNamespaces, string[] viewModelNamespaces, string[] serviceNamespaces )
        {
            if (Instance == null)
            {
                Instance = new Navigation();
            }
        }

        public static void Stop()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance = null;
            }
        }
        
        public static Navigation Instance { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }
        ~Navigation()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing)
        {

        }

    }
}