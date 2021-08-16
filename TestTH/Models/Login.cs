
using System.ComponentModel.DataAnnotations;

namespace TestTH.Models
{
    public class Login
    {
        //[Key]
        //public int Loginid { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
