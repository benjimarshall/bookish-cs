using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class AddBookViewModel : Book
    {
        public AddBookViewModel(
            string title,
            string authors,
            string isbn,
            int copies,
            string message,
            bool invalidIsbn)
            : base(title, authors, isbn)
        {
            Copies = copies >= 1 ? copies : 1;
            Message = message;
            InvalidIsbn = invalidIsbn;
        }

        public bool InvalidIsbn { get; set; } = false;
        public int Copies { get; set; }
        public string Message { get; set; } = "";
    }
}
