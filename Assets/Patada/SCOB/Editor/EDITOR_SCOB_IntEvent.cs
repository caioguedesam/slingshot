using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

using Patada.Events;

[CustomEditor(typeof(SCOB_IntEvent))]
public class EDITOR_SCOB_IntEvent : Editor {
    private int value = 0;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        value = EditorGUILayout.IntField("Event Value", value);

        if(GUILayout.Button("Raise")){
            Debug.Log("TEs");
            ((SCOB_IntEvent)target).Raise(value);
        }
    }
}
