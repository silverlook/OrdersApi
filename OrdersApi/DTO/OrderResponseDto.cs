namespace OrdersApi.DTO
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FulfilledAt { get; set; }
        public string Status { get; set; } = null!;
        public ClientDto Client { get; set; } = null!;
        public List<ProductDto> Products { get; set; } = new();
    }
    public class ClientDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }

    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
