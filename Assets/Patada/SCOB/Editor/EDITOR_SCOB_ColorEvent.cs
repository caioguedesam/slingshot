using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

using Patada.Events;

[CustomEditor(typeof(SCOB_ColorEvent))]
public class EDITOR_SCOB_ColorEvent : Editor {
    private Color value = Color.white;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        value = EditorGUILayout.ColorField("Event Value", value);

        if(GUILayout.Button("Raise")){
            Debug.Log("TEs");
            ((SCOB_ColorEvent)target).Raise(value);
        }
    }
}
