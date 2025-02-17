using LMSCapital.Models.Db;

namespace LMSCapital.Models
{
    public class BookAndUsersViewModel
    {
        public Book? Books { get; set; } = new Book();
        public List<User>? Users { get; set; } = [];

    }
}
