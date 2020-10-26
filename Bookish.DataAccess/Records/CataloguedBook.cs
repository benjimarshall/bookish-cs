
namespace Bookish.DataAccess.Records
{
    public class CataloguedBook : Book
    {
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public CataloguedBook() { }

        public CataloguedBook(LoanedBook loanedBook, int totalCopies, int availableCopies)
        {
            Title = loanedBook.Title;
            Authors = loanedBook.Authors;
            Isbn = loanedBook.Isbn;
            TotalCopies = totalCopies;
            AvailableCopies = availableCopies;
        }
    }
}
