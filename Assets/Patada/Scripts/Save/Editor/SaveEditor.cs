using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CanEditMultipleObjects]
[CustomEditor(typeof(Save))]
public class SaveEditor : Editor {

    void OnEnable() {

    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        Save myTarget = (Save)target;

        if(UnityEngine.Application.isPlaying) {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Save Version: ", myTarget.Version.ToString());
            EditorGUILayout.Space();

            var savedVariables = myTarget.PublicItems;
            if(savedVariables != null) {
                EditorGUILayout.LabelField("Saved Items: ", (savedVariables.Count).ToString());
        
                EditorGUILayout.Space();
                foreach(var variable in savedVariables){
                    if(!variable.Key.Equals("version")) {
                        if(variable.Value != null) {
                            EditorGUILayout.LabelField(variable.Key, variable.Value.ToString());
                        } else {
                            EditorGUILayout.LabelField(variable.Key, "null");
                        }
                    }
                }
            } else {
                EditorGUILayout.LabelField("No saved Items");
            }
        }
        
        EditorGUILayout.Space();
        if(GUILayout.Button("Reset Save")){
            if(UnityEditor.EditorUtility.DisplayDialog("Deleting Save","Are you sure you want to erase the save?","Confirm","Cancel")){
                ((Save)target).Clean();
            }
        }


        serializedObject.ApplyModifiedProperties();

        DrawDefaultInspector ();

        // EditorUtility.SetDirty( myTarget );
    }
}
