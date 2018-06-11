using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;
using LocalAd;

public class LocalBannerInfoEditor : EditorWindow
{

    string propInfoPath;
    JsonList<LocalBannerData> json;
    List<Sprite> iconList;
    Dictionary<int, Sprite> iconDic = new Dictionary<int, Sprite>();
    bool showIcon;
    Vector2 scrollVector;

    [MenuItem("MyEditor/LocalBanner Info")]
    static void Init()
    {
        LocalBannerInfoEditor propInfoEditor = (LocalBannerInfoEditor)EditorWindow.GetWindow(typeof(LocalBannerInfoEditor), false, "LocalBanner Info", false);
        propInfoEditor.maxSize = new Vector2(900, 600);
        propInfoEditor.minSize = new Vector2(900, 200);
        propInfoEditor.Show();
    }

    void OnEnable()
    {
        propInfoPath = "Text/";
        json = JsonFile.ReadFromFile<JsonList<LocalBannerData>>(propInfoPath, "localBannerInfo");
        iconList = new List<Sprite>(Resources.LoadAll<Sprite>("Image/Banner/"));
        foreach(var icon in iconList){
            iconDic.Add(int.Parse(icon.name.Split('_')[0]), icon);
        }
        if (json == null)
            json = new JsonList<LocalBannerData>();
        if (json.list.Count == 0)
            json.list.Add(new LocalBannerData());
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存文件", GUILayout.Width(100)))
        {
            JsonFile.SaveToFile(json, propInfoPath, "localBannerInfo");
        }
        if(GUILayout.Button("显示图标",GUILayout.Width(100))){
            showIcon = showIcon == true ? false : true;
        }
        if(GUILayout.Button("刷新",GUILayout.Width(100))){
            iconList = new List<Sprite>(Resources.LoadAll<Sprite>("Image/Banner/"));
            iconDic.Clear();
            foreach (var icon in iconList)
            {
                iconDic.Add(int.Parse(icon.name.Split('_')[0]), icon);
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("id                               sprite                            url                                          type ");
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
        json.list[index].sprite= EditorGUILayout.TextField(GetIconName(json.list[index].id), GUILayout.Width(100));
        json.list[index].appStoreUrl = EditorGUILayout.TextField(json.list[index].appStoreUrl, GUILayout.Width(200));
        json.list[index].googlePlayUrl = EditorGUILayout.TextField(json.list[index].googlePlayUrl, GUILayout.Width(200));
        json.list[index].type=(LocalBannerType) EditorGUILayout.EnumPopup(json.list[index].type, GUILayout.Width(100));

        if(showIcon==true){
            EditorGUILayout.ObjectField(GetIcon(json.list[index].id), typeof(Sprite), GUILayout.Width(50),GUILayout.Height(50));
        }

        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            json.list.Insert(index + 1, new LocalBannerData());
        }
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            json.list.RemoveAt(index);
        }
    }

    void OnDestroy()
    {
        JsonFile.SaveToFile(json, propInfoPath, "localBannerInfo");
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
