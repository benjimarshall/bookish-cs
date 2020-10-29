using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookish.Web.Models;

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

        public IActionResult Index()
        {
            var loans = bookishService.GetUsersLoanedBooks(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(new LoansViewModel(loans));
        }

        public IActionResult Catalogue(string searchTerm)
        {
            var books = bookishService.GetCatalogue(searchTerm);

            return View(new CatalogueViewModel(books, searchTerm));
        }

        [Route("BookDetails/{isbn}")]
        public IActionResult BookDetails(string isbn)
        {
            var copies = bookishService.GetCopiesOfBook(isbn);

            return View(new BookDetailsViewModel(copies));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddBook(
            string title,
            string authors,
            string isbn,
            int copies,
            bool invalidIsbn)
        {
            return View(new AddBookViewModel(title, authors, isbn, copies, invalidIsbn));
        }

        [HttpPost]
        public IActionResult AddBook(string title, string authors, string isbn, int copies)
        {
            if (copies < 1)
            {
                return RedirectToAction("AddBook", new AddBookViewModel(title, authors, isbn, copies, false));
            }

            if (bookishService.IsbnIsUsed(isbn))
            {
                return RedirectToAction("AddBook", new AddBookViewModel(title, authors, isbn, copies, true));
            }

            bookishService.AddBook(new Book(title, authors, isbn), copies);
            return RedirectToAction("BookAdded", new { isbn });
        }

        [Route("BookAdded/{isbn}")]
        public IActionResult BookAdded(string isbn)
        {
            var title = bookishService.GetBook(isbn)?.Title;
            var newBooks = barcodeService.GetNewBooks(isbn);

            return View(new BookAddedViewModel(title, newBooks));
        }
    }
}
