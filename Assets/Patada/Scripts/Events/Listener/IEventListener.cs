using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventListener<T> {
    void OnEventRaised(T item);
}

public interface IEventListener<T1, T2> {
    void OnEventRaised(T1 first, T2 second);
}
