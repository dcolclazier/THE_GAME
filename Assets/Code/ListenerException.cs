using System;

namespace Assets.Code {
    public class ListenerException : Exception {
        public ListenerException(string msg)
            : base(msg) { }
    }
}