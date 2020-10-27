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
        User? GetUser(string name);
        IEnumerable<LoanedBook> GetUsersLoanedBooks(string username);
        IEnumerable<CataloguedBook> GetCatalogue();
        IEnumerable<CataloguedBook> SearchCatalogue(string searchTerm, CatalogueFilterCategory category);
        IEnumerable<LoanedBook> GetCopiesOfBook(string isbn);
    }

    public class BookishService : IBookishService
    {
        private readonly IDbConnection connection;

        public BookishService()
        {
            connection = new SqlConnection("Server = localhost; Database = Bookish; Trusted_Connection = True;");
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

        public User? GetUser(string name)
        {
            var sqlString = $"SELECT id FROM users WHERE username=@name";
            return connection.QueryFirstOrDefault<User>(sqlString, new { name });
        }

        public IEnumerable<LoanedBook> GetUsersLoanedBooks(string username)
        {
            var userId = GetUser(username)?.Id;

            var sqlString =
                @"SELECT books.title AS Title,
                         books.Authors AS Authors,
                         bookcopies.isbn AS ISBN,
                         bookcopies.id AS CopyId,
                         loans.due AS DueDate,
                         users.username AS Username
                FROM loans
                INNER JOIN bookcopies ON loans.bookid = bookcopies.id
                INNER JOIN books ON books.isbn = bookcopies.isbn
                INNER JOIN users ON loans.userid = users.id
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
                         users.username AS Username
                FROM books
                INNER JOIN bookcopies ON books.isbn = bookcopies.isbn
                LEFT JOIN loans ON loans.bookid = bookcopies.id
                LEFT JOIN users ON loans.userid = users.id
                WHERE books.isbn = @isbn;";

            return connection.Query<LoanedBook>(sqlString, new { isbn });
        }

        public IEnumerable<CataloguedBook> GetCatalogue()
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
                GROUP BY books.isbn, books.title, books.authors;";

            return connection.Query<CataloguedBook>(sqlString);
        }

        public IEnumerable<CataloguedBook> SearchCatalogue(string? searchTerm, CatalogueFilterCategory category)
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
                  WHERE books." + category +
                 @" LIKE CONCAT('%', @searchTerm, '%')
                  GROUP BY books.isbn, books.title, books.authors;";

            return connection.Query<CataloguedBook>(sqlString, new { searchTerm = searchTerm ?? "" });
        }
    }
}
