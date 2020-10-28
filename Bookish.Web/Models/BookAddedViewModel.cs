using System.Collections.Generic;
using Bookish.DataAccess;

namespace Bookish.Web.Models
{
    public class BookAddedViewModel
    {
        public string Title { get; set; }
        public IEnumerable<AddedBook> Copies { get; set; }

        public BookAddedViewModel(string title, IEnumerable<AddedBook> copies)
        {
            Title = title;
            Copies = copies;
        }
    }
}
