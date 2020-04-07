using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

using Patada.Events;

[CustomEditor(typeof(SCOB_BaseEvent))]
public class EDITOR_SCOB_Event : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Raise")){
            ((SCOB_Event)target).Raise();
        }
    }
}
