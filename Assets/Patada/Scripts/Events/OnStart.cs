using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patada.Actions;
public abstract class OnStart : MonoBehaviour {
    public abstract void Start();
}

public abstract class OnStart<TType, TUnityEvent, TActions> : OnStart where TActions : GameAction<TType> where TUnityEvent : UnityEvent<TType> {
    #pragma warning disable CS0649
    [SerializeField] private TType target;
    #pragma warning restore CS0649
    [SerializeField] private TUnityEvent uActions = null;
    [SerializeField] private List<TActions> actions = null;

    public override void Start(){
        uActions.Invoke(target);
        for(int i = 0 ; i < actions.Count ; i++) {
            actions[i].Do(target);
        }
    }
}

public abstract class OnStart<TType1, TType2, TUnityEvent, TActions> : OnStart where TActions : GameAction<TType1, TType2> where TUnityEvent : UnityEvent<TType1, TType2> {
    #pragma warning disable CS0649
    [SerializeField] private TType1 target;
    [SerializeField] private TType2 target2;
    #pragma warning restore CS0649
    [SerializeField] private TUnityEvent uActions = null;
    [SerializeField] private List<TActions> actions = null;

    public override void Start(){
        for(int i = 0 ; i < actions.Count ; i++) {
            uActions.Invoke(target, target2);
            actions[i].Do(target, target2);
        }
    }
}