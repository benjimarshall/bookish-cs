using System.Collections.Generic;
using System.Diagnostics;
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
            if (!copies.Any())
            {
                return StatusCode(404);
            }

            return View(new BookDetailsViewModel(copies));
        }

        public IActionResult Error()
        {
            return View("UnknownError");
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

            bookishService.AddBook(new Book(title, authors, isbn), copies);
            return RedirectToAction("BookAdded", new { isbn });
        }

        [Route("BookAdded/{isbn}")]
        public IActionResult BookAdded(string isbn)
        {
            var newBooks = barcodeService.GetNewBooks(isbn);

            if (newBooks.Title == null)
            {
                return StatusCode(404);
            }

            return View(new BookAddedViewModel(newBooks.Title, newBooks.NewBooks));
        }

        [Route("StatusCode/{code}")]
        public new IActionResult StatusCode(int code)
        {
            Response.StatusCode = code;

            return code switch
            {
                404 => View("PageNotFound"),
                _ => View("UnknownError")
            };
        }
    }
}
