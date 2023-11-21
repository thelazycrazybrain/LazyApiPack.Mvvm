namespace LazyApiPack.Mvvm.Exceptions
{
    [Serializable]
    public class ViewModelNotFoundException : Exception
    {
        public ViewModelNotFoundException(string viewName) : base($"ViewModel {viewName} not found.")
        {

        }
        public ViewModelNotFoundException(string viewName, string message) : base($"ViewModel {viewName} not found.", new Exception(message))
        {
        }
        public ViewModelNotFoundException(string viewName, string message, Exception inner) : base($"ViewModel {viewName} not found.", new Exception(message, inner)) { }
        protected ViewModelNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


}

