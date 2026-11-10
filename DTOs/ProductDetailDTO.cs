using abibas_back.Enums;

namespace abibas_back.DTOs
{
    /// <summary>
    /// DTO for product card 
    /// </summary>
    public class ProductDetailDTO
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int AverageRating { get; set; }
        public ColorItem Color { get; set; }
        public Tag CurrentTag { get; set; }
        public List<SizeItem> Sizes { get; set; } = new();
        public List<ProductImageDTO> Images {  get; set; } = new();
        public List<TechnicalDetailDTO> TechnicalDetails { get; set;} = new();
    }
}
