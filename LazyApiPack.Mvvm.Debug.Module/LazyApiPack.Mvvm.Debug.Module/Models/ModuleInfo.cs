namespace LazyApiPack.Mvvm.Debug.Module.Models
{
    public class ModuleInfo
    {
        public ModuleInfo(string id, string name, ModuleStatus status)
        {
            Id = id;
            Name = name;
            Status = status;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public ModuleStatus Status { get; set; }

    }
}
