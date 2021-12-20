using System.Xml.Serialization;

namespace Pattern.Adapter.Model
{
    [XmlRoot("product")]
    public class RemoteResponceObject
    {
        public int productId { get; set; }
        public string source { get; set; }
        public string productName { get; set; }
        public string info { get; set; }
        public string color { get; set; }
        public decimal weight { get; set; }
        public decimal price { get; set; }
        public int itemsForSale { get; set; }
    }
}
