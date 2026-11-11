using abibas_back.Enums;
using abibas_back.Models;
using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    public class CheckoutItemDTO
    {
        [Key]
        public long Id { get; set; }
        public long UserItemId {  get; set; }
        public List<CartItemDTO> Products { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.InProgress;
    }
}
