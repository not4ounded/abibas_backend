using abibas_back.Enums;

namespace abibas_back.DTOs
{
    public class CollectionItemDTO
    {
        public long Id { get; set; }
        public List<View> Collections { get; set; }
    }
}
