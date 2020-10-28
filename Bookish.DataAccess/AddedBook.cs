namespace Bookish.DataAccess
{
    public class AddedBook
    {
        public int CopyId { get; }
        public string Barcode { get; }

        public AddedBook(int copyId, string barcode)
        {
            CopyId = copyId;
            Barcode = barcode;
        }
    }
}
