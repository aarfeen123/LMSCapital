using LMSCapital.Services;
using Microsoft.AspNetCore.Mvc;

namespace LMSCapital.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userSvc;

        public UserController(LMSDbContext context)
        {
            _userSvc = new UserService(context);
        }

        // Users List
        public IActionResult Index(int? userId, string? name, string? message)
        {
            TempData["Message"] = message;
            TempData["UserId"] = userId;
            TempData["Name"] = name;
            var users = _userSvc.GetUsers();
            return View(users);
        }


        // Add User - Form
        public IActionResult Add(string? message)
        {
            TempData["Message"] = message;
            return View();
        }
        // Add User - - POST
        [HttpPost]
        public IActionResult Add(Models.Db.User user)
        {
            if (ModelState.IsValid)
            {
                if (_userSvc.AddUser(user))
                {
                    return RedirectToAction("Index", new { user.UserId, user.Name, message = "User Registered Successfully !" });
                }
            }
            return RedirectToAction("Add", new { message = "User already exists or Invalid Credentials !" });
        }

    }
}
