using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformLogger : MonoBehaviour {
    private RectTransform rt = null;

    void Start(){
        rt = GetComponent<RectTransform>();
    }
    
    // Update is called once per frame
    void Update() {
        var s = "";
        if(rt != null) {
            s += rt.sizeDelta.ToString() + " " + rt.anchoredPosition.ToString();
        }
        Debug.LogWarning(s);
    }
}
