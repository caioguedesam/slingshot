using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;

public class Save : MonoBehaviour {
    public static readonly IList<object> DefaultItems = new ReadOnlyCollection<object>(
        new List<object>(){
            "!key",
            "umvHLhgpsJn9DQzTncZnSyi7dGWy34o7uNEI8dUyCRQ22oKdTM3esbY9CZexPsUR67zSuSXbu1WTCp18nX2NYXFvK",
            "finished_tutorial",
            false,
            "key",
            "S3zu2bc3DSBd9siLCznPFFrsPB2DRI0NQ8EpsVZZm1QiXjAu4xm",
            "sfx_muted",
            false,
            "music_muted",
            false,
            "~key",
            "6r9nF60vSImHhfiISXBjTbEgVQgeEqKpDtdKkGOVwdd4JTXuNIBeaQMH2JTa"
        }
    );

    [SerializeField] private string editorFileName = "/editor_data.dat";
    [SerializeField] private string buildFileName = "/build_data.dat";
    private string FileName {
        get{
            #if UNITY_EDITOR
            return editorFileName;
            #else
            return buildFileName;
            #endif
        }
    }

    private static Save instance = null;
    public static Save Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Save>();

                if(instance == null) {
                    var newObj = new GameObject();
                    newObj.name = "Save";
                    newObj.AddComponent(typeof(Save));

                    instance = newObj.GetComponent<Save>();
                    instance.LoadStatus();
                }
                
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    private const string key = "umvHLhgpsJn9DQzTncZnSyi7dGWy34o7uNEI8dUyCRQ22oKdTM3esbY9CZexPsUR67zSuSXbu1WTCp18nX2NYXFvKS3zu2bc3DSBd9siLCznPFFrsPB2DRI0NQ8EpsVZZm1QiXjAu4xm6r9nF60vSImHhfiISXBjTbEgVQgeEqKpDtdKkGOVwdd4JTXuNIBeaQMH2JTa";

    //	private JSONObject playerSave;
    private FileStream saveFile = null;
    private bool newSave = false;
    private BinaryFormatter formatter = new BinaryFormatter();

    private GameData saveData;

    private bool deletingSave = false;

    public float Version {
        get {
            if (saveData != null) {
                return saveData.Version;
            } else {
                return 0f;
            }
        }
    }

    public Dictionary<string, object> PublicItems {
        get {
            if (saveData != null) {
                return saveData.PublicItems;
            }

            return null;
        }
    }

    private bool loaded = false;
    // private System.Action<int> onCoinsChange = null;

    // Use this for initialization
    void Awake() {
        if (Instance != this) {
            Destroy(this.gameObject);
        } else {
            var temp = Save.Instance;
        }

    }

	public void OnDestroy() {
		if (saveData != null) {
			SaveStatus ();
//			saveFile.Close ();
		}
	}

	public void OnApplicationPause(bool paused) {
		if (paused && saveData != null) {
			SaveStatus ();
		}
	}

    private void SaveStatus() {
        if (!deletingSave) {

            Debug.Log("Saving Locally...");
            saveFile = File.Open(Application.persistentDataPath + FileName, FileMode.Open);
            formatter.Serialize(saveFile, saveData);
            saveFile.Close();
        }
    }

    public void LoadStatus() {
        if(!loaded) {
            Debug.Log("Loading save...");
            loaded = true;

            //already has an save file
            if (File.Exists(Application.persistentDataPath + FileName)) {
                // Debug.Log("Loading save2...");
                saveFile = File.Open(Application.persistentDataPath + FileName, FileMode.Open);
                saveFile.Position = 0;
                // Debug.Log("Loading save3...");
                try {
                    saveData = (GameData)formatter.Deserialize(saveFile);
                    saveFile.Close();

                    if (!saveData.Key.Equals(key)) {
                        // Debug.Log("Save has invalid key\n");
                        saveFile = File.Create(Application.persistentDataPath + FileName);
                        saveData = new GameData(Application.version);
                        newSave = true;
                        saveFile.Close();
                    }
                    // Debug.Log("Loading save5...");

                } catch (System.Runtime.Serialization.SerializationException e) {
                    Debug.Log(e.Message);
                    saveFile.Close();

                    // Debug.Log("Couldn't read file : " + e.Message);
                    saveFile = File.Create(Application.persistentDataPath + FileName);
                    saveData = new GameData(Application.version);
                    newSave = true;
                    saveFile.Close();
                } catch (FileLoadException e) {
                    Debug.Log(e.Message);
                    saveFile.Close();

                    // Debug.Log("Couldn't read file : " + e.Message);
                    saveFile = File.Create(Application.persistentDataPath + FileName);
                    saveData = new GameData(Application.version);
                    newSave = true;
                    saveFile.Close();
                }

                //doesnt have save file
            } else {
                Debug.Log("Couldn't find file creating a new one!");
                saveFile = File.Create(Application.persistentDataPath + FileName);
                saveData = new GameData(Application.version);
                newSave = true;
                saveFile.Close();
            }
        }
    }

