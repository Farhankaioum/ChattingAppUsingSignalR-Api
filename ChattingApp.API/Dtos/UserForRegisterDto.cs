using System.ComponentModel.DataAnnotations;

namespace ChattingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
