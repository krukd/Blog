using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Content { get; set; }

        [ForeignKey("Tag_Id")]
        public ICollection<Article> Articles { get; set; }


    }
}
