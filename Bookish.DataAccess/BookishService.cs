using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Bookish.DataAccess.Records;
using Dapper;

namespace Bookish.DataAccess
{
    public class BookishService
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

        public User? GetUser(string name)
        {
            var sqlString = $"SELECT id FROM users WHERE username=@name";
            return connection.QueryFirstOrDefault<User>(sqlString, new { name });
        }

        public IEnumerable<LoanedBook> GetJoinedLoans(string username)
        {
            var userId = GetUser(username)?.Id;

            var sqlString =
                @"SELECT loans.due AS DueDate,
                       books.title AS Title,
	                   books.Authors AS Authors,
	                   bookcopies.isbn AS ISBN,
                       bookcopies.id AS CopyId
                FROM loans
                INNER JOIN bookcopies ON loans.bookid = bookcopies.id
                INNER JOIN books ON books.isbn = bookcopies.isbn
                WHERE loans.userid = @userId;";

            return connection.Query<LoanedBook>(sqlString, new { userId });
        }
    }
}
