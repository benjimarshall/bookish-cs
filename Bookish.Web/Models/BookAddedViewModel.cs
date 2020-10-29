using System.Collections.Generic;
using System.Linq;
using Bookish.DataAccess;

namespace Bookish.Web.Models
{
    public class BookAddedViewModel
    {
        public string Title { get; }
        public IEnumerable<NewBook> Copies { get; }

        public BookAddedViewModel(IEnumerable<NewBook> copies)
        {
            Title = copies.First().Title;
            Copies = copies;
        }
    }
}
