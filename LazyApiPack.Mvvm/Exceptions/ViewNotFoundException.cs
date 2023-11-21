namespace LazyApiPack.Mvvm.Exceptions
{
    [Serializable]
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string viewName) : base($"View {viewName} not found.")
        {

        }
        public ViewNotFoundException(string viewName, string message) : base($"View {viewName} not found.", new Exception(message))
        {
        }
        public ViewNotFoundException(string viewName, string message, Exception inner) : base($"View {viewName} not found.", new Exception(message, inner)) { }
        protected ViewNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


}

