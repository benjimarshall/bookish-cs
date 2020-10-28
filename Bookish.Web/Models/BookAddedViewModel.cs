using System.Collections.Generic;

namespace Bookish.Web.Models
{
    public class BookAddedViewModel
    {
        public BookAddedViewModel(string title, IEnumerable<int> copyIds)
        {
            Title = title;
            CopyIds = copyIds;
        }

        public string Title { get; set; }
        public IEnumerable<int> CopyIds { get; set; }
    }
}
