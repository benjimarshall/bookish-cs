namespace Bookish.DataAccess.Records
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }

        public Book() { }

        public Book(Book book)
        {
            Title = book.Title;
            Authors = book.Authors;
            Isbn = book.Isbn;
        }

        public Book(string title, string authors, string isbn)
        {
            Title = title;
            Authors = authors;
            Isbn = isbn;
        }

        public string Summary => $"{Title} by {Authors}; ISBN: {Isbn}";
    }
}
