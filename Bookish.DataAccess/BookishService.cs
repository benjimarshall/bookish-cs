using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bookish.DataAccess.Records;
using Dapper;

namespace Bookish.DataAccess
{
    public interface IBookishService
    {
        CataloguedBook? GetBook(string isbn);
        LoanedBook? GetBookCopy(string copyId);
        IEnumerable<Book> GetBooks();
        IEnumerable<LoanedBook> GetUsersLoanedBooks(string userId);
        IEnumerable<CataloguedBook> GetCatalogue(string? searchTerm);
        IEnumerable<LoanedBook> GetCopiesOfBook(string isbn);
        bool IsbnIsUsed(string isbn);
        void AddBook(Book book, int numberOfCopies);
        void CheckoutBook(LoanedBook book, string userId);
        void ReturnBook(LoanedBook book);
    }

    public class BookishService : IBookishService
    {
        private readonly IDbConnection connection;

        public BookishService(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<Book> GetBooks()
        {
            var sqlString = "SELECT isbn, title, authors FROM books";

            return connection.Query<Book>(sqlString);
        }

        public CataloguedBook? GetBook(string isbn)
        {
            return GetCatalogue().FirstOrDefault(book => book.Isbn == isbn);
        }
        public LoanedBook? GetBookCopy(string copyId)
        {
            var sqlString =
                @"SELECT books.title AS Title,
                         books.Authors AS Authors,
                         bookcopies.isbn AS ISBN,
                         bookcopies.id AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username,
                         AspNetUsers.id AS Userid
                FROM bookcopies
                INNER JOIN books ON bookcopies.isbn = books.isbn
                LEFT JOIN loans ON loans.bookid = bookcopies.id
                LEFT JOIN AspNetUsers ON loans.userid = AspNetUsers.id
                WHERE bookcopies.id= @copyId;";

            return connection.Query<LoanedBook>(sqlString, new { copyId }).FirstOrDefault();
        }

        public IEnumerable<LoanedBook> GetUsersLoanedBooks(string userId)
        {
            var sqlString =
                @"SELECT books.title AS Title,
                         books.Authors AS Authors,
                         bookcopies.isbn AS ISBN,
                         bookcopies.id AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username,
                         AspNetUsers.id AS Userid
                FROM loans
                INNER JOIN bookcopies ON loans.bookid = bookcopies.id
                INNER JOIN books ON books.isbn = bookcopies.isbn
                INNER JOIN AspNetUsers ON loans.userid = AspNetUsers.id
                WHERE loans.userid = @userId
                ORDER BY loans.due;";

            return connection.Query<LoanedBook>(sqlString, new { userId });
        }

        public IEnumerable<LoanedBook> GetCopiesOfBook(string isbn)
        {
            var sqlString =
                @"SELECT books.title AS Title,
                         books.Authors AS Authors,
                         bookcopies.isbn AS ISBN,
                         bookcopies.id AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username,
                         AspNetUsers.id AS Userid
                FROM books
                INNER JOIN bookcopies ON books.isbn = bookcopies.isbn
                LEFT JOIN loans ON loans.bookid = bookcopies.id
                LEFT JOIN AspNetUsers ON loans.userid = AspNetUsers.id
                WHERE books.isbn = @isbn;";

            return connection.Query<LoanedBook>(sqlString, new { isbn });
        }

        public IEnumerable<CataloguedBook> GetCatalogue(string? searchTerm = "")
        {
            var sqlString =
                @"SELECT books.title AS Title,
                         books.authors AS Authors,
                         books.isbn AS Isbn,
                         COUNT(bookcopies.id) TotalCopies,
                         COUNT(bookcopies.id) - COUNT(loans.due) AS AvailableCopies
                  FROM books
                  FULL OUTER JOIN bookcopies ON books.isbn = bookcopies.isbn
                  FULL OUTER JOIN loans ON loans.bookid = bookcopies.id
                  WHERE books.title   LIKE @searchTerm
                  OR    books.authors LIKE @searchTerm
                  GROUP BY books.isbn, books.title, books.authors
                  ORDER BY books.title;";

            return connection.Query<CataloguedBook>(sqlString, new { searchTerm = $"%{searchTerm ?? ""}%" });
        }

        public bool IsbnIsUsed(string isbn)
        {
            var sqlString = "SELECT books.isbn FROM books WHERE books.isbn = @isbn";

            var result = connection.Query<string>(sqlString, new {isbn});
            return result.FirstOrDefault() == isbn;
        }

        public void AddBook(Book book, int numberOfCopies)
        {
            var sqlString =
                @"INSERT INTO books(isbn, title, authors)
                  VALUES (@isbn, @title, @authors);

                  DECLARE @i int = 0
                  WHILE @i < @numberOfCopies
                  BEGIN
                      SET @i = @i + 1
                      INSERT INTO bookcopies(isbn)
                      VALUES (@isbn)
                  END

                  SELECT bookcopies.id FROM bookcopies WHERE bookcopies.isbn = @isbn;";

            connection.Execute(sqlString, new
            {
                isbn = book.Isbn,
                title = book.Title,
                authors = book.Authors,
                numberOfCopies
            });
        }

        public void CheckoutBook(LoanedBook book, string userId)
        {
            var dueDate = DateTime.Now.AddDays(14);

            var sqlString =
                @"INSERT INTO loans(bookid, userid, due)
                  VALUES (@bookId, @userId, @dueDate);";

            connection.Execute(sqlString, new
            {
                bookId = book.CopyId,
                userId,
                dueDate
            });
        }

        public void ReturnBook(LoanedBook book)
        {
            var sqlString = @"DELETE FROM loans WHERE bookId = @copyId;";

            connection.Execute(sqlString, new { copyId = book.CopyId });
        }
    }
}
