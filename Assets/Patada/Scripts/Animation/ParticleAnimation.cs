using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patada.Animation {
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAnimation : MonoBehaviour {
        private ParticleSystem pSystem = null;
        public ParticleSystem Particles {
            set{
                pSystem = value;
            }
            
            get{
                return pSystem;
            }
        }

        private Coroutine callbackHandlerCoroutine = null;
        private bool playing = false;

        private float duration = 0f;

        void Awake() {
            var pSystemList = GetComponentsInChildren<ParticleSystem>();
            pSystem = pSystemList[0];

            for(int i = 0; i < pSystemList.Length; i++) {
                var current = pSystemList[i];

                var realDuration = Mathf.Max(current.main.duration, current.main.startLifetime.constant + current.main.startDelay.constant);
                if(duration < realDuration) {
                    duration = realDuration;
                }
            }

        }

        public void SetPosition(Vector3 pos) {
            this.transform.position = pos;
        }

        public void Play(float callbackNormalizedTime = 0f, System.Action callback = null){
            // playing = true;
            if(pSystem.isPlaying) {
                pSystem.Clear(true);
            }

            pSystem.Play();
            var startTime = Time.time;

            if(callback != null) {
                if(callbackHandlerCoroutine != null) {
                    StopCoroutine(callbackHandlerCoroutine);
                }

                callbackHandlerCoroutine = StartCoroutine(HandleCallback(startTime, callbackNormalizedTime, callback));
            }
        }

        private IEnumerator HandleCallback(float startTime, float callbackNormalizedTime, System.Action callback) {
            yield return new WaitUntil(() => Time.time - startTime >= callbackNormalizedTime * duration);

            callback();

            // playing = false;
            callbackHandlerCoroutine = null;
        }
    }
}
