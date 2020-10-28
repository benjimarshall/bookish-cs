using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class AddBookFormViewModel : Book
    {
        public bool InvalidIsbn { get; set; } = false;
        public int Copies { get; set; } = 1;
        public string Message { get; set; } = "";
    }
}
