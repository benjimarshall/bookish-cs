namespace Bookish.DataAccess
{
    public class NewBook
    {
        public int CopyId { get; }
        public string Barcode { get; }

        public NewBook(int copyId, string barcode)
        {
            CopyId = copyId;
            Barcode = barcode;
        }
    }
}
