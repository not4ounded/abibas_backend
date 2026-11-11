using abibas_back.Enums;

namespace abibas_back.Models
{
    /// <summary>
    /// Cart item
    /// </summary>
    public class CartItem
    {
        public long Id { get; set; }
        public int Quantity {  get; set; }
        public SizeItem SelectedSize {  get; set; }
        public ColorItem SelectedColor {  get; set; }
        public long UserItemId { get; set; }
        public UserItem User { get; set; }
        public long ProductItemId { get; set; }
        public ProductItem Product { get; set; }
    }
}
