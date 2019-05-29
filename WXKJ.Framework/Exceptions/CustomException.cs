using System;

namespace WXKJ.Framework.Exceptions
{
    public class CustomException : ApplicationException
    {
        public CustomException() { }
        public CustomException(string msg) : base(msg)
        {

        }

        public CustomException(string message, Exception inner) : base(message, inner)
        {

        }
        public CustomException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {

        }
    }
}
