using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Patada.Tween;

[CustomEditor(typeof(TweenObject))]
[CanEditMultipleObjects]
public class TweenObjectEditor : Editor {
    public override void OnInspectorGUI () {
		base.OnInspectorGUI();
		
		var tweenObject = target as TweenObject;

		EditorGUILayout.LabelField("Positions", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add") ){
			tweenObject.AddPosition();
		}

		if(GUILayout.Button("Remove") ){
			tweenObject.RemovePosition();
		}

		if(GUILayout.Button("Clear") ){
			tweenObject.ClearPosition();
		}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Play") ){
			tweenObject.PlayTween();
		}

		if(GUILayout.Button("Stop") ){
			tweenObject.StopTween();
		}
		EditorGUILayout.EndHorizontal();
	}
}
