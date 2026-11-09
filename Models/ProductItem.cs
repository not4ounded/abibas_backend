using abibas_back.Enums;

namespace abibas_back.Models
{
    public class ProductItem
    {
        public long Id { get; set;  }
        public string? Description {  get; set; }
        public string? ShortDescription {  get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public List<SizeItem> Sizes { get; set; } = new();
        public List<ColorItem> Colors { get; set; } = new();
        public bool IsFreeShipping {  get; set; }
        public bool IsFreeShippingAndReturns {  get; set; }
        public string? Fabric { get; set; }
        public string? Pattern { get; set; }
        public string? Fit { get; set; }
        public string? Neck { get; set; }
        public string? Sleeve { get; set; }
        public string? Style { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new();   
    }
}
