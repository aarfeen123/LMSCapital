namespace LMSCapital.Services
{
    public class UserService
    {
        private readonly LMSDbContext _context;

        public UserService(LMSDbContext context)
        {
            _context = context;
        }

        // Get Users List
        public List<Models.Db.User> GetUsers()
        {
            var users = _context.Users.OrderBy(x => x.UserId).ToList();
            return users;
        }


        // Add User
        public bool AddUser(Models.Db.User user)
        {
            bool isDuplicateUser = _context.Users.Any(x => x.UserId == user.UserId || x.Email==user.Email || x.EnrollmentNo==user.EnrollmentNo);
            if (isDuplicateUser == true)
            {
                return false;
            }

            var userId = 1;
            var lastUser = _context.Users.OrderBy(x => x.UserId).LastOrDefault();
            if (lastUser != null)
            {
                userId = (lastUser.UserId + 1);
            }
            user.UserId = userId;
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }
    }
}
