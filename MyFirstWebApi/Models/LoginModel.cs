using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApi.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
