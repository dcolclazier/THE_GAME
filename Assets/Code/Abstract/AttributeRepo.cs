using System;
using System.Collections.Generic;
using UnityEditor;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class AttributeRepo {
        private readonly Dictionary<string, object> _repository;
        public AttributeRepo() {
            _repository = new Dictionary<string, object>();
        }

        public void Register<T>(string key, T value) {
            OnRegistering(key);
            _repository.Add(key, value);
        }
        private void OnRegistering(string key) {
            if (_repository.ContainsKey(key)) throw new Exception("REPO: You are attempting to add a key that already exists: Key = " + key);
        }

        public T Get<T>(string key) {
            if (!_repository.ContainsKey(key)) return default(T);
            OnGetting(key, typeof(T));
            return (T)_repository[key];
        }

        private void OnGetting(string key, Type type) {

            
            if (_repository[key].GetType() != type)
                Debug.Log(string.Format("You tried to retrieve a value from the lexicon with " +
                                                                   "type {0}, but the type you tried to retrieve was {1}.",
                    _repository[key].GetType(), type));
        }

        public void Update<T>(string key, T value) {
            
            OnUpdating(key, typeof(T));
            _repository[key] = value;
        }

        private void OnUpdating(string key, Type type) {
            if (!_repository.ContainsKey(key)) throw new Exception("LEXICON: You are attempting to update a value that does not exist.");

            var t = _repository[key].GetType();
            if (t != type)
                throw new ArrayTypeMismatchException(string.Format("You tried to update an attribute with " +
                                                                   "type {0}, but the type you provided was {1}.",
                                                                   t, type));
        }

        public bool Contains(string currentlyselected) {
            return _repository.ContainsKey(currentlyselected);
        }
    }
}