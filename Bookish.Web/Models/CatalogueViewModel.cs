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
            PageNumber = GetValidPageNumber(pageNumber, PageCount);

            Books = books.Skip((PageNumber - 1) * PageSize).Take(PageSize);
        }

        private static int GetValidPageNumber(int pageNumber, int pageCount)
        {
            if (pageNumber < 1) return 1;

            if (pageNumber > pageCount) return pageCount;

            return pageNumber;
        }

        public string PreviousButtonStatus => PageNumber == 1 ? "disabled" : "";

        public string NextButtonStatus => PageNumber == PageCount ? "disabled" : "";

        public string LinkNumberStatus(int linkNumber)
        {
            return linkNumber == PageNumber ? "active" : "";
        }
    }
}
