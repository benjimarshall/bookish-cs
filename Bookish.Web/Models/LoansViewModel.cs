using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class LoansViewModel
    {
        private BookishService bookishService = new BookishService();
        public IEnumerable<LoanedBook> JoinedLoans { get; }

        public LoansViewModel(IEnumerable<LoanedBook> joinedLoans)
        {
            JoinedLoans = joinedLoans;
        }
    }
}
