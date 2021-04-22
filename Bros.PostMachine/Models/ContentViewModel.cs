using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bros.PostMachine.Models
{
    public class ContentViewModel
    {
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Login isn't be empty")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
