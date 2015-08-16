using System;

namespace Assets.Code {
    public class BroadcastException : Exception {
        public BroadcastException(string msg) : base(msg) { }
    }
}