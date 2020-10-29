using System.Collections.Generic;
using Bookish.DataAccess;

namespace Bookish.Web.Models
{
    public class BookAddedViewModel
    {
        public string Title { get; set; }
        public IEnumerable<NewBook> Copies { get; set; }

        public BookAddedViewModel(string title, IEnumerable<NewBook> copies)
        {
            Title = title;
            Copies = copies;
        }
    }
}
