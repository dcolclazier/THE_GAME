using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Code.Abstract {
    public sealed class Lexicon<T> where T : class {
        private readonly Dictionary<string, T> _lexicon = new Dictionary<string, T>();

        private void OnRegistering(string key, Type type) {
            if (_lexicon.ContainsKey(key)) throw new Exception("LEXICON: You are attempting to add a key that already exists.");
            if (typeof(T)!=type) throw new Exception(string.Format("You tried to register a value with " +
                                                                "type {0}, but the type it needs to be is a {1}.",
                                                                typeof(T) , type));
        }

        public void Register<T1>(string key, T1 value) {
            OnRegistering(key, typeof(T1));
            _lexicon.Add(key, value as T);
        }

        public bool Contains(string key) {
            return _lexicon.ContainsKey(key);
        }

        public T1 Get<T1>(string key) where T1 : class {
            OnGetting(key, typeof(T1));
            return _lexicon[key] as T1;
        }

        private void OnGetting(string key, Type type) {
            if(!_lexicon.ContainsKey(key)) throw new Exception("LEXICON: You are attempting to retrieve a value that does not exist.");
            if(_lexicon[key].GetType() != type) 
                throw new ArrayTypeMismatchException(string.Format( "You tried to retrieve a value from the lexicon with " +
                                                                    "type {0}, but the type you tried to retrieve was {1}.",
                    _lexicon[key].GetType() , type));        
        }
    }

    //public sealed class Lexicon : IEnumerable 
    //{
    //    private readonly Dictionary<string, object> _lexicon = new Dictionary<string, object>();

    //    //private void OnRegistering(string key, Type type)
    //    //{
    //    //    //if (_lexicon.ContainsKey(key)) throw new Exception("LEXICON: You are attempting to add a key that already exists.");
    //    //    //if (typeof(T) != type) throw new Exception(string.Format("You tried to register a value with " +
    //    //    //                                                      "type {0}, but the type it needs to be is a {1}.",
    //    //    //                                                      typeof(T), type));
    //    //}

    //    public void Register<T1>(string key, T1 value) where T1 : class
    //    {
    //        //OnRegistering(key, typeof(T1));
    //        _lexicon.Add(key, value);
    //    }

    //    public bool Contains(string key)
    //    {
    //        return _lexicon.ContainsKey(key);
    //    }

    //    public T1 Get<T1>(string key) where T1 : class
    //    {
    //        OnGetting(key, typeof(T1));
    //        return _lexicon[key] as T1;
    //    }

    //    private void OnGetting(string key, Type type)
    //    {
    //        if (!_lexicon.ContainsKey(key)) throw new Exception("LEXICON: You are attempting to retrieve a value that does not exist.");
    //        if (_lexicon[key].GetType() != type)
    //            throw new ArrayTypeMismatchException(string.Format("You tried to retrieve a value from the lexicon with " +
    //                                                                "type {0}, but the type you tried to retrieve was {1}.",
    //                _lexicon[key].GetType(), type));
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }

    //    private LexiconEnumerator GetEnumerator() {
    //        return new LexiconEnumerator(_lexicon);
    //    }

    //    public void Add<T>(string key, T value) {
    //        _lexicon.Add(key, value);
    //    }
    //}

    //public class LexiconEnumerator : IEnumerator {
    //    private object _current;

    //    private readonly Dictionary<string,object> _lexicon;

    //    private int _position = -1;

    //    public LexiconEnumerator(Dictionary<string, object> lexicon) {
    //        _lexicon = lexicon;
    //    }

    //    public bool MoveNext() {
    //        _position++;
    //        return (_position < _lexicon.Count);
    //    }

    //    public void Reset() {
    //        _position = -1;
    //    }

    //    object IEnumerator.Current {
    //        get { return Current; }
    //    }

    //    private object Current {
    //        get {
    //            try {
    //                return _lexicon.Keys.ElementAt(_position);
    //            }
    //            catch {
    //                throw new InvalidOperationException();
    //            }
    //        }
    //    }
    //}
}