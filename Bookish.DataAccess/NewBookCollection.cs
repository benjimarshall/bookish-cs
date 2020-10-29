using System.Collections.Generic;

namespace Bookish.DataAccess
{
    public class NewBookCollection
    {
        public IEnumerable<NewBook> NewBooks { get; }
        public string? Title { get; }

        public NewBookCollection(IEnumerable<NewBook> newBooks, string title)
        {
            NewBooks = newBooks;
            Title = title;
        }
    }
}
