namespace abibas_back.Models
{
    /// <summary>
    /// Profile item
    /// </summary>
    public class UserProfileItem
    {
        public long UserId {  get; set; }
        public UserItem User {  get; set; }
        public long ProductId {  get; set; }
        public ProductItem Product { get; set; }
    }
}
