using System.Collections.Generic;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class CatalogueViewModel
    {
        public IEnumerable<CataloguedBook> Books { get; }
        public string SearchTerm { get; }
        public int PageNumber { get; }
        public int PageCount { get; }

        public CatalogueViewModel(IEnumerable<CataloguedBook> books, string searchTerm, int pageNumber, int pageCount)
        {
            Books = books;
            SearchTerm = searchTerm;
            PageNumber = pageNumber;
            PageCount = pageCount;
        }
    }
}
