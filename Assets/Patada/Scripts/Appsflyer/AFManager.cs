// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AFManager : MonoBehaviour {
//     private static AFManager instance = null;
//     public static AFManager Instance {
//         get{
//             if(instance == null) {
//                 instance = FindObjectOfType<AFManager>();
//             }

//             return instance;
//         }
//     }

//     [SerializeField] private string appsflyerDevKey = "";
//     [SerializeField] private string androidAppID = "";
//     [SerializeField] private string iosAppID = "";

//     private bool startedInit = false;

//     void Start() {
//         if(Instance != this) {
//             this.enabled = false;
//             Destroy(this.gameObject);
        
//         } else {
//             DontDestroyOnLoad(this);
//             StartAF();
//         }
//     }

//     void Update() {
//         if(!startedInit) {
//             if(!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Introduction")) {
//                 Debug.Log("Init AF");
//                 StartAF();
//             }
//         }
//     }

//     public void StartAF() {
//         startedInit = true;
//         /* Mandatory - set your AppsFlyer’s Developer key. */
//         AppsFlyer.setAppsFlyerKey (appsflyerDevKey);
//         /* For detailed logging */
//         /* AppsFlyer.setIsDebug (true); */
//         #if UNITY_IOS
//         /* Mandatory - set your apple app ID
//         NOTE: You should enter the number only and not the "ID" prefix */
//         AppsFlyer.setAppID (iosAppID);
//         AppsFlyer.trackAppLaunch ();
//         #elif UNITY_ANDROID
//         /* Mandatory - set your Android package name */
//         AppsFlyer.setAppID (androidAppID);
//         /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
//         AppsFlyer.init (appsflyerDevKey,"AppsFlyerTrackerCallbacks");
//         #endif
//     }
// }
