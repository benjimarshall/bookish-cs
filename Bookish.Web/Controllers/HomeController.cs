using System.Diagnostics;
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
            var loans = bookishService.GetUsersLoanedBooks(User.Identity.Name);

            return View(new LoansViewModel(loans));
        }

        public IActionResult Catalogue()
        {
            var books = bookishService.GetCatalogue();

            return View(new CatalogueViewModel(books));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
