using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace LMSCapital.Models.Db
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string EnrollmentNo { get; set; } = string.Empty;


        // (One-to-Many Relationship)
        public List<IssuedBook> IssuedBooks { get; set; } = [];

    }
}
