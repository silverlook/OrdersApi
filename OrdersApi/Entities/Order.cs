namespace OrdersApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FulfilledAt { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int StatusId { get; set; }
        public Status Status { get; set; } = null!;

        public ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();
    }
}
