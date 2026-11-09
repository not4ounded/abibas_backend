namespace abibas_back.Models
{
    public class Comment
    {
        public int ProductId {  get; set; }
        public int Grade { get; set; }
        public string AuthorName {  get; set; }
        public string Text {  get; set; }
    }
}
