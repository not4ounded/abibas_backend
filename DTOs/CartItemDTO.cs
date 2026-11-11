using abibas_back.Enums;
using abibas_back.Models;

namespace abibas_back.DTOs
{
    public class CartItemDTO
    {
        public long UserItemId { get; set; }
        public long ProductItemId {  get; set; }
        public int Quantity { get; set; }
        public SizeItem SelectedSize { get; set; }
        public ProductShortDTO SelectedProduct { get; set; } 
    }
}