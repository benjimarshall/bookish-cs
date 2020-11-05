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
        CataloguedBook? GetBook(int bookId);
        LoanedBook? GetBookCopy(string copyId);
        IEnumerable<Book> GetBooks();
        IEnumerable<LoanedBook> GetUsersLoanedBooks(string userId);
        IEnumerable<CataloguedBook> GetCatalogue(string? searchTerm);
        IEnumerable<LoanedBook> GetCopiesOfBook(int bookId);
        IEnumerable<LoanedBook> GetCopiesOfBookByIsbn(string isbn);
        bool IsbnIsUsed(string isbn);
        void AddBook(Book book, int numberOfCopies);
        void CheckoutBook(LoanedBook book, string userId);
        void ReturnBook(LoanedBook book);
    }

    public class BookishService : IBookishService
    {
        private readonly IDbConnection connection;
        private const int LoanPeriod = 14;

        public BookishService(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<Book> GetBooks()
        {
            var sqlString = "SELECT bookId as books.id, isbn, title, authors FROM books";

            return connection.Query<Book>(sqlString);
        }

        public CataloguedBook? GetBook(int bookId)
        {
            return GetCatalogue().FirstOrDefault(book => book.BookId  == bookId);
        }

        public LoanedBook? GetBookCopy(string copyId)
        {
            var sqlString =
                @"SELECT books.id AS BookId,
                         books.title AS Title,
                         books.Authors AS Authors,
                         books.isbn AS ISBN,
                         bookcopies.copyId AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username,
                         AspNetUsers.id AS Userid
                FROM bookcopies
                INNER JOIN books ON bookcopies.bookId = books.id
                LEFT JOIN loans ON loans.copyId = bookcopies.copyId
                LEFT JOIN AspNetUsers ON loans.userid = AspNetUsers.id
                WHERE bookcopies.copyId = @copyId;";

            return connection.Query<LoanedBook>(sqlString, new { copyId }).FirstOrDefault();
        }

        public IEnumerable<LoanedBook> GetUsersLoanedBooks(string userId)
        {
            var sqlString =
                @"SELECT books.id AS BookId,
                         books.title AS Title,
                         books.Authors AS Authors,
                         books.isbn AS ISBN,
                         bookcopies.copyId AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username,
                         AspNetUsers.id AS Userid
                FROM loans
                INNER JOIN bookcopies ON loans.copyId = bookcopies.copyId
                INNER JOIN books ON books.id = bookcopies.bookId
                INNER JOIN AspNetUsers ON loans.userId = AspNetUsers.id
                WHERE loans.userId = @userId
                ORDER BY loans.due;";

            return connection.Query<LoanedBook>(sqlString, new { userId });
        }

        public IEnumerable<LoanedBook> GetCopiesOfBook(int bookId)
        {
            var sqlString =
                @"SELECT books.Id AS BookId,
                         books.title AS Title,
                         books.Authors AS Authors,
                         books.isbn AS ISBN,
                         bookcopies.copyId AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username,
                         AspNetUsers.id AS Userid
                FROM books
                INNER JOIN bookcopies ON books.id = bookcopies.bookId
                LEFT JOIN loans ON loans.copyId = bookcopies.copyId
                LEFT JOIN AspNetUsers ON loans.userId = AspNetUsers.id
                WHERE books.id = @bookId;";

            return connection.Query<LoanedBook>(sqlString, new { bookId });
        }

        public IEnumerable<LoanedBook> GetCopiesOfBookByIsbn(string isbn)
        {
            var sqlString =
                @"SELECT books.Id AS BookId,
                         books.title AS Title,
                         books.Authors AS Authors,
                         books.isbn AS ISBN,
                         bookcopies.copyId AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username,
                         AspNetUsers.id AS Userid
                FROM books
                INNER JOIN bookcopies ON books.id = bookcopies.bookId
                LEFT JOIN loans ON loans.copyId = bookcopies.copyId
                LEFT JOIN AspNetUsers ON loans.userId = AspNetUsers.id
                WHERE books.isbn = @isbn;";

            return connection.Query<LoanedBook>(sqlString, new { isbn });
        }

        public IEnumerable<CataloguedBook> GetCatalogue(string? searchTerm = "")
        {
            var sqlString =
                @"SELECT books.id AS BookId,
                         books.title AS Title,
                         books.authors AS Authors,
                         books.isbn AS Isbn,
                         COUNT(bookcopies.copyId) TotalCopies,
                         COUNT(bookcopies.copyId) - COUNT(loans.due) AS AvailableCopies
                  FROM books
                  FULL OUTER JOIN bookcopies ON books.id = bookcopies.bookId
                  FULL OUTER JOIN loans ON loans.copyId = bookcopies.copyId
                  WHERE books.title   LIKE @searchTerm
                  OR    books.authors LIKE @searchTerm
                  GROUP BY books.isbn, books.title, books.authors, books.id
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

                  DECLARE @InsertedBookId AS int
                  SET @InsertedBookId = SCOPE_IDENTITY();

                  DECLARE @i int = 0
                  WHILE @i < @numberOfCopies
                  BEGIN
                      SET @i = @i + 1
                      INSERT INTO bookcopies(bookId)
                      VALUES (@InsertedBookId)
                  END

                  SELECT bookcopies.copyId FROM bookcopies WHERE bookcopies.bookId = @InsertedBookId;";

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
            var dueDate = DateTime.Now.AddDays(LoanPeriod);

            var sqlString =
                @"INSERT INTO loans(copyId, userId, due)
                  VALUES (@copyId, @userId, @dueDate);";

            var rowsChanged = connection.Execute(sqlString, new
            {
                copyId = book.CopyId,
                userId,
                dueDate
            });

            if (rowsChanged != 1)
            {
                Console.WriteLine($"Expected to see one row changed after user {userId} checks out copy " +
                                  $"{book.CopyId}, instead {rowsChanged} rows were changed.");
            }
        }

        public void ReturnBook(LoanedBook book)
        {
            var sqlString = @"DELETE FROM loans WHERE copyId = @copyId;";

            var rowsChanged = connection.Execute(sqlString, new { copyId = book.CopyId });

            if (rowsChanged != 1)
            {
                Console.WriteLine($"Expected to see one row changed after returning {book.CopyId}, instead " +
                                  $"{rowsChanged} rows were changed.");
            }
        }
    }
}
