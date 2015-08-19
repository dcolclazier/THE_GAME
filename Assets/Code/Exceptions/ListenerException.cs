using System;

namespace Assets.Code.Exceptions {
    public class ListenerException : Exception {
        public ListenerException(string msg)
            : base(msg) { }
    }
}