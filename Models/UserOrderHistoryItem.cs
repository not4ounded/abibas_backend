namespace abibas_back.Models
{
    /// <summary>
    /// Order history
    /// </summary>
    public class UserOrderHistoryItem
    {
        public long UserItemId {  get; set; }
        public UserItem User {  get; set; }
        public long ProductItemId {  get; set; }
        public ProductItem Product { get; set; }
    }
}
