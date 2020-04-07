using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;
using Patada.Actions;

[System.Serializable]
public class TEventListener<TType, TEvent, TResponse, TActions> : EventListener, IEventListener<TType> where TEvent : SCOB_TEvent<TType> where TResponse : UnityEvent<TType> where TActions : GameAction<TType>{
    [SerializeField] protected TEvent Event;
    [SerializeField] protected TResponse Response;
    [SerializeField] protected List<TActions> Actions;

    public override void OnEventRaised() {
        throw new UnityException("Event raised with no data");
    }

    public virtual void OnEventRaised(TType data) {
        Response.Invoke(data);
        for(int i = Actions.Count - 1 ; i >= 0 ; i--) {
            Actions[i].Do(data);
        }
    }

    public override void Register(){
        if(!registered) {
            Event.RegisterListener(this, raiseOnRegister);
            registered = true;
        }
    }

    public override void Unregister(){
        if(registered) {
            Event.RemoveListener(this);
            registered = false;
        }
    }

    public virtual void AddResponse(TActions action){
        Actions.Add(action);
    }

    public virtual void RemoveResponse(TActions action){
        Actions.Remove(action);
    }
}