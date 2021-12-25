using System;

namespace MusimilarApi.Helpers{

    public class AppException : Exception
    {
        public AppException() { }
        public AppException(string message) : base(message) { }
        public AppException(string message, Exception inner) : base(message, inner) { }

    }
}