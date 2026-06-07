namespace OrdersApi.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
