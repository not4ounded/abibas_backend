using System.ComponentModel.DataAnnotations;

namespace abibas_back.Models
{
    public class CommentItem
    {
        [Key]
        public long Id { get; set; }
        public long UserItemId {  get; set; }
        public long ProductItemId {  get; set; }
        public string AuthorName {  get; set; }
        public string Text {  get; set; }
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int Rating { get; set; }
    }
}
