using abibas_back.Enums;
using System.ComponentModel.DataAnnotations;
namespace abibas_back.Models
{
    public class CheckoutItem
    {
        [Key]
        public long Id { get; set;  }
        public long UserItemId {  get; set; }
        public string Region {  get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ApartmentNumber {  get; set; }
        public DeliveryType Delivery {  get; set; }
        [CreditCard(ErrorMessage = "Введите корректный номер карты")] 
        public string CreditCardNumber { get; set; }
        [RegularExpression(@"^((1[0-2])|([0-9]))\/\d{2}$")]
        public string CardExpired { get; set; }
        [Length(3, 3, ErrorMessage = "Введите корректный CVV")]
        public string CVV { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string? PromoCode { get; set; }
    }
}
