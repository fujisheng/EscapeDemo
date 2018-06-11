using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClickButton),true)]
public class ClickButtonEditor : Editor {
    public override void OnInspectorGUI()
    {
        ClickButton clickButton = (ClickButton)target;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Recording", GUILayout.Width(100)))
            clickButton.Recording();
        if (GUILayout.Button("Diaplasis", GUILayout.Width(100)))
            clickButton.Diaplasis();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("RecordingParent", GUILayout.Width(100)))
            clickButton.RecordingParent();
        if (GUILayout.Button("DiaplasisParent", GUILayout.Width(100)))
            clickButton.DiaplasisParent();
        EditorGUILayout.EndHorizontal();
  
        base.DrawDefaultInspector();
    }
}
