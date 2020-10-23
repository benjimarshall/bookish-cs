using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Bookish.DataAccess.Records;
using Dapper;


namespace Bookish.DataAccess
{
    public class BookishDb
    {
        private readonly IDbConnection db;

        public BookishDb()
        {
            db = new SqlConnection(@"Server = localhost; Database = Bookish; Trusted_Connection = True;");
        }

        public IEnumerable<Book> GetBooks()
        {
            var sqlString = "SELECT TOP 100 [isbn],[title],[authors] FROM [books]";

            return (List<Book>)db.Query<Book>(sqlString);
        }
    }
}
