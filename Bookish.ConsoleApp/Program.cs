using System;
using Bookish.DataAccess;

namespace Bookish.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bookishService = new BookishService("Server=(localdb)\\mssqllocaldb;Database=aspnet-Bookish.Web-05F4DAB2-1D66-4A6A-85B2-2FCAC24D9C24;Trusted_Connection=True;MultipleActiveResultSets=true");

            var books = bookishService.GetBooks();

            foreach (var book in books)
            {
                Console.WriteLine(book.Summary);
            }
        }
    }
}
