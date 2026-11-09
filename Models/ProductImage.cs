namespace abibas_back.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string URL { get; set; } = null!;
        public int SortOrder {  get; set; }
        public string Caption { get; set; }

        public int ProductId {  get; set; }
        public ProductItem ProductItem { get; set; } = null!;
    }
}
