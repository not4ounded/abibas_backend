using abibas_back.Enums;

namespace abibas_back.DTOs
{
    /// <summary>
    /// DTO for product card
    /// </summary>
    public class ProfuctShortDTO
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription {  get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; } = null!;
        public ColorItem CurrentColorItem { get; set; }
        public Tag CurrentTag { get; set; }
    }
}
