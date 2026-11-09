namespace abibas_back.Models
{
    public class Product
    {
        public int Id { get; set;  }
        public string Description {  get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool IsSizeAndFit {  get; set; }
        public bool IsFreeShipping {  get; set; }
        public bool IsFreeShippingAndReturns {  get; set; }
        public string Fabric { get; set; }
        public string Pattern { get; set; }
        public string Fit { get; set; }
        public string Neck { get; set; }
        public string Sleeve { get; set; }
        public string Style { get; set; }
    }
}
