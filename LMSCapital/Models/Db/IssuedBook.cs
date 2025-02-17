using System.ComponentModel.DataAnnotations;

namespace LMSCapital.Models.Db
{
    public class IssuedBook
    {
        [Key]
        public int Id { get; set; }

        [Required] // Foreign Key (FK) for Book
        public short BookId { get; set; }
        public Book? Book { get; set; }

        [Required] // Foreign Key (FK) for User
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public bool IsReturned { get; set; }



        
        

    }
}
