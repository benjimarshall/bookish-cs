﻿using Bookish.DataAccess.Records;

namespace Bookish.Web.Models
{
    public class EditBookViewModel
    {
        public int BookId { get; }
        public string Title { get; }
        public string Authors { get; }
        public string Isbn { get; }
        public bool InvalidIsbn { get; }
        public int Copies { get; }
        public string ErrorMessage { get; }

        public EditBookViewModel(
            int bookId,
            string title,
            string authors,
            string isbn,
            int copies,
            bool invalidIsbn
        )
        {
            BookId = bookId;
            Title = title;
            Authors = authors;
            Isbn = isbn;
            Copies = copies;

            InvalidIsbn = invalidIsbn;
            ErrorMessage = GenerateErrorMessage(copies, invalidIsbn);
        }

        private static string GenerateErrorMessage(int copies, bool invalidIsbn)
        {
            if (invalidIsbn)
            {
                return "ISBN is already being used by another book";
            }

            if (copies < 0)
            {
                return "A negative number of books cannot be added";
            }

            return "";
        }
    }
}
