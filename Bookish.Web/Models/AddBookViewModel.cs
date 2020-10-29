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
        public string Message => InvalidIsbn ? "ISBN is already in use" : "";

        public AddBookViewModel(string title, string authors, string isbn, int copies, bool invalidIsbn)
        {
            Title = title;
            Authors = authors;
            Isbn = isbn;
            Copies = copies >= 1 ? copies : 1;
            InvalidIsbn = invalidIsbn;
        }
    }
}
