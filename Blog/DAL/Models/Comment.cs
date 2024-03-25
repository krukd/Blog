using System.ComponentModel.DataAnnotations;

namespace Blog.DAL.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostedDate { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }
        public int ArticleId { get; set; }

    }
}
