using System;
using System.Collections.Generic;
using System.Linq;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class CatalogueViewModel
    {
        public IEnumerable<CataloguedBook> Books { get; }
        public string SearchTerm { get; }
        public int PageNumber { get; }
        public int PageCount { get; }
        private const int PageSize = 10;

        public CatalogueViewModel(IEnumerable<CataloguedBook> books, string searchTerm, int pageNumber)
        {
            SearchTerm = searchTerm;
            PageCount = (int)Math.Ceiling((double)books.Count() / PageSize);
            PageNumber = pageNumber >= 1 ? pageNumber : 1;
            PageNumber = PageNumber > PageCount ? PageCount : PageNumber;

            Books = books.Skip((PageNumber - 1) * PageSize).Take(PageSize);
        }

        public string PreviousButtonStatus => PageNumber == 1 ? "disabled" : "";

        public string NextButtonStatus => PageNumber == PageCount ? "disabled" : "";

        public string LinkNumberStatus(int linkNumber)
        {
            return linkNumber == PageNumber ? "active" : "";
        }
    }
}
