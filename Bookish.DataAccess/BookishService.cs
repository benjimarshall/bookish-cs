using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bookish.DataAccess.Records;
using Dapper;

namespace Bookish.DataAccess
{
    public interface IBookishService
    {
        CataloguedBook? GetBook(string isbn);
        IEnumerable<Book> GetBooks();
        IEnumerable<LoanedBook> GetUsersLoanedBooks(string userId);
        IEnumerable<CataloguedBook> GetCatalogue(string searchTerm);
        IEnumerable<LoanedBook> GetCopiesOfBook(string isbn);
    }

    public class BookishService : IBookishService
    {
        private readonly IDbConnection connection;

        public BookishService(string connectionString)
        {
            connection = new SqlConnection(connectionString);
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

        public IEnumerable<LoanedBook> GetUsersLoanedBooks(string userId)
        {
            var sqlString =
                @"SELECT books.title AS Title,
                         books.Authors AS Authors,
                         bookcopies.isbn AS ISBN,
                         bookcopies.id AS CopyId,
                         loans.due AS DueDate,
                         AspNetUsers.username AS Username
                FROM loans
                INNER JOIN bookcopies ON loans.bookid = bookcopies.id
                INNER JOIN books ON books.isbn = bookcopies.isbn
                INNER JOIN AspNetUsers ON loans.userid = AspNetUsers.id
                WHERE loans.userid = @userId;";

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
                         AspNetUsers.username AS Username
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
                  GROUP BY books.isbn, books.title, books.authors;";

            return connection.Query<CataloguedBook>(sqlString, new { searchTerm = $"%{searchTerm ?? ""}%" });
        }
    }
}
