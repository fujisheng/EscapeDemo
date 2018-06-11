using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class PlotInfoEditor : EditorWindow {

    string infoPath;
    JsonList<Plot> json;
    Vector2 scroll;

    [MenuItem("MyEditor/Plot Info")]
    static void Init(){
        PlotInfoEditor plotInfoEditor = (PlotInfoEditor)GetWindow(typeof(PlotInfoEditor), true, "Plot Info", false);
        plotInfoEditor.maxSize = new Vector2(900, 600);
        plotInfoEditor.minSize = new Vector2(900, 200);
        plotInfoEditor.Show();
    }

    private void OnEnable()
    {
        infoPath = "Text/";
        json = JsonFile.ReadFromFile<JsonList<Plot>>(infoPath, "plotInfo");
        if (json == null)
            json = new JsonList<Plot>();
        if (json.list.Count == 0)
            json.list.Add(new Plot());
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("save",GUILayout.Width(100))){
            JsonFile.SaveToFile(json, infoPath, "plotInfo");
        }
        EditorGUILayout.EndHorizontal();
        scroll = EditorGUILayout.BeginScrollView(scroll);
        for (int i = 0; i < json.list.Count;i++){
            DrawInfoItem(i);
            GUILayout.Space(10);
        }
        EditorGUILayout.EndScrollView();
    }

    void DrawInfoItem(int index){
        
        EditorGUILayout.LabelField("---------------------------------------------------------------------------------------------------------------------------------------------------");
        EditorGUILayout.BeginHorizontal();
        json.list[index].id = EditorGUILayout.IntField("剧情ID",json.list[index].id, GUILayout.Width(300));
        if(GUILayout.Button("+",GUILayout.Width(20))){
            json.list.Insert(index + 1, new Plot());
        }
        if(GUILayout.Button("-",GUILayout.Width(20))){
            json.list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        if(index<json.list.Count){
            if (json.list[index].dialogueList.Count == 0)
                json.list[index].dialogueList.Add(new Dialogue());
            DrawDialogue(json.list[index].dialogueList);
        }

    }

    void DrawDialogue(List<Dialogue> dialogueList){
        EditorGUILayout.LabelField("对白：");
        EditorGUILayout.LabelField("id                               npcId                           next                          words");
        for (int i = 0; i < dialogueList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            dialogueList[i].id = EditorGUILayout.IntField(dialogueList[i].id, GUILayout.Width(100));
            dialogueList[i].npcId = EditorGUILayout.IntField(dialogueList[i].npcId, GUILayout.Width(100));
            dialogueList[i].next = EditorGUILayout.IntField(dialogueList[i].next, GUILayout.Width(100));
            dialogueList[i].str = EditorGUILayout.TextField(dialogueList[i].str, GUILayout.Width(500));
            if(GUILayout.Button("+",GUILayout.Width(20))){
                dialogueList.Insert(i + 1, new Dialogue());
            }
            if(GUILayout.Button("-",GUILayout.Width(20))){
                dialogueList.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();

        }
    }
}
