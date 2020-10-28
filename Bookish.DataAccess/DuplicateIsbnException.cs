using System;

namespace Bookish.DataAccess
{
    public class DuplicateIsbnException : Exception
    {
        public DuplicateIsbnException(string message) : base(message) { }
    }
}
