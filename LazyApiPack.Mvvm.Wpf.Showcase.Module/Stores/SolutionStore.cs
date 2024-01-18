using LazyApiPack.XmlTools.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LazyApiPack.Mvvm.Showcaseworks.Stores
{
    [XmlClass("Solution")]
    public class SolutionStore
    {
        public string Name { get; set; }

        [XmlAttribute("Nodes")]
        public ObservableCollection<SolutionNode> Nodes { get; set; } = new();
    }

    [XmlClass("Node")]
    public class SolutionNode
    {
        public string Name { get; set; }
        public string Data { get; set; }
        public string DataType { get; set; }

    }
}
