using MongoDB.Bson.Serialization.Attributes;

namespace CQRSQuery.Models
{
    public class OrderQuery
    {
        [BsonId]
        [BsonSerializer(typeof(FlexibleIdSerializer))]
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
