using System.ComponentModel.DataAnnotations;

namespace abibas_back.Models
{
    public class PaymentMethod
    {
        [Key]
        public long Id {  get; set; }
        public long UserItemId { get; set; }
        public string Last4 {  get; set; }
        public string Brand { get; set; }
        public string PaymentToken { get; set; }
    }
}
