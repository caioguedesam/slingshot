using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Vec3EventListener))]
public class EDITOR_Vec3EventListener : Editor {
    public override void OnInspectorGUI() {
        DrawPropertiesExcluding(this.serializedObject, new string[]{
            "conditionalListener",
            "ConditionalNotResponse",
            "ConditionalNotActions",
            "idValue"
        });

        // var serializedObj = this.serializedObject;
        
        // var conditional = serializedObject.FindProperty("conditionalListener");
        // EditorGUILayout.PropertyField(conditional);

        

        

        // if(conditional.boolValue) {
        //     var id = serializedObject.FindProperty("idValue");
        //     EditorGUILayout.PropertyField(id,new GUIContent("ID"));

        //     EditorGUILayout.Space();
        //     EditorGUILayout.Space();

        //     EditorGUILayout.LabelField("Condition is true", EditorStyles.boldLabel);
        //     EditorGUI.indentLevel++;
        //     var response = serializedObject.FindProperty("Response");
        //     EditorGUILayout.PropertyField(response, new GUIContent("Response"));

        //     var actions = serializedObject.FindProperty("Actions");
        //     EditorGUILayout.PropertyField(actions, new GUIContent("Actions"), true);
        //     EditorGUI.indentLevel--;

        //     EditorGUILayout.Space();
        //     EditorGUILayout.Space();

        //     EditorGUILayout.LabelField("Condition is false", EditorStyles.boldLabel);
        //     EditorGUI.indentLevel++;
        //     var response2 = serializedObject.FindProperty("ConditionalNotResponse");
        //     EditorGUILayout.PropertyField(response2, new GUIContent("Response"));

        //     var actions2 = serializedObject.FindProperty("ConditionalNotActions");
        //     EditorGUILayout.PropertyField(actions2, new GUIContent("Actions"), true);
        //     EditorGUI.indentLevel--;

        // } else {
            
        //     EditorGUILayout.Space();
        //     EditorGUILayout.Space();

        //     EditorGUI.indentLevel++;
        //     var response = serializedObject.FindProperty("Response");
        //     EditorGUILayout.PropertyField(response);

        //     var actions = serializedObject.FindProperty("Actions");
        //     EditorGUILayout.PropertyField(actions,true);
        //     EditorGUI.indentLevel--;
        // }

        

        serializedObject.ApplyModifiedProperties();

    }
}
