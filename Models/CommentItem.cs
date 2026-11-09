namespace abibas_back.Models
{
    public class CommentItem
    {
        public long Id { get; set; }
        public long UserId {  get; set; }
        public long ProductId {  get; set; }
        public string? AuthorName {  get; set; }
        public string? Text {  get; set; }
    }
}
