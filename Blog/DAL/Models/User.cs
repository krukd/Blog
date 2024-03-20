using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Blog.DAL.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int RoleId { get; set; } = 3;

        [ForeignKey("User_Id")]
        public List<Role> Roles { get; set; } = new List<Role>();



    }
}