    public void Clean() {
        Debug.Log("Cleaning Save");

        File.Delete(Application.persistentDataPath + FileName);
        loaded = false;
        LoadStatus();

        // UnityEngine.SceneManagement.SceneManager.LoadScene("EntranceScene");
    }

    // public void OnApplicationQuit() {
    //     if (saveData != null) {
    //         SaveStatus();
    //         //			saveFile.Close ();
    //     }
    // }

    // public void OnApplicationPause(bool paused) {
    //     if (paused && saveData != null) {
    //         SaveStatus();
    //     }
    // }

    public void SetObject(string key, object o) {
        if (key.Equals("lastOpenTime")) {
            Debug.Log("Saving lastOpenTime : " + o);
        }
        saveData.SetObject(key, o);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    public void SetInt(string key, int value) {
        SetObject(key, value);
    }

    public void SetString(string key, string value) {
        SetObject(key, value);
    }

    public void SetFloat(string key, float value) {
        SetObject(key, value);
    }

    public void SetBool(string key, bool value) {
        SetObject(key, value);
    }

    public object GetObject(string key, object defaultValue = null) {
        if(!saveData.Contains(key)) {
            return defaultValue;
        }

        return saveData.GetObject(key);
    }

    public int GetInt(string key, int defaultValue = 0) {
        if(!saveData.Contains(key)) {
            return defaultValue;
        }

        return (int)(GetObject(key));
    }

    public string GetString(string key, string defaultValue = null) {
        if(!saveData.Contains(key)) {
            return defaultValue;
        }

        return (string)GetObject(key);
    }

    public float GetFloat(string key, float defaultValue = 0f) {
        if(!saveData.Contains(key)) {
            return defaultValue;
        }

        return (float)(GetObject(key));
    }

    public bool GetBool(string key, bool defaultValue = false) {
        if(!saveData.Contains(key)) {
            return defaultValue;
        }

        return (bool)(GetObject(key));
    }

}

[System.Serializable]
class GameData : System.Runtime.Serialization.IDeserializationCallback {
    private Dictionary<string, object> items;

    public Dictionary<string, object> PublicItems {
        get {
            if (items != null) {
                return new Dictionary<string, object>(items);
            }

            return null;
        }
    }

    public string Key {
        get {
            return (string)GetObject("!key") + (string)GetObject("key") + (string)GetObject("~key");
        }
    }

    public float Version {
        get {
            if (items != null && items.ContainsKey("version")) {
                return (float)GetObject("version");
            } else {
                return 0f;
            }
        }

        set {
            this.items["version"] = value;
        }
    }

    public GameData(string version) {
        Debug.Log("New Save!");
        InitializeDefaults();
        Version = float.Parse(version, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
    }

    public GameData(Dictionary<string, object> data) {
        items = new Dictionary<string, object>();
        foreach (var item in data) {
            items.Add(item.Key, item.Value);
        }

        InitializeDefaults();
    }

    private void InitializeDefaults() {
        if (this.items == null) {
            this.items = new Dictionary<string, object>();
        }

        var defaultItems = Save.DefaultItems;
        for (int i = 0; i < defaultItems.Count; i += 2) {
            if (!items.ContainsKey((string)defaultItems[i])) {
                items.Add((string)defaultItems[i], defaultItems[i + 1]);
            }
        }
    }

    public void SetObject(string key, object o) {
        items[key] = o;
    }

    public object GetObject(string key) {
        if (!items.ContainsKey(key)) {
            InitializeDefaults();
        }

        return items[key];
    }

    public bool Contains(string key) {
        return items.ContainsKey(key);
    }

    #region IDeserializationCallback implementation

    void System.Runtime.Serialization.IDeserializationCallback.OnDeserialization(object sender) {
        if (items != null) {
            items.OnDeserialization(sender);
        }

        InitializeDefaults();
    }

    #endregion
}
