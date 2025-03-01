namespace CQRSCommand.Models
{
    public class OrderQuery
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
