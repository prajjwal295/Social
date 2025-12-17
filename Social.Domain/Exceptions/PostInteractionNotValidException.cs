using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Exceptions
{
    public class PostInteractionNotValidException : NotValidException
    {
        public PostInteractionNotValidException() { }

        public PostInteractionNotValidException(string message) : base(message) { }

        public PostInteractionNotValidException(string message, Exception innerEception) : base(message, innerEception) { }
    }
}
