using abibas_back.Models;

namespace abibas_back.DTOs
{
    public class UserProfileItemDTO
    {
        public long UserId {  get; set; }
        public UserItem User {  get; set; }
        public long ProductId {  get; set; }
        public List<ProductItem> Products { get; set; }
    }
}
