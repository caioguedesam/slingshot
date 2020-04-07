using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [System.Serializable]
    public abstract class SCOB_TEvent<T> : SCOB_Event {
        private List<IEventListener<T>> listeners = new List<IEventListener<T>>();

        public override void Raise() {
            Debug.Log("Raised " + this.name + " event");
        }

        public virtual void Raise(T data) {
            // Debug.Log("Raised " + this.name + " event {" + data.ToString() + "}");
            for (int i = listeners.Count - 1; i >= 0; i--) {
                // Debug.Log(listeners[i].ToString());
                listeners[i].OnEventRaised(data);
            }
        }

        public virtual void RegisterListener(IEventListener<T> listener, bool raise = false) {
            listeners.Insert(0, listener);
            if(raise) {
                Raise();
            }
        }

        public virtual void RemoveListener(IEventListener<T> listener) {
            listeners.Remove(listener);
        }
    }
}