using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

using Patada.Events;

[CustomEditor(typeof(SCOB_MaterialEvent))]
public class EDITOR_SCOB_MaterialEvent : Editor {
    private Material value = null;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        value = (Material)EditorGUILayout.ObjectField(value, typeof (Material));

        if(GUILayout.Button("Raise")){
            Debug.Log("TEs");
            ((SCOB_MaterialEvent)target).Raise(value);
        }
    }
}
