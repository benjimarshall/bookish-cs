using System;
using System.Collections.Generic;
using System.Linq;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class BookDetailsViewModel
    {
        public IEnumerable<LoanedBook> BookCopies { get; }
        public CataloguedBook Book { get; }
        public bool BookJustEdited { get; }

        public BookDetailsViewModel(IEnumerable<LoanedBook> bookCopies, bool bookJustEdited)
        {
            var firstCopy = bookCopies.First();
            var availableCopies = bookCopies.Count(book => !book.DueDate.HasValue);

            Book = new CataloguedBook(firstCopy, bookCopies.Count(), availableCopies);
            BookCopies = bookCopies;
            BookJustEdited = bookJustEdited;
        }

        public static string GetBookCopyStatus(LoanedBook book)
        {
            if (book.DueDate == null)
            {
                return "Available";
            }

            return $"On loan to {book.Username}" +
                   $"{(book.DueDate >= DateTime.Now ? " until" : ", overdue since")} " +
                   $"{book.DueDate?.ToShortDateString()}";
        }
    }
}
