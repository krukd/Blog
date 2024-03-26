using System.ComponentModel.DataAnnotations;

namespace Blog.DAL.Models
{
    public class EditArticleViewModel
    {
        public int Id { get; set; }

       
        public string? Title { get; set; }

       
        public string? Content { get; set; }

        public IList<Tag>? TagsSelected { get; set; }

        public IList<Tag>? Tags { get; set; }
    }
}
