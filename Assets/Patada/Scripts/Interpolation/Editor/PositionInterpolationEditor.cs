using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(PositionInterpolation))]
public class PositionInterpolationEditor : Editor {
    private static GUIStyle toggleButtonStyleNormal = null;
    private static GUIStyle toggleButtonStyleToggled = null;

    private static GUIStyle componentBGStyle = null;
    private static GUIStyle pathsBGStyle = null;

    void OnEnable() {
        Initialize();
    }

    private void Initialize() {
        if (toggleButtonStyleNormal == null) {
            Texture2D[] __texture_array = new Texture2D[3]{ new Texture2D(1,1),new Texture2D(1,1),new Texture2D(1,1) };
 
            __texture_array[0].SetPixel(0,0,Color.grey);
            __texture_array[0].Apply();
            
            __texture_array[1].SetPixel(0,0,Color.grey * .05f);
            __texture_array[1].Apply();
            
            __texture_array[2].SetPixel(0,0,Color.grey * .01f);
            __texture_array[2].Apply();

            toggleButtonStyleNormal = EditorStyles.toolbarButton;
            toggleButtonStyleToggled = new GUIStyle(toggleButtonStyleNormal);
            toggleButtonStyleToggled.normal.background = __texture_array[0];
            toggleButtonStyleToggled.fontStyle = FontStyle.Bold;

            componentBGStyle = new GUIStyle();
            componentBGStyle.normal.background = __texture_array[1];

            pathsBGStyle = EditorStyles.helpBox;
        }
    }


    public override void OnInspectorGUI() {
        Initialize();
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.BeginVertical(componentBGStyle);
        GUILayout.BeginHorizontal();
        var threeDInterpolation = this.serializedObject.FindProperty("threeDInterpolation");
        var is2DLerp = (threeDInterpolation.boolValue == false);
        if(GUILayout.Button("2D",  is2DLerp ? toggleButtonStyleToggled : toggleButtonStyleNormal)) {
            threeDInterpolation.boolValue = false;
        }

        if(GUILayout.Button("3D",  !is2DLerp ? toggleButtonStyleToggled : toggleButtonStyleNormal)) {
            threeDInterpolation.boolValue = true;
        }
        GUILayout.EndHorizontal();

        

        EditorGUILayout.Space();
        var debugging = this.serializedObject.FindProperty("debugging");
        if(GUILayout.Button("Debug" + (debugging.boolValue ? " ON" : " OFF"), debugging.boolValue ? toggleButtonStyleToggled : toggleButtonStyleNormal)) {
            debugging.boolValue = !debugging.boolValue;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("startDelay"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onStart"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onFinished"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("duration"));
        EditorGUILayout.Space();
        if(is2DLerp) {
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("holder"),new GUIContent("Holder","Parent in which this canvas element will be positioned"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("canvas"),new GUIContent("Canvas","Canvas in which this canvas element will be positioned"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("canvasScaler"),new GUIContent("Canvas Scaler","Canvas Scaler in which this canvas element will be positioned"));
        }
        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        var smoothsArray = this.serializedObject.FindProperty("smooths");
        for(int i = 0 ; i < smoothsArray.arraySize ; i++) {
            var index = i;
            var groupRect = EditorGUILayout.BeginVertical(pathsBGStyle);

            var currentItem = smoothsArray.GetArrayElementAtIndex(index);
            var expanded = currentItem.FindPropertyRelative("expanded");

            GUILayout.Space(5f);

            var titleRect = EditorGUILayout.BeginHorizontal();
            var label = "Path Curve #" + (index+1).ToString();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel,new GUILayoutOption[] {GUILayout.Width(label.Length * 8f), GUILayout.ExpandWidth(false)});

            if (GUILayout.Button(expanded.boolValue ? "Hide" : "Show", new GUILayoutOption[] {GUILayout.Height(15), GUILayout.Width(45)})) {
                expanded.boolValue = !expanded.boolValue;
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);

            if(smoothsArray.arraySize > 1) {
                var bkpBackgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                if (GUI.Button(new Rect(groupRect.width - 5f, groupRect.y + 5f, 20f, 20f), "X")) {
                    smoothsArray.DeleteArrayElementAtIndex(index);
                    smoothsArray.serializedObject.ApplyModifiedProperties();
                    this.serializedObject.ApplyModifiedProperties();
                    return;
                }
                GUI.backgroundColor = bkpBackgroundColor;
            }

            
            if(expanded.boolValue) {
                EditorGUI.indentLevel++;
                var xSmooth = currentItem.FindPropertyRelative("x");
                var ySmooth = currentItem.FindPropertyRelative("y");
                var zSmooth = currentItem.FindPropertyRelative("z");

                xSmooth.animationCurveValue = EditorGUILayout.CurveField("X Lerp", xSmooth.animationCurveValue);
                ySmooth.animationCurveValue = EditorGUILayout.CurveField("Y Lerp", ySmooth.animationCurveValue);
                if(!is2DLerp) {
                    zSmooth.animationCurveValue = EditorGUILayout.CurveField("Z Lerp", zSmooth.animationCurveValue);
                }

                EditorGUILayout.Space();
                var xAdditional = currentItem.FindPropertyRelative("xAdditional");
                var yAdditional = currentItem.FindPropertyRelative("yAdditional");
                var zAdditional = currentItem.FindPropertyRelative("zAdditional");

                xAdditional.animationCurveValue = EditorGUILayout.CurveField("X Additional Lerp", xAdditional.animationCurveValue);
                yAdditional.animationCurveValue = EditorGUILayout.CurveField("Y Additional Lerp", yAdditional.animationCurveValue);
                if(!is2DLerp) {
                    zAdditional.animationCurveValue = EditorGUILayout.CurveField("Z Additional Lerp", zAdditional.animationCurveValue);
                }

                EditorGUILayout.Space();
                var xMaxNoise = currentItem.FindPropertyRelative("xMaxNoise");
                var yMaxNoise = currentItem.FindPropertyRelative("yMaxNoise");
                var zMaxNoise = currentItem.FindPropertyRelative("zMaxNoise");

                xMaxNoise.floatValue = EditorGUILayout.FloatField("X Max Noise", xMaxNoise.floatValue);
                yMaxNoise.floatValue = EditorGUILayout.FloatField("Y Max Noise", yMaxNoise.floatValue);
                if(!is2DLerp) {
                    zMaxNoise.floatValue = EditorGUILayout.FloatField("Z Max Noise", zMaxNoise.floatValue);
                }

                EditorGUILayout.Space();
                var noiseMode = currentItem.FindPropertyRelative("noiseMode");
                noiseMode.enumValueIndex = EditorGUILayout.Popup("Noise Mode", noiseMode.enumValueIndex, noiseMode.enumDisplayNames);

                EditorGUILayout.Space();
                var perpNoiseOnly = currentItem.FindPropertyRelative("onlyPerpendicularNoise");
                perpNoiseOnly.boolValue = EditorGUILayout.Toggle("Only Perpendicular Noise", perpNoiseOnly.boolValue);

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }


            EditorGUILayout.EndVertical();
        }
        if(GUILayout.Button("Add Curve")) {
            smoothsArray.arraySize++;
            smoothsArray.serializedObject.ApplyModifiedProperties();
            this.serializedObject.ApplyModifiedProperties();
            return;
        }
        GUILayout.EndVertical();
        
        GUILayout.EndVertical();
        this.serializedObject.ApplyModifiedProperties();
    }
}
