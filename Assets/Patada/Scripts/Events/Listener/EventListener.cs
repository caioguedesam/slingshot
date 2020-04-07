using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener : MonoBehaviour {
    [SerializeField] protected bool registerOnAwake = false;
    [SerializeField] protected bool raiseOnRegister = false;
    protected bool registered = false;

    public virtual void OnEventRaised() {
        
    }

    public virtual void Register(){
        // if(!registered) {
        //     Event.RegisterListener(this, raiseOnRegister);
        //     registered = true;
        // }
        throw new UnityException("Method not implemented");
    }

    public virtual void Unregister(){
        // if(registered) {
        //     Event.RemoveListener(this);
        //     registered = false;
        // }
        throw new UnityException("Method not implemented");
    }

    public void Awake() {
        if (registerOnAwake) {
            Register();
        }
    }

    public void OnDestroy() {
        Unregister();
    }

    public void OnEnable() {
        Register();
    }

    public void OnDisable() {
        if(!registerOnAwake) {
            Unregister();
        }
    }
}