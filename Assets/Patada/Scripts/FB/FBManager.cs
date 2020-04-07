// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // Include Facebook namespace
// using Facebook.Unity;

// public class FBManager : MonoBehaviour {
//     private static FBManager instance = null;
//     public static FBManager Instance {
//         get{
//             if(instance == null) {
//                 instance = FindObjectOfType<FBManager>();
//             }

//             return instance;
//         }
//     }

//     private bool startedInit = false;
//     // Awake function from Unity's MonoBehavior
//     void Awake() {
//         if(Instance != this) {
//             Destroy(this.gameObject);
//         } else {
//             DontDestroyOnLoad(this.gameObject);

//             // if(Save.Instance.GetBool("finished_tutorial", false)) {
//                 // Debug.Log("Init FB");
//                 // InitFB();
//             // }
//         }
//     }

//     void Update() {
//         if(!startedInit && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)) {
//             if(!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Introduction")) {
//                 Debug.Log("Init FB");
//                 InitFB();
//             }
//         }
//     }

//     private void InitFB() {
//         startedInit = true;
//         if (!FB.IsInitialized) {
//             // Initialize the Facebook SDK
//             FB.Init(InitCallback, OnHideUnity);
//         } else {
//             // Already initialized, signal an app activation App Event
//             Debug.Log("FB Activated");
//             FB.ActivateApp();
//         }
//     }

//     private void InitCallback() {
//         if (FB.IsInitialized) {
//             // Signal an app activation App Event
//             Debug.Log("FB Activated");
//             FB.ActivateApp();
//             // Continue with Facebook SDK
//             // ...
//         } else {
//             startedInit = false;
//             Debug.Log("Failed to Initialize the Facebook SDK");
//         }
//     }

//     private void OnHideUnity(bool isGameShown) {
//         if (!isGameShown) {
//             // Pause the game - we will need to hide
//             Time.timeScale = 0;
//         } else {
//             // Resume the game - we're getting focus again
//             Time.timeScale = 1;
//         }
//     }
// }
