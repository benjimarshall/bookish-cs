using System.Security.Claims;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookish.Web.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Bookish.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IBookishService bookishService;
        private readonly IBarcodeService barcodeService;

        public HomeController(
            ILogger<HomeController> logger,
            IBookishService bookishService,
            IBarcodeService barcodeService)
        {
            this.logger = logger;
            this.bookishService = bookishService;
            this.barcodeService = barcodeService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public IActionResult Index()
        {
            var loans = bookishService.GetUsersLoanedBooks(UserId);

            return View(new LoansViewModel(loans));
        }

        public IActionResult Catalogue(string searchTerm, int pageNumber = 1)
        {
            var books = bookishService.GetCatalogue(searchTerm);

            return View(new CatalogueViewModel(books, searchTerm, pageNumber));
        }

        [Route("BookDetails/{bookId}")]
        public IActionResult BookDetails(int bookId, bool bookJustEdited = false)
        {
            var copies = bookishService.GetCopiesOfBook(bookId);
            if (!copies.Any())
            {
                return StatusCode(404);
            }

            return View(new BookDetailsViewModel(copies, bookJustEdited));
        }

        public IActionResult AddBook(
            string title = "",
            string authors = "",
            string isbn = "",
            int copies = 1)
        {
            return View(new AddBookViewModel(title, authors, isbn, copies, bookishService.IsbnIsUsed(isbn)));
        }

        [HttpPost]
        [ActionName("AddBook")]
        public IActionResult AddBookPost(string title, string authors, string isbn, int copies)
        {
            if (copies < 1 || bookishService.IsbnIsUsed(isbn))
            {
                return RedirectToAction("AddBook", new { title, authors, isbn, copies });
            }

            bookishService.AddBook(new Book(0, title, authors, isbn), copies);
            return RedirectToAction("BookAdded", new { isbn });
        }

        [Route("BookAdded/{isbn}")]
        public IActionResult BookAdded(string isbn)
        {
            var newBooks = barcodeService.GetNewBooks(isbn);

            if (!newBooks.Any())
            {
                return StatusCode(404);
            }

            return View(new BookAddedViewModel(newBooks));
        }

        [Route("EditBook")]
        [Route("EditBook/{bookId}")]
        public IActionResult EditBook(
            int? bookId,
            string? title,
            string? authors,
            string? isbn,
            int? copies)
        {
            if (bookId == null) return StatusCode(404);

            var book = bookishService.GetBook(bookId.Value);
            if (book == null) return StatusCode(404);

            return View(new EditBookViewModel(
                bookId.Value,
                title ?? book.Title,
                authors ?? book.Authors,
                isbn ?? book.Isbn,
                copies ?? 0,
                isbn != null && isbn != book.Isbn && bookishService.IsbnIsUsed(isbn)
                )
            );
        }

        [HttpPost]
        [Route("EditBook/{bookId}")]
        public IActionResult EditBookPost(int bookId, string title, string authors, string isbn, int copies)
        {
            var currentBook = bookishService.GetBook(bookId);
            if (currentBook == null || copies < 0
                                    || isbn != currentBook.Isbn && bookishService.IsbnIsUsed(isbn))
            {
                return RedirectToAction("EditBook", new { bookId, title, authors, isbn, copies });
            }

            bookishService.EditBook(bookId, title, authors, isbn, copies);
            return RedirectToAction("BookDetails", new { bookId, bookJustEdited = true });
        }

        public IActionResult CheckoutBook()
        {
            return StatusCode(404);
        }

        [HttpPost]
        [ActionName("CheckoutBook")]
        public IActionResult CheckoutBook(int copyId)
        {
            var bookCopy = bookishService.GetBookCopy(copyId);

            if (bookCopy == null)
            {
                return StatusCode(404);
            }

            if (!bookCopy.Available)
            {
                return StatusCode(403);
            }

            bookishService.CheckoutBook(copyId, UserId);

            return RedirectToAction("BookCheckedOut", new { copyId });
        }

        public IActionResult ReturnBook()
        {
            return StatusCode(404);
        }

        [HttpPost]
        [ActionName("ReturnBook")]
        public IActionResult ReturnBook(int copyId)
        {
            var bookCopy = bookishService.GetBookCopy(copyId);

            if (bookCopy == null)
            {
                return StatusCode(404);
            }

            if (bookCopy.UserId != UserId || bookCopy.Available)
            {
                return StatusCode(403);
            }

            bookishService.ReturnBook(copyId);

            return RedirectToAction("BookReturned", new { copyId });
        }

        [Route("BookCheckedOut/{copyId}")]
        public IActionResult BookCheckedOut(int copyId)
        {
            var bookCopy = bookishService.GetBookCopy(copyId);

            if (bookCopy == null)
            {
                return StatusCode(404);
            }

            if (bookCopy.UserId != UserId)
            {
                return StatusCode(403);
            }

            return View(bookCopy);
        }

        [Route("BookReturned/{copyId}")]
        public IActionResult BookReturned(int copyId)
        {
            var bookCopy = bookishService.GetBookCopy(copyId);

            if (bookCopy == null || !bookCopy.Available)
            {
                return StatusCode(404);
            }

            return View(bookCopy);
        }

        public IActionResult Error()
        {
            return View("UnknownError");
        }

        [Route("StatusCode/{code}")]
        public new IActionResult StatusCode(int code)
        {
            Response.StatusCode = code;

            return code switch
            {
                403 => View("Forbidden"),
                404 => View("PageNotFound"),
                _ => View("UnknownError")
            };
        }
    }
}
