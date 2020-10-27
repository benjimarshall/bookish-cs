using System.Collections.Generic;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class CatalogueViewModel
    {
        public IEnumerable<CataloguedBook> Books { get; }
        public SearchParameters SearchParameters { get; set; }

        public CatalogueViewModel(IEnumerable<CataloguedBook> books, SearchParameters searchParameters)
        {
            Books = books;
            SearchParameters = searchParameters;
        }
    }
}
