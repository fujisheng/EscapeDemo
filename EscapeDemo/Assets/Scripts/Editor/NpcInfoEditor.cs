using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class NpcInfoEditor : EditorWindow
{

    string infoPath;
    JsonList<Npc> json;
    List<GameObject> prefabList;

    [MenuItem("MyEditor/Npc Info")]
    static void Init()
    {
        NpcInfoEditor npcInfoEditor = (NpcInfoEditor)EditorWindow.GetWindow(typeof(NpcInfoEditor), true, "Npc Info", false);
        npcInfoEditor.maxSize = new Vector2(900, 600);
        npcInfoEditor.minSize = new Vector2(900, 200);
        npcInfoEditor.Show();
    }

    void OnEnable()
    {
        infoPath = "Text/";
        json = JsonFile.ReadFromFile<JsonList<Npc>>(infoPath, "npcInfo");
        prefabList = new List<GameObject>(Resources.LoadAll<GameObject>("Prefab/Npc"));
        if (json == null)
            json = new JsonList<Npc>();
        if (json.list.Count == 0)
            json.list.Add(new Npc());
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存文件", GUILayout.Width(100)))
        {
            JsonFile.SaveToFile(json, infoPath, "npcInfo");
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("id                               name                            prefab");
        GUILayout.Space(10);

        for (int i = 0; i < json.list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            DrawInfoItem(i);
            EditorGUILayout.EndHorizontal();
        }

    }

    void DrawInfoItem(int index)
    {
        json.list[index].id = EditorGUILayout.IntField(json.list[index].id, GUILayout.Width(100));
        json.list[index].name = EditorGUILayout.TextField(json.list[index].name, GUILayout.Width(100));
        json.list[index].prefab = EditorGUILayout.TextField(GetPrefabName(json.list[index].id), GUILayout.Width(100));
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            json.list.Insert(index + 1, new Npc());
        }
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            json.list.RemoveAt(index);
        }
    }

    void OnDestroy()
    {
        JsonFile.SaveToFile(json, infoPath, "npcInfo");
    }

    string GetPrefabName(int id)
    {
        if (prefabList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id))
            return prefabList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id).name;
        else
            return string.Empty;
    }
}
