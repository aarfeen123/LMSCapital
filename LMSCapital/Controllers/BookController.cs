using LMSCapital.Models;
using LMSCapital.Models.Db;
using LMSCapital.Services;
using Microsoft.AspNetCore.Mvc;

namespace LMSCapital.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookSvc;
        private readonly UserService _userSvc;
        
        public BookController(LMSDbContext context)
        {
            _bookSvc = new BookService(context);
            _userSvc = new UserService(context);
        }

        // Books List
        public IActionResult Index(short? bookId, string? title, string? message)
        {
            TempData["Message"] = message;
            TempData["BookId"] = bookId;
            TempData["Title"] = title;
            var books = _bookSvc.GetBooks();
            return View(books);
        }

        // Search Books
        [HttpGet]
        public IActionResult SearchBooks(string searchString)
        {
            var books = _bookSvc.SearchBooks(searchString);
            return View(books);
        }

        // Add Book - Form
        public IActionResult Add(string? message)
        {
            TempData["Message"] = message;
            return View();
        }
        // Add Book - - POST
        [HttpPost]
        public IActionResult Add(Models.Db.Book book)
        {
            if (ModelState.IsValid)
            {
                if (_bookSvc.AddBook(book))
                {
                    return RedirectToAction("Index", new { book.BookId, book.Title, message = "Book Added Successfully !" });
                }
            }
            return RedirectToAction("Add", new { message = "Book already exists or Invalid Credentials !" });
        }


        // View Book Details
        public IActionResult Details(short bookId)
        {
            var book = _bookSvc.Details(bookId);
            return View(book);
        }


        // Update Book - FORM
        public IActionResult Update(short bookId)
        {
            var book = _bookSvc.Details(bookId);
            return View(book);
        }


        // Update Book - POST
        [HttpPost]
        public IActionResult Update(Book book)
        {
            bool status = _bookSvc.Update(book);
            if (status && book != null)
            {
                return RedirectToAction("Index", new { book.BookId, book.Title, message = "Book Updated Successfully !" });
            }
            return RedirectToAction("Index", new { book?.BookId, book?.Title, message = "Error in Updating !" });
        }


        // Delete Book
        public IActionResult Delete(short bookId)
        {
            var book = _bookSvc.Details(bookId);
            bool status = _bookSvc.Delete(bookId);
            if (status && book != null)
            {
                return RedirectToAction("Index", new { book.BookId, book.Title, message = "Book Deleted Successfully !" });
            }
            return RedirectToAction("Index", new { book?.BookId, book?.Title, message = "Error in Deleting !" });
        }

        // Issue Book - FORM
        public IActionResult Issue(short bookId)
        {
            var book = _bookSvc.Details(bookId);
            var users = _userSvc.GetUsers();
            var bookAndUsers = new Models.BookAndUsersViewModel()
            {
                Books = book,
                Users = users
            };
            return View(bookAndUsers);       
        }


        // Issue Book - POST
        [HttpPost]
        public IActionResult Issue(BookAndUsersViewModel model, int UserId)
        {
            bool status;
            if (model?.Books!=null)
            {
                status = _bookSvc.IssueBook(model.Books.BookId, UserId);
                if (status)
                {
                    return RedirectToAction("Index", new { model.Books.BookId, model.Books.Title, message = "Book Issued Successfully !" });
                }
            }
            return RedirectToAction("Index", new { model?.Books?.BookId, model?.Books?.Title, message = "Book not Issued, Faced Problem !" });
        }


        // Issued Books List
        public IActionResult IssuedBooks(string? message)
        {
            TempData["Message"] = message;
            var books = _bookSvc.GetIssuedBooks();
            return View(books);
        }


        // Return Book
        public IActionResult Return(int issueId)
        {
            bool status = _bookSvc.ReturnBook(issueId);
            if (status)
            {
                return RedirectToAction("IssuedBooks", new { message= $"Book Returned ! (with Issue Id: {issueId})"});
            }
            return RedirectToAction("IssuedBooks", new { message = $"Book NOT Returned ! (with Issue Id: {issueId})" });
        }


    }
}
