namespace Blog.DAL.Models
{
    public class AddArticleViewModel
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public IList<Tag>? Tags { get; set; }
    }
}
