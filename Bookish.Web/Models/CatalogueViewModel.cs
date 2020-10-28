using System.Collections.Generic;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class CatalogueViewModel
    {
        public IEnumerable<CataloguedBook> Books { get; }
        public string SearchTerm { get; }

        public CatalogueViewModel(IEnumerable<CataloguedBook> books, string searchTerm)
        {
            Books = books;
            SearchTerm = searchTerm;
        }
    }
}
