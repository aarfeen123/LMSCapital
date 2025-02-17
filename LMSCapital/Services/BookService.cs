using LMSCapital.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LMSCapital.Services
{
    public class BookService
    {
        private readonly LMSDbContext _context;

        public BookService(LMSDbContext context)
        {
            _context = context;
        }

        // Get Books
        public List<Models.Db.Book>? GetBooks()
        {
            try
            {
                var books = _context.Books.OrderBy(x => x.BookId).Where(x => x.IsDeleted == false).ToList();
                return books;
            }
            catch
            {
                return null;
            }

        }

        // Search Books
        public List<Models.Db.Book>? SearchBooks(string str)
        {
            try
            {
                var books = _context.Books.OrderBy(x => x.BookId).Where(x => (x.Title.ToLower().Contains(str.ToLower())) && x.IsDeleted == false).ToList();
                return books;
            }
            catch
            {
                return null;
            }

        }

        // Add Book
        public bool AddBook(Models.Db.Book book)
        {
            bool isDuplicateBook = _context.Books.Any(x => x.ISBN == book.ISBN && x.IsDeleted == false);
            if (isDuplicateBook == true)
            {
                return false;
            }

            var bookId = 1;
            var lastBook = _context.Books.OrderBy(x => x.BookId).LastOrDefault();
            if (lastBook != null)
            {
                bookId = (short)(lastBook.BookId + 1);

            }
            book.BookId = (short)bookId;
            book.AvailableCopies = book.TotalCopies;
            _context.Books.Add(book);
            _context.SaveChanges();
            return true;
        }

        // Book Details
        public Models.Db.Book? Details(short? bookid)
        {
            var book = _context.Books.OrderBy(x => x.BookId).Where(x => x.BookId == bookid).FirstOrDefault();
            return book;
        }

        // Update Book
        public bool Update(Models.Db.Book book)
        {
            try
            {
                var oldBook = _context.Books.OrderBy(x => x.BookId).Where(x => x.BookId == book.BookId).FirstOrDefault();
                if (oldBook != null)
                {
                    oldBook.Title = book.Title;
                    oldBook.Author = book.Author;
                    oldBook.ISBN = book.ISBN;
                    oldBook.AvailableCopies = (sbyte)(oldBook.AvailableCopies + (book.TotalCopies - oldBook.TotalCopies));
                    oldBook.TotalCopies = book.TotalCopies;
                    _context.Books.Update(oldBook);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Delete Book
        public bool Delete(short bookid)
        {
            try
            {
                var book = _context.Books.OrderBy(x => x.BookId).Where(x => x.BookId == bookid).FirstOrDefault();
                if (book != null)
                {
                    book.IsDeleted = true;
                    _context.Books.Update(book);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // Issue Book
        public bool IssueBook(short bookId, int userId)
        {
            bool isAvailable = _context.Books.Any(x => x.BookId == bookId && x.AvailableCopies > 0 && x.IsDeleted == false);
            bool alreadyIssuedToThisUser = _context.IssuedBooks.Any(x => x.BookId == bookId && x.UserId == userId && x.IsReturned == false);
            if (isAvailable == false || alreadyIssuedToThisUser)
            {
                return false;
            }
            var issuedBook = new IssuedBook()
            {
                BookId = bookId,
                UserId = userId,
                IssueDate = DateTime.Now,
                IsReturned = false
            };
            _context.IssuedBooks.Add(issuedBook);
            var bookStatus = UpdateAvailableCopies(bookId);
            if (bookStatus)
            {
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        // Update Available Book Copies
        public bool UpdateAvailableCopies(short bookId)
        {
            try
            {
                var book = _context.Books.OrderBy(x => x.BookId).Where(x => x.BookId == bookId).FirstOrDefault();
                if (book != null)
                {
                    book.AvailableCopies = (sbyte)(book.AvailableCopies - 1);
                    _context.Books.Update(book);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // Get Issued Books
        public List<Models.Db.IssuedBook>? GetIssuedBooks()
        {
            try
            {
                var issuedBooks = _context.IssuedBooks.OrderByDescending(x=>x.IssueDate).Include(b => b.Book).Include(u => u.User).ToList();
                return issuedBooks;
            }
            catch
            {
                return null;
            }
        }


        // Return Book
        public bool ReturnBook(int issueId)
        {
            try
            {
                var issuedBook = _context.IssuedBooks.OrderBy(x => x.Id).Where(x => x.Id == issueId).FirstOrDefault();
                if (issuedBook != null)
                {
                    issuedBook.IsReturned = true;
                    issuedBook.ReturnDate = DateTime.Now;
                    _context.IssuedBooks.Update(issuedBook);
                    var book = _context.Books.OrderBy(x => x.BookId).Where(x => x.BookId == issuedBook.BookId).FirstOrDefault();
                    if (book != null)
                    {
                        book.AvailableCopies = (sbyte)(book.AvailableCopies + 1);
                        _context.Books.Update(book);
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        // Issued Books Count
        public int GetIssuedBooksCount()
        {
            var count = _context.IssuedBooks.Where(x => x.IsReturned == false).Count();
            return count;
        }

    }
}
