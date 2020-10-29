using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class AddBookViewModel
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Isbn { get; set; }
        public bool InvalidIsbn { get; set; }
        public int Copies { get; set; }
        public string Message { get; set; }

        public AddBookViewModel(string title, string authors, string isbn, int copies, bool invalidIsbn)
        {
            Title = title;
            Authors = authors;
            Isbn = isbn;
            Copies = copies >= 1 ? copies : 1;

            InvalidIsbn = invalidIsbn;
            Message = invalidIsbn ? "ISBN is already in use" :
                copies < 1 ? "At least one book must be added" : "";
        }
    }
}
