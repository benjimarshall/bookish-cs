using System.Collections.Generic;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class CatalogueViewModel
    {
        public IEnumerable<CataloguedBook> Books { get; }
        public string Search { get; set; }

        public CatalogueViewModel(IEnumerable<CataloguedBook> books, string search)
        {
            Books = books;
            Search = search;
        }
    }
}
