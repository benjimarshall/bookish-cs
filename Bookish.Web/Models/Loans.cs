using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookish.DataAccess;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class Loans
    {
        private BookishService bookishService = new BookishService();
        public IEnumerable<JoinedLoan> JoinedLoans { get; }

        public Loans(string username)
        {
            JoinedLoans = bookishService.GetJoinedLoans(username);
        }
    }
}
