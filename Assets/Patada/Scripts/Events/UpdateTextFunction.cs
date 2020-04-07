using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Patada.Functions;

public class UpdateTextFunction : MonoBehaviour {
    [SerializeField] private SCOB_StringFunction stringFunc = null;
    [SerializeField] private StringUnityEvent uActions = null;

    public void SetText() {
        uActions.Invoke(stringFunc.Call());
    }
}
