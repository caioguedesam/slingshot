using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class EDITOR_AudioManager : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		// DrawPropertiesExcluding(this.serializedObject, new string[]{"clips"});
	}
}
