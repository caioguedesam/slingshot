using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisabler : MonoBehaviour {
    public void Enable() {
        Debug.unityLogger.logEnabled = true;
    }

    public void Disable() {
        Debug.unityLogger.logEnabled = false;
    }
}