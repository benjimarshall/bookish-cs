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

        public HomeController(ILogger<HomeController> logger, IBookishService bookishService)
        {
            this.logger = logger;
            this.bookishService = bookishService;
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
            try
            {
                var copyIds = bookishService.AddBook(model, model.Copies);
                return View(new BookAddedViewModel(model.Title, copyIds));
            }
            catch (BookishServiceArgumentException e)
            {
                model.Message = e.Message;
                return RedirectToAction("AddBookForm", model);
            }
            catch (DuplicateIsbnException e)
            {
                model.Message = e.Message;
                model.InvalidIsbn = true;
                return RedirectToAction("AddBookForm", model);
            }
        }
    }
}
