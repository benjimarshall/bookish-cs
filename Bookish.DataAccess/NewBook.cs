using Bookish.DataAccess.Records;

namespace Bookish.DataAccess
{
    public class NewBook : Book
    {
        public int CopyId { get; }
        public string Barcode { get; }

        public NewBook(Book book, int copyId, string barcode)
            : base(book.BookId, book.Title, book.Authors, book.Isbn)
        {
            CopyId = copyId;
            Barcode = barcode;
        }
    }
}
