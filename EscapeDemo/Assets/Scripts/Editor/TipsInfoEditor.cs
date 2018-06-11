using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class TipsInfoEditor : EditorWindow
{

    string tipsInfoPath;
    JsonList<Tips> json;
    List<Sprite> spriteList;
    Vector2 scrollVector;

    [MenuItem("MyEditor/Tips Info")]
    static void Init()
    {
        TipsInfoEditor tipsInfoEditor = (TipsInfoEditor)EditorWindow.GetWindow(typeof(TipsInfoEditor), false, "Tips Info", false);
        tipsInfoEditor.maxSize = new Vector2(900, 600);
        tipsInfoEditor.minSize = new Vector2(900, 200);
        tipsInfoEditor.Show();
    }

    void OnEnable()
    {
        tipsInfoPath = "Text/";
        json = JsonFile.ReadFromFile<JsonList<Tips>>(tipsInfoPath, "tipsInfo");
        spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("Image/Tips/"));
        if (json == null)
            json = new JsonList<Tips>();
        if (json.list.Count == 0)
            json.list.Add(new Tips());
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存文件", GUILayout.Width(100)))
        {
            JsonFile.SaveToFile(json, tipsInfoPath, "tipsInfo");
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("id                               levelId                            partId                          sprite                     price");
        GUILayout.Space(10);


        scrollVector = EditorGUILayout.BeginScrollView(scrollVector);
        for (int i = 0; i < json.list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            DrawInfoItem(i);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    void DrawInfoItem(int index)
    {
        json.list[index].id = EditorGUILayout.IntField(json.list[index].id, GUILayout.Width(100));
        json.list[index].levelId = EditorGUILayout.IntField(json.list[index].levelId, GUILayout.Width(100));
        json.list[index].partId = EditorGUILayout.TextField(json.list[index].partId, GUILayout.Width(100));
        json.list[index].sprite = EditorGUILayout.TextField(GetSprite(json.list[index].id), GUILayout.Width(100));
        json.list[index].price = EditorGUILayout.IntField(json.list[index].price, GUILayout.Width(100));

        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            json.list.Insert(index + 1, new Tips());
        }
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            json.list.RemoveAt(index);
        }
    }

    void OnDestroy()
    {
        JsonFile.SaveToFile(json, tipsInfoPath, "tipsInfo");
    }

    string GetSprite(int id){
        foreach (var sprite in spriteList){
            if (int.Parse(sprite.name.Split('_')[0]) == id)
                return sprite.name;
        }
        return string.Empty;
    }
}
