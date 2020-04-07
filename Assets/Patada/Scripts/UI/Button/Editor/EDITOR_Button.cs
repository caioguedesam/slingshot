using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(Button), true)]
public class EDITOR_Button : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}
