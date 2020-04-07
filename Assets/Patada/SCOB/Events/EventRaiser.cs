using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    public class EventRaiser : MonoBehaviour {
        [SerializeField] private bool raiseOnAwake = false;
        [SerializeField] private SCOB_BaseEvent Event = null;

        private void Raise() {
            Event.Raise();
        }

        public void Awake() {
            if (raiseOnAwake) {
                Raise();
            }
        }

        public void OnEnable() {
            if (!raiseOnAwake) {
                Raise();

            }
        }
    }
}