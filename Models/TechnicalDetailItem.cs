using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace abibas_back.Models
{
    /// <summary>
    /// Product details model
    /// </summary>
    public class TechnicalDetailItem
    {
        public long Id { get; set; }

        public long ProductItemId {  get; set; }
        public ProductItem Product { get; set; }

        public string Name { get; set; } = null!;

        public string Value { get; set; } = null!;

        public int SortOrder {  get; set; }
    }
}
