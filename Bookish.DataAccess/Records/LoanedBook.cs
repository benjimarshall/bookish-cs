#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataAccess.Records
{
    public class LoanedBook : Book
    {
        public DateTime? DueDate { get; set; }
        public int CopyId { get; set; }
        public string? Username { get; set; }
        public string UserId { get; set; }

        public bool Available => DueDate == null;
    }
}
