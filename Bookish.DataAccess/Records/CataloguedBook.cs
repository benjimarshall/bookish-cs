
namespace Bookish.DataAccess.Records
{
    public class CataloguedBook
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Isbn { get; set; }
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
