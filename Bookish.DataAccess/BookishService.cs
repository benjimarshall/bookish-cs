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
    }
}
