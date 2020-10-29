using System.Diagnostics;
using System.Security.Claims;
using Bookish.DataAccess;
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

        public IActionResult AddBookForm(AddBookFormViewModel model) => View(model);

        [HttpPost]
        public IActionResult BookAdded(AddBookFormViewModel model)
        {
            if (model.Copies < 1)
            {
                model.Message = "At least one book must be added";
                return RedirectToAction("AddBookForm", model);
            }

            if (bookishService.IsbnIsUsed(model.Isbn))
            {
                model.Message = "ISBN is already in use";
                model.InvalidIsbn = true;
                return RedirectToAction("AddBookForm", model);
            }

            var copyIds = barcodeService.AddBook(model, model.Copies);
            return View(new BookAddedViewModel(model.Title, copyIds));
        }
    }
}
