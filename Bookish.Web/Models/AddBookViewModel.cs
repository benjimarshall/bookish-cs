using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class AddBookViewModel
    {
        public string Title { get; }
        public string Authors { get; }
        public string Isbn { get; }
        public bool InvalidIsbn { get; }
        public int Copies { get; }
        public string ErrorMessage { get; }

        public AddBookViewModel(string title, string authors, string isbn, int copies, bool invalidIsbn)
        {
            Title = title;
            Authors = authors;
            Isbn = isbn;
            Copies = copies;

            InvalidIsbn = invalidIsbn;
            ErrorMessage = GenerateErrorMessage(copies, invalidIsbn);
        }

        private static string GenerateErrorMessage(int copies, bool invalidIsbn)
        {
            if (invalidIsbn)
            {
                return "ISBN is already in use";
            }

            if (copies < 1)
            {
                return "At least one book must be added";
            }

            return "";
        }
    }
}
