// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// using System.Threading.Tasks;
// using System.Linq;

// using Firebase.Unity.Editor;

// public class FirebaseManager : MonoBehaviour {
//     private static FirebaseManager instance = null;
//     public static FirebaseManager Instance {
//         get {
//             if (instance == null) {
//                 instance = FindObjectOfType<FirebaseManager>();
//             }

//             return instance;
//         }
//     }

//     public static string Path {
//         get {
//             return "/matches_data/";
//         }
//     }

//     public static string OfflinePath {
//         get {
//             return "/offline_matches_data/";
//         }
//     }

//     private System.DateTime serverTime = new System.DateTime();
//     public System.DateTime ServerTime {
//         get {
//             Debug.Log("Getting server time!");
//             return serverTime;
//         }
//     }

//     private System.DateTime lastTimeCheckTimestamp = new System.DateTime();
//     public System.DateTime EstimatedServerTime {
//         get {
//             return serverTime + (System.DateTime.UtcNow - lastTimeCheckTimestamp);
//         }
//     }

//     private bool isFirebaseReady = false;
//     private bool initialized = false;
//     private bool saveLoadStarted = false;
//     private bool saveLoaded = false;

//     private Firebase.FirebaseApp firebaseApp = null;
//     private Firebase.Auth.FirebaseAuth firebaseAuth = null;
//     private Firebase.Database.DatabaseReference databaseRoot = null;

//     private Firebase.Auth.FirebaseUser currentUser = null;
//     public bool IsAnonymousAccount {
//         get {
//             return currentUser.IsAnonymous;
//         }
//     }

//     public string CurrentUserID {
//         get {
//             return currentUser.UserId;
//         }
//     }

//     private string deviceIdentifier = "";

//     // [SerializeField] private Animator loadingAnim = null;

//     private AsyncOperation sceneLoadOperation = null;
//     private bool gameSceneLoaded = false;

//     private bool newUser = false;
//     private bool finishedCreatingPlayer = false;

//     private System.Action onFirebaseInitialized = null;

//     void Awake() {
//         if (FirebaseManager.Instance != this) {
//             Destroy(FirebaseManager.instance.gameObject);
//             FirebaseManager.instance = this;
//         }

//         DontDestroyOnLoad(this);

//         deviceIdentifier = SystemInfo.deviceUniqueIdentifier;
//         InitFirebase();
//     }

//     void OnDestroy() {
//         firebaseAuth.StateChanged -= OnAuthChanged;
//     }

//     public void RegisterReadyListener(System.Action callback) {
//         onFirebaseInitialized += callback;
//     }

//     public void RemoveReadyListener(System.Action callback) {
//         onFirebaseInitialized -= callback;
//     }

//     private void InitFirebase() {
//         Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
//             var dependencyStatus = task.Result;
//             if (dependencyStatus == Firebase.DependencyStatus.Available) {

//                 firebaseApp = Firebase.FirebaseApp.DefaultInstance;
//                 firebaseApp.Options.DatabaseUrl = new System.Uri("https://magipipes.firebaseio.com/");

//                 firebaseAuth = Firebase.Auth.FirebaseAuth.GetAuth(firebaseApp);
//                 firebaseAuth.StateChanged += OnAuthChanged;

//                 databaseRoot = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;

//                 if (firebaseAuth.CurrentUser != null) {
//                     Debug.Log("Already signed in");
//                     newUser = false;
//                     OnAuthChanged(this, null);
//                     // isFirebaseReady = true;
//                 } else {
//                     Debug.Log("Sign in!");
//                     if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
//                         newUser = false;
//                         Login();
//                     } else {
//                         newUser = true;
//                         finishedCreatingPlayer = true;
//                         Login();
//                     }

//                 }


//             } else {
//                 Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
//                 isFirebaseReady = false;
//             }
//         });

//     }

//     public Task<Dictionary<string, string>> SaveSinglePlayerData(string path, object data) {
//         Debug.Log("Saving data to Server");
//         return SaveSinglePlayerData(path, "", data);

//     }

//     public Task<Dictionary<string, string>> SaveSinglePlayerData(string path, string subPath, object data) {
//         // Debug.Log("Saving data to Server {" + (path + " " + currentUser.UserId +  " " +subPath) + "} = " + data + " ;");
//         return Firebase.Database.FirebaseDatabase.DefaultInstance
//         .GetReference(path + currentUser.UserId + subPath)
//         .SetValueAsync(data).ContinueWith(task => {
//             // Debug.Log("Data saved: " + task.IsFaulted);

//             if (!task.IsFaulted && task.IsCompleted) {
//                 return new Dictionary<string, string>(){
//                     {"status","success"}
//                 };

//             } else {
//                 return new Dictionary<string, string>(){
//                     {"status","failed"},
//                     {"message",task.Exception.Message}
//                 };
//             }
//         });

//     }

//     public void PushMultipleData(string path, Dictionary<string, object> dataSet) {
//         var pushKey = "";
//         Dictionary<string, object> childUpdates = new Dictionary<string, object>();

//         foreach (var data in dataSet) {
//             var pathSnapshot = databaseRoot.Child(path + data.Key);

//             if(string.IsNullOrEmpty(pushKey)) {
//                 pushKey = pathSnapshot.Push().Key;
//             }

//             childUpdates.Add(path+data.Key+"/"+pushKey,data.Value);
//         }

//         databaseRoot.UpdateChildrenAsync(childUpdates);
//     }

