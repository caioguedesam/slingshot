using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_BaseEvent_Name", menuName = "Patada/Events/New<>", order = 1)]
    public class SCOB_BaseEvent : SCOB_Event {
        private List<EventListener> listeners = new List<EventListener>();

        public void OnEnable(){
            listeners = new List<EventListener>();
        }

        public override void Raise() {
            Debug.Log("Raised " + this.name + " " + listeners.Count);
            for (int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised();
            }
        }

        public override void RegisterListener(EventListener listener, bool raise = false) {
            listeners.Insert(0, listener);
            if(raise) {
                Raise();
            }
        }

        public override void RemoveListener(EventListener listener) {
            listeners.Remove(listener);
        }
    }
}