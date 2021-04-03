using System.ComponentModel.DataAnnotations;

namespace Bros.PostMachine.Models
{
    public class BaseViewModel
    {
        [Required(ErrorMessage = "Login isn't be empty")]
        [StringLength(30, ErrorMessage = "Login can't be more than 30 characters")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "AccessToken is required")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "ApplicationId is required")]
        public string ApplicationId { get; set; }
    }
}