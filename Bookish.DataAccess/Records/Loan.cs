using System;

namespace Bookish.DataAccess.Records
{
    class Loan
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime Due { get; set; }
    }
}
