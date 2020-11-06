namespace Bookish.DataAccess.Records
{
    public class EditedBook : Book
    {
        public int MoreCopies { get; set; }

        public EditedBook() { }

        public EditedBook(int bookId, string title, string authors, string isbn, int moreCopies)
            : base(bookId, title, authors, isbn)
        {
            MoreCopies = moreCopies;
        }
    }
}