//     public Task<Dictionary<string, string>> SaveMultiplePlayerData(string path, Dictionary<string, object> data) {
//         Debug.Log("Saving data to Server {" + (path + " " + currentUser.UserId) + "}");
//         return Firebase.Database.FirebaseDatabase.DefaultInstance
//         .GetReference(path + currentUser.UserId)
//         .UpdateChildrenAsync(data).ContinueWith(task => {
//             Debug.Log("Data saved: " + task.IsFaulted);

//             if (!task.IsFaulted && task.IsCompleted) {
//                 return new Dictionary<string, string>(){
//                     {"status","success"}
//                 };

//             } else {
//                 return new Dictionary<string, string>(){
//                     {"status","failed"},
//                     {"message",task.Exception.Message}
//                 };
//             }
//         });
//     }

//     public Task<Firebase.Database.DataSnapshot> LoadSinglePlayerData(string dbPath, string dataPath) {
//         Debug.Log("Loading data to Server");
//         return LoadSingleData(dbPath + currentUser.UserId + dataPath);
//     }

//     public Task<Firebase.Database.DataSnapshot> LoadSingleData(string path) {
//         Debug.Log("Loading data to Server");
//         return Firebase.Database.FirebaseDatabase.DefaultInstance
//         .GetReference(path)
//         .GetValueAsync().ContinueWith(task => {
//             if (!task.IsFaulted && task.IsCompleted) {
//                 return task.Result;
//             } else {
//                 return null;
//             }
//         });
//     }

//     public static Task<Firebase.Database.DataSnapshot> LoadSingleDataFromEditor(string path) {
//         Debug.Log("Loading data to Server");
//         return Firebase.Database.FirebaseDatabase.DefaultInstance
//         .GetReference(path)
//         .GetValueAsync().ContinueWith(task => {
//             if (!task.IsFaulted && task.IsCompleted) {
//                 return task.Result;
//             } else {
//                 return null;
//             }
//         });
//         // return null;
//     }

//     private void SignIn() {
//         Debug.Log("Sign in!");
//     }

//     private Task<bool> Login() {
//         return LoginWithCredentials("games@patada.studio", "gAXeyMUItkbkwWAGBC8C").ContinueWith((resultTask) => {
//             var success = resultTask.Result;
//             if (success) {
//                 // isFirebaseReady = true;
//                 return true;
//             } else {
//                 Debug.LogError("ID-1: Couldn't connect to firebase user. Retrying in 2 seconds");
//                 Invoke("Login", 2f);
//                 return false;
//             }
//         });
//     }

//     private Task<bool> LoginAnonymously() {
//         return firebaseAuth.SignInAnonymouslyAsync().ContinueWith(resultTask => {
//             if (resultTask.IsCanceled) {
//                 Debug.LogError("SignInAnonymouslyAsync was canceled.");
//                 return false;
//             }
//             if (resultTask.IsFaulted) {
//                 Debug.LogError("SignInAnonymouslyAsync encountered an error: " + resultTask.Exception);
//                 return false;
//             }

//             Debug.Log("User signed in successfully: " + currentUser.DisplayName + " (" + currentUser.UserId + ")");
//             return true;
//         });
//     }

//     private Task<bool> LoginWithCredentials(string id, string pw) {
//         return firebaseAuth.SignInWithEmailAndPasswordAsync(id, pw).ContinueWith(resultTask => {
//             if (resultTask.IsCanceled) {
//                 Debug.LogError("SignInAnonymouslyAsync was canceled.");
//                 return false;
//             }
//             if (resultTask.IsFaulted) {
//                 Debug.LogError("SignInAnonymouslyAsync encountered an error: " + resultTask.Exception);
//                 return false;
//             }

//             return true;
//         });
//     }

//     public Task<string> LinkAnonymousAccount(string email, string pw) {
//         var credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, pw);
//         return firebaseAuth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith((task) => {
//             if (task.IsCanceled) {
//                 Debug.LogError("LinkWithCredentialAsync was canceled.");
//                 return "failed";
//             }
//             if (task.IsFaulted) {
//                 Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
//                 return "failed";
//             }

//             Firebase.Auth.FirebaseUser newUser = task.Result;
//             Debug.LogFormat("Credentials successfully linked to Firebase user: {0} ({1})",
//             newUser.DisplayName, newUser.UserId);
//             return "success";
//         });
//     }

//     public Task<string> ConnectToAccount(string email, string pw) {
//         if (currentUser != null && string.IsNullOrEmpty(currentUser.UserId)) {
//             firebaseAuth.SignOut();
//         }

//         var credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, pw);
//         return firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith((task) => {
//             if (task.IsCanceled) {
//                 Debug.LogError("Connect to Account was canceled.");
//                 return "failed";
//             }
//             if (task.IsFaulted) {
//                 Debug.LogError("Connect to Account encountered an error: " + task.Exception);
//                 return "failed";
//             }

//             Firebase.Auth.FirebaseUser newUser = task.Result;
//             return "success";
//         });
//     }

//     private void OnAuthChanged(object sender, System.EventArgs eventArgs) {
//         Debug.Log("OnAuthChanged");
//         if (firebaseAuth.CurrentUser != currentUser) {
//             bool signedIn = currentUser != firebaseAuth.CurrentUser && firebaseAuth.CurrentUser != null;
//             if (!signedIn && currentUser != null) {
//                 Debug.Log("Signed out");
//                 currentUser = null;
//             }

//             if (signedIn) {
//                 currentUser = firebaseAuth.CurrentUser;
//                 Debug.Log("Signed in " + currentUser.UserId);
//                 isFirebaseReady = true;

//                 if(onFirebaseInitialized != null) {
//                     onFirebaseInitialized();
//                 }
//             }
//         }
//     }
// }
