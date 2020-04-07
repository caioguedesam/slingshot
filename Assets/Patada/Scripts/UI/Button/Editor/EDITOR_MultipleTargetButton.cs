using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(MultipleTargetButton))]
public class EDITOR_MultipleTargetButton : EDITOR_Button {
    SerializedProperty otherGraphicsProperty = null;

    void OnEnable() {
        if(target is MultipleTargetButton) {
            otherGraphicsProperty = serializedObject.FindProperty("otherGraphics");
        } else {
        }
    }

    public override void OnInspectorGUI() {
        if(target is MultipleTargetButton) {
            EditorGUILayout.PropertyField(otherGraphicsProperty,true);
            serializedObject.ApplyModifiedProperties();
            Debug.Log("Oia só");
        } else {
            Debug.Log("Oia só 2");
        }

        base.OnInspectorGUI();
    }
}
