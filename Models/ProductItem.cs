using abibas_back.Enums;
using Microsoft.Build.ObjectModelRemoting;

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
        public List<ProductImage> ProductImages { get; set; } = new();  
        public List<TechnicalDetailItem> TechnicalDetails { get; set; } = new();
        public List<Tag> ProductTags { get; set; } = new();
    }
}
