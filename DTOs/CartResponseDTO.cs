namespace abibas_back.DTOs
{
    public class CartResponseDTO
    {
        public List<CartResponseDTO> CartProducts { get; set; } = new();
        public int TotalItemsCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
