using System;
using Bookish.DataAccess;

namespace Bookish.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bookishService = new BookishService();

            var books = bookishService.GetBooks();

            foreach (var book in books)
            {
                Console.WriteLine(book.Summary);
            }
        }
    }
}
