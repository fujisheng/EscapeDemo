using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class LevelInfoEditor : EditorWindow {

    string levelInfoPath;
    JsonList<Level> json;
    List<Sprite> iconList;
    List<GameObject> prefabList;

    [MenuItem("MyEditor/Level Info")]
    static void Init()
    {
        LevelInfoEditor levelInfoEditor = (LevelInfoEditor)EditorWindow.GetWindow(typeof(LevelInfoEditor), true, "Level Info", false);
        levelInfoEditor.maxSize = new Vector2(900, 600);
        levelInfoEditor.minSize = new Vector2(900, 200);
        levelInfoEditor.Show();
    }

    void OnEnable()
    {
        levelInfoPath = "Text/";
        json = JsonFile.ReadFromFile<JsonList<Level>>(levelInfoPath, "levelInfo");
        iconList = new List<Sprite>(Resources.LoadAll<Sprite>("Image/Level/"));
        prefabList = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Level/"));
        if (json == null)
            json = new JsonList<Level>();
        if (json.list.Count == 0)
            json.list.Add(new Level());
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存文件", GUILayout.Width(100)))
        {
            JsonFile.SaveToFile(json, levelInfoPath, "levelInfo");
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);


        for (int i = 0; i < json.list.Count;i++){
            EditorGUILayout.BeginHorizontal();
            DrawInfoItem(i);
            EditorGUILayout.EndHorizontal();
        }

    }

    void DrawInfoItem(int index){
        json.list[index].id = EditorGUILayout.IntField(json.list[index].id, GUILayout.Width(100));
        json.list[index].icon = EditorGUILayout.TextField(GetIconName(json.list[index].id), GUILayout.Width(100));
        json.list[index].prefab = EditorGUILayout.TextField(GetPrefabName(json.list[index].id), GUILayout.Width(100));
        if(GUILayout.Button("+",GUILayout.Width(20))){
            json.list.Insert(index+1, new Level());
        }
        if(GUILayout.Button("-",GUILayout.Width(20))){
            json.list.RemoveAt(index);
        }
    }

    void OnDestroy()
    {
        JsonFile.SaveToFile(json, levelInfoPath, "levelInfo");
    }

    string GetIconName(int id){
        if (iconList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id))
            return iconList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id).name;
        else
            return string.Empty;
    }

    string GetPrefabName(int id){
        if (prefabList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id))
            return prefabList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id).name;
        else
            return string.Empty;
    }
}
