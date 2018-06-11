using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class PropInfoEditor : EditorWindow
{

    string propInfoPath;
    JsonList<Props> json;
    List<Sprite> iconList;
    Dictionary<int, Sprite> iconDic = new Dictionary<int, Sprite>();
    bool showIcon;
    Vector2 scrollVector;

    [MenuItem("MyEditor/Props Info")]
    static void Init()
    {
        PropInfoEditor propInfoEditor = (PropInfoEditor)EditorWindow.GetWindow(typeof(PropInfoEditor), false, "Props Info", false);
        propInfoEditor.maxSize = new Vector2(900, 600);
        propInfoEditor.minSize = new Vector2(900, 200);
        propInfoEditor.Show();
    }

    void OnEnable()
    {
        propInfoPath = "Text/";
        json = JsonFile.ReadFromFile<JsonList<Props>>(propInfoPath, "propsInfo");
        iconList = new List<Sprite>(Resources.LoadAll<Sprite>("Image/Props/"));
        foreach(var icon in iconList){
            iconDic.Add(int.Parse(icon.name.Split('_')[0]), icon);
        }
        if (json == null)
            json = new JsonList<Props>();
        if (json.list.Count == 0)
            json.list.Add(new Props());
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存文件", GUILayout.Width(100)))
        {
            JsonFile.SaveToFile(json, propInfoPath, "propsInfo");
        }
        if(GUILayout.Button("显示图标",GUILayout.Width(100))){
            showIcon = showIcon == true ? false : true;
        }
        if(GUILayout.Button("刷新",GUILayout.Width(100))){
            iconList = new List<Sprite>(Resources.LoadAll<Sprite>("Image/Props/"));
            iconDic.Clear();
            foreach (var icon in iconList)
            {
                iconDic.Add(int.Parse(icon.name.Split('_')[0]), icon);
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("id                               name                            icon                          usageCount                     resultCount               resultId               type                 consumType");
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
        json.list[index].name = EditorGUILayout.TextField(GetName(json.list[index].id), GUILayout.Width(100));
        json.list[index].icon = EditorGUILayout.TextField(GetIconName(json.list[index].id), GUILayout.Width(100));
        json.list[index].usageCount = EditorGUILayout.IntField(json.list[index].usageCount, GUILayout.Width(100));
        json.list[index].resultCount = EditorGUILayout.IntField(json.list[index].resultCount, GUILayout.Width(100));
        json.list[index].resultId = EditorGUILayout.IntField(json.list[index].resultId, GUILayout.Width(100));
        json.list[index].type = (PropsType)EditorGUILayout.EnumPopup(json.list[index].type, GUILayout.Width(100));
        json.list[index].consumType = (ConsumType)EditorGUILayout.EnumPopup(json.list[index].consumType, GUILayout.Width(100));

        if(showIcon==true){
            EditorGUILayout.ObjectField(GetIcon(json.list[index].id), typeof(Sprite), GUILayout.Width(50),GUILayout.Height(50));
        }

        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            json.list.Insert(index + 1, new Props());
        }
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            json.list.RemoveAt(index);
        }
    }

    void OnDestroy()
    {
        JsonFile.SaveToFile(json, propInfoPath, "propsInfo");
    }

    string GetIconName(int id)
    {
        if (iconList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id))
            return iconList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id).name;
        else
            return string.Empty;
    }

    Sprite GetIcon(int id){
        if(iconDic.ContainsKey(id)){
            return iconDic[id];
        }
        return null;
    }

    string GetName(int id){
        if (iconList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id))
            return iconList.Find((obj) => int.Parse(obj.name.Split('_')[0]) == id).name.Split('_')[1];
        else
            return string.Empty;
    }
}
