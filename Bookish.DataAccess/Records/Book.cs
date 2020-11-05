namespace Bookish.DataAccess.Records
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public int BookId { get; set; }

        public Book() { }

        public Book(int bookId, string title, string authors, string isbn)
        {
            Title = title;
            Authors = authors;
            Isbn = isbn;
            BookId = bookId;
        }

        public string Summary => $"{Title} by {Authors}; ISBN: {Isbn}";
    }
}
