namespace LazyApiPack.Mvvm.Regions
{
    [Serializable]
    public class RegionNotFoundException : Exception
    {
        const string REGIONMSG = "The region '{0}' was not registered. Ensure that the containing Window is already open.";
        public RegionNotFoundException() { }
        public RegionNotFoundException(string regionName) : base(string.Format(REGIONMSG, regionName)) { }
        public RegionNotFoundException(string message, string region, Exception inner) : base(string.Format(REGIONMSG, region) + "\r\n" + message, inner) { }
        protected RegionNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
