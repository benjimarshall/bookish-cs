namespace Bookish.DataAccess.Records
{
    public class CataloguedBook : Book
    {
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public CataloguedBook() { }

        public CataloguedBook(Book loanedBook, int totalCopies, int availableCopies)
            : base(loanedBook.Title, loanedBook.Authors, loanedBook.Isbn)
        {
            TotalCopies = totalCopies;
            AvailableCopies = availableCopies;
        }
    }
}
