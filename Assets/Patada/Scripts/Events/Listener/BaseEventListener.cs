using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;

public class BaseEventListener : EventListener {
    [SerializeField] protected SCOB_BaseEvent Event;
    [Space]
    [SerializeField] protected UnityEvent Response;
    [Space]
    [SerializeField] protected List<BaseGameAction> Actions;

    public override void OnEventRaised() {
        Response.Invoke();
        for(int i = Actions.Count-1 ; i >= 0 ; i--) {
            Actions[i].Do();
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
}