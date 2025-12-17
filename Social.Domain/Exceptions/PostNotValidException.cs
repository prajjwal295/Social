using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Exceptions
{
    public class PostNotValidException : NotValidException
    {
        public PostNotValidException() { }

        public PostNotValidException(string message) : base(message) { }

        public PostNotValidException(string message, Exception innerEception) : base(message, innerEception) { }
    }
}
