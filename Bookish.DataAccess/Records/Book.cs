namespace Bookish.DataAccess.Records
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }

        public string Summary => $"{Title} by {Authors}; ISBN: {Isbn}";
    }
}
