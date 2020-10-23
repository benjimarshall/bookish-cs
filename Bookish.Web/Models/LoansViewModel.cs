using System.Collections.Generic;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class LoansViewModel
    {
        public IEnumerable<LoanedBook> LoanedBooks { get; }

        public LoansViewModel(IEnumerable<LoanedBook> loanedBooks)
        {
            LoanedBooks = loanedBooks;
        }
    }
}
