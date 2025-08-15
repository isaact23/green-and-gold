using System;

namespace GnG.Exceptions {
    public class UnhandledEventException : Exception {
        public UnhandledEventException() {}

        public UnhandledEventException(string message) : base(message) {}
        public UnhandledEventException(string message, Exception inner) : base(message, inner) {}
    }
}
