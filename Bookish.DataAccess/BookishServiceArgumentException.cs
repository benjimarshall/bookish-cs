using System;

namespace Bookish.DataAccess
{
    public class BookishServiceArgumentException : Exception
    {
        public BookishServiceArgumentException(string message) : base(message) { }
    }
}
