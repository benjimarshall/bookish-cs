using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataAccess.Records
{
    public class JoinedLoan
    {
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Isbn { get; set; }
        public int CopyId { get; set; }
    }
}
