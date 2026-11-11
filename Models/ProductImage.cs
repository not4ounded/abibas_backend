namespace abibas_back.Models
{
    public class ProductImage
    {
        public long Id { get; set; }

        public string URL { get; set; } = null!;
        public int SortOrder {  get; set; }
        public string Caption { get; set; }

        public long ProductItemId {  get; set; }
        public ProductItem Product { get; set; } = null!;
    }
}
