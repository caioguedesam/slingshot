using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Animation.Extensions {
    [RequireComponent(typeof(Animator))]
    public class AnimatorExtension : MonoBehaviour {
        private Animator anim = null;
        [SerializeField] private List<UnityEvent> animationEvents = null;

        public void Awake() {
            anim = GetComponent<Animator>();
        }

        public void SetBoolOn(string name) {
            anim.SetBool(name,true);
        }

        public void SetBoolOff(string name) {
            anim.SetBool(name,false);
        }

        public void ToggleBool(string name) {
            anim.SetBool(name,!anim.GetBool(name));
        }

        public void PlayAnimationEventByID(int eventID) {
            animationEvents[eventID].Invoke();
        }
    }
}

