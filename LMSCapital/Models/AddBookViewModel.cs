using System.ComponentModel.DataAnnotations;

namespace LMSCapital.Models
{
    public class AddBookViewModel
    {
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

    }
}
