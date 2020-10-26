using System.Diagnostics;
using System.Linq;
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

        public HomeController(ILogger<HomeController> logger, IBookishService bookishService)
        {
            this.logger = logger;
            this.bookishService = bookishService;
        }

        public IActionResult Index()
        {
            var loans = bookishService.GetUsersLoanedBooks(User.Identity.Name);

            return View(new LoansViewModel(loans));
        }

        public IActionResult Catalogue()
        {
            var books = bookishService.GetCatalogue();

            return View(new CatalogueViewModel(books));
        }

        [HttpGet]
        public IActionResult BookDetails(BookSelection selection)
        {
            var copies = bookishService.GetCopiesOfBook(selection.Isbn);
            var firstCopy = copies.First();

            var availableCopies = copies.Count(book => !book.DueDate.HasValue);

            var book = new CataloguedBook(firstCopy, copies.Count(), availableCopies);

            return View(new BookDetailsViewModel(book, copies));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
