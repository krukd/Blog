using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("Role_Id")]
        public List<User> Users { get; set; } = new List<User>();
    }
}
