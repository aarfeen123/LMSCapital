using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSCapital.Models.Db
{
    public class Book
    {
        [Key]
        public short Id { get; set; }

        [Required]
        public short BookId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public ulong ISBN { get; set; }

        [Required]
        public sbyte TotalCopies { get; set; }

        [Required]
        public sbyte AvailableCopies { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        // (One-to-Many Relationship)
        public List<IssuedBook> IssuedBooks { get; set; } = [];

    }
}
