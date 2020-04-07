using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Patada {
    public abstract class IntroductionSceneManager : MonoBehaviour {
        [SerializeField] protected string privacyPolicyTermLink = "";
        [SerializeField] protected string firstSceneName = "";
        protected bool accepted = false;
        protected bool initialized = false;
        protected AsyncOperation loadSceneOperation = null;

        public virtual void Awake(){
            if(Save.Instance.GetBool("privacy_terms_accepted", false)) {
                initialized = true;
                SceneManager.LoadScene(firstSceneName);
            }
        }

        public virtual void Start() {
            if(!initialized) {
                loadSceneOperation = SceneManager.LoadSceneAsync(firstSceneName);
                loadSceneOperation.allowSceneActivation = false;

                accepted = Save.Instance.GetBool("privacy_terms_accepted", false);
                
                if (accepted) {
                    loadSceneOperation.allowSceneActivation = true;
                }
            }
        }

        public void Update() {
            if (accepted && !loadSceneOperation.allowSceneActivation) {
                loadSceneOperation.allowSceneActivation = true;
            }
        }

        public void Accept() {
            Save.Instance.SetBool("privacy_terms_accepted", true);
            accepted = true;
        }

        public void OpenTerms() {
            Application.OpenURL(privacyPolicyTermLink);
        }
    }
}
