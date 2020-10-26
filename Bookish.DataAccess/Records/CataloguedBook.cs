
namespace Bookish.DataAccess.Records
{
    public class CataloguedBook
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Isbn { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
    }
}
