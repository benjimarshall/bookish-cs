using System;
using System.Collections.Generic;
using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class BookDetailsViewModel
    {
        public IEnumerable<LoanedBook> BookCopies { get; }
        public CataloguedBook Book { get; }

        public BookDetailsViewModel(CataloguedBook book, IEnumerable<LoanedBook> bookCopies)
        {
            Book = book;
            BookCopies = bookCopies;
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
