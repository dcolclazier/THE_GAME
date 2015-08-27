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
}