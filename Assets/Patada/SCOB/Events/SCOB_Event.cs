using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    public abstract class SCOB_Event : ScriptableObject {
        [Multiline]
        [SerializeField] private string description = "";

        public virtual void Raise() {
            throw new UnityException("Method not implemented");
        }

        public virtual void RegisterListener(EventListener listener, bool raise = false) {
            throw new UnityException("Method not implemented");
        }

        public virtual void RemoveListener(EventListener listener) {
            throw new UnityException("Method not implemented");
        }
    }
}