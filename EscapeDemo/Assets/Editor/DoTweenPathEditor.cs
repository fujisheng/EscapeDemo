using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(DoTweenPath))]
public class DoTweenPathEditor : Editor {

    DoTweenPath doTweenPath;
    SerializedProperty wayPoints;

    void OnEnable(){
        doTweenPath = (DoTweenPath)target;
        wayPoints = serializedObject.FindProperty("wayPoints");
    }

    public override void OnInspectorGUI()
    { 
        serializedObject.Update(); 

        EditorGUILayout.LabelField("Option", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("------------------------------------------------------", EditorStyles.boldLabel);
        doTweenPath.autoPlay = EditorGUILayout.Toggle("AutoPlay",doTweenPath.autoPlay,GUILayout.Width(200));
        doTweenPath.defaultDuration = EditorGUILayout.FloatField("DefaultDuration",doTweenPath.defaultDuration, GUILayout.Width(200));
        doTweenPath.defaultStayTime = EditorGUILayout.FloatField("DefaultStayTime",doTweenPath.defaultStayTime, GUILayout.Width(200));
        doTweenPath.ease = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("EaseType", doTweenPath.ease, GUILayout.Width(200));
        doTweenPath.loops = EditorGUILayout.IntField("Loops",doTweenPath.loops,GUILayout.Width(200));
        if (doTweenPath.loops > 1)
        {
            doTweenPath.loopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup("LoopType",doTweenPath.loopType, GUILayout.Width(200));
        }
        EditorGUILayout.LabelField("WayPoints", EditorStyles.boldLabel);
        if (GUILayout.Button("Replacement", GUILayout.Width(100)))
        {
            if (doTweenPath.wayPoints.Count != 0)
                doTweenPath.transform.localPosition = doTweenPath.wayPoints[0].position;
            else
                doTweenPath.transform.localPosition = Vector3.zero;
        }
        if (doTweenPath.wayPoints.Count >= 3)
            doTweenPath.seal = EditorGUILayout.Toggle("Seal", doTweenPath.seal, GUILayout.Width(200));
        EditorGUILayout.LabelField("------------------------------------------------------", EditorStyles.boldLabel);
        if (doTweenPath.wayPoints.Count == 0)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("+", GUILayout.Width(200)))
            {
                WayPoint wayPoit = new WayPoint();
                doTweenPath.wayPoints.Add(wayPoit);
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            for (int i = 0; i < doTweenPath.wayPoints.Count; i++)
            {
                DrawPointsInspector(i);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
    void DrawPointsInspector(int index){
        if (index != 0)
            EditorGUILayout.LabelField("------------------------------------------------------", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Point " + (index + 1).ToString(), EditorStyles.boldLabel);
        doTweenPath.wayPoints[index].useDefaultData = EditorGUILayout.Toggle("UseDefaultData",doTweenPath.wayPoints[index].useDefaultData, GUILayout.Width(200));
        if (doTweenPath.wayPoints[index].useDefaultData == true)
        {
            doTweenPath.wayPoints[index].duration = doTweenPath.defaultDuration;
            doTweenPath.wayPoints[index].stayTime = doTweenPath.defaultStayTime;
        }
        else
        {
            doTweenPath.wayPoints[index].duration = EditorGUILayout.FloatField("Duration",doTweenPath.wayPoints[index].duration, GUILayout.Width(200));
            doTweenPath.wayPoints[index].stayTime = EditorGUILayout.FloatField("StayTime",doTweenPath.wayPoints[index].stayTime, GUILayout.Width(200));

        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("R", GUILayout.Width(30)))
        {
            doTweenPath.wayPoints[index].position = doTweenPath.transform.localPosition;
        }
        GUILayout.Label("  P", GUILayout.Width(30));
        doTweenPath.wayPoints[index].position = EditorGUILayout.Vector3Field("", doTweenPath.wayPoints[index].position, GUILayout.Width(200));
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            WayPoint wayPoit = new WayPoint();
            doTweenPath.wayPoints.Insert(index + 1, wayPoit);
        }
        if (GUILayout.Button("X", GUILayout.Width(20)))
        {
            doTweenPath.wayPoints.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        doTweenPath.wayPoints[index].showInEvents = EditorGUILayout.Toggle("InEvents",doTweenPath.wayPoints[index].showInEvents, GUILayout.Width(200));
        doTweenPath.wayPoints[index].showOutEvents = EditorGUILayout.Toggle("OutEvents", doTweenPath.wayPoints[index].showOutEvents, GUILayout.Width(200));
        if (doTweenPath.wayPoints[index].showInEvents == true)
        {
            EditorGUILayout.PropertyField(wayPoints.GetArrayElementAtIndex(index).FindPropertyRelative("inEvents"),GUILayout.Width(300));
        }
        if (doTweenPath.wayPoints[index].showOutEvents == true)
        {
            EditorGUILayout.PropertyField(wayPoints.GetArrayElementAtIndex(index).FindPropertyRelative("outEvents"), GUILayout.Width(300));

        }    
    }



    void OnSceneGUI(){
        DrawLine(doTweenPath.wayPoints);
        DrawHandle(doTweenPath.wayPoints);
    }

    void DrawLine(List<WayPoint> points){
        if (points.Count < 3)
            return;
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (i == points.Count - 1)
                continue;
            Vector3 p1 = doTweenPath.transform.parent.TransformPoint(points[i].position);
            Vector3 p2 = doTweenPath.transform.parent.TransformPoint(points[i + 1].position);
            Handles.DrawLine(p1, p2);
        }
        if (doTweenPath.seal == true)
        {
            Vector3 p1 = doTweenPath.transform.parent.TransformPoint(points[points.Count - 1].position);
            Vector3 p2 = doTweenPath.transform.parent.TransformPoint(points[0].position);
            Handles.DrawLine(p1, p2);
        }
    }

    void DrawHandle(List<WayPoint> points){
        if (points.Count < 3)
            return;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 p = doTweenPath.transform.parent.TransformPoint(points[i].position);
            EditorGUI.BeginChangeCheck();
            p = Handles.DoPositionHandle(p, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                points[i].position = doTweenPath.transform.parent.InverseTransformPoint(p);
            }
        }
    }
}
