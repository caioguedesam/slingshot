using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patada {
    public class SafeData<T> {

        [SerializeField] T _value;
        public T Value {
            get {
                return _value;
            }
        }

        protected SafeData() { }
        public SafeData(T data) {
            _value = data;
        }
    }

    [System.Serializable]
    public class SafeInt : SafeData<int> {
        public SafeInt(int value) : base(value) {

        }
    }
}
