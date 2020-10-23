using System;
using Bookish.DataAccess;

namespace Bookish.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bookishDb = new BookishDb();

            var books = bookishDb.GetBooks();

            foreach (var book in books)
            {
                Console.WriteLine(book.Summary);
            }
        }
    }
}
