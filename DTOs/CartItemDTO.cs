using abibas_back.Models;

namespace abibas_back.DTOs
{
    public class CartItemDTO
    {
        public long UserId { get; set; }
        public long ProductId {  get; set; }
        public List<ProductItem> Products { get; set; } = new();
    }
}