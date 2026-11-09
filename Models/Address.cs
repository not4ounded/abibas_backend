namespace abibas_back.Models
{
    public class Address
    {
        public int UserId {  get; set; }
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public string Country {  get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Region {  get; set; }
        public string Phone {  get; set; }
        public string PostalCode { get; set; }
        public string DeliveryInstruction { get; set; }
        public bool IsDefault {  get; set; }
    }
}
