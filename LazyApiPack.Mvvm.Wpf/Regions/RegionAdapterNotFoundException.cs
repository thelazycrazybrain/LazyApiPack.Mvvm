namespace LazyApiPack.Mvvm.Wpf.Regions
{
    [Serializable]
    public class RegionAdapterNotFoundException : Exception
    {
        const string REGIONMSG = "Adapter to the region '{0}' was not registered. Ensure that you used the extension 'WithRegionAdapter' on you module configuration.";
        public RegionAdapterNotFoundException() { }
        public RegionAdapterNotFoundException(string regionName) : base(string.Format(REGIONMSG, regionName)) { }
        public RegionAdapterNotFoundException(string message, string region, Exception inner) : base(string.Format(REGIONMSG, region) + "\r\n" + message, inner) { }
        protected RegionAdapterNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
