using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

using Patada.Events;

[CustomEditor(typeof(SCOB_SpriteEvent))]
public class EDITOR_SCOB_SpriteEvent : Editor {
    private Sprite value = null;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        value = (Sprite)EditorGUILayout.ObjectField(value, typeof(Sprite));

        if(GUILayout.Button("Raise")){
            Debug.Log("TEs");
            ((SCOB_SpriteEvent)target).Raise(value);
        }
    }
}
