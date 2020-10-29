using System.Collections.Generic;
using Bookish.DataAccess;

namespace Bookish.Web.Models
{
    public class BookAddedViewModel
    {
        public string Title { get; }
        public IEnumerable<NewBook> Copies { get; }

        public BookAddedViewModel(string title, IEnumerable<NewBook> copies)
        {
            Title = title;
            Copies = copies;
        }
    }
}
