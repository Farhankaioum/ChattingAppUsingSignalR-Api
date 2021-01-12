using System.ComponentModel.DataAnnotations;

namespace ChattingApp.API.Dtos
{
    public class UserForLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
