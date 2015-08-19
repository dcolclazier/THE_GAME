using System;

namespace Assets.Code.Exceptions {
    public class BroadcastException : Exception {
        public BroadcastException(string msg) : base(msg) { }
    }
}