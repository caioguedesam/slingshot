using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLogger : MonoBehaviour {
    private RectTransform rt = null;

    void Start(){
        rt = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update() {
        var s = transform.position.ToString();
        if(rt != null) {
            s += " " + rt.anchoredPosition.ToString();
        }
        Debug.LogWarning(s);
    }
}
