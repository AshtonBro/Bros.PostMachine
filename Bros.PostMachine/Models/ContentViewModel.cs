using System;
using System.ComponentModel.DataAnnotations;

namespace Bros.PostMachine.Models
{
    public class ContentViewModel
    {
        [Range(1, 8000)]
        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Login isn't be empty")]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
