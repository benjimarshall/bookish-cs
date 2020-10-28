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

        public IActionResult Catalogue(string search)
        {
            var books = bookishService.GetCatalogue(search);

            return View(new CatalogueViewModel(books, search));
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
    }
}
