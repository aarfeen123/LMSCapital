using System.ComponentModel.DataAnnotations;

namespace LMSCapital.Models
{
    public class AddUserViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string EnrollmentNo { get; set; } = string.Empty;

    }
}
