namespace OrdersApi.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;  // kolumna w bazie to "Nam" — skonfigurujemy w DbContext

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
