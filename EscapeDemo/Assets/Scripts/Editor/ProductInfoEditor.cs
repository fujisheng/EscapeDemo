using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class ProductInfoEditor : EditorWindow {

    Vector2 scrollPos = new Vector2();
    string productInfoPath;
    JsonList<Product> json = new JsonList<Product>();

    [MenuItem("MyEditor/Product Info")]
    static void Init()
    {
        ProductInfoEditor productInfoEditor = (ProductInfoEditor)EditorWindow.GetWindow(typeof(ProductInfoEditor), true, "Product Info", false);
        productInfoEditor.maxSize = new Vector2(400, 600);
        productInfoEditor.minSize = new Vector2(400, 200);
        productInfoEditor.Show();
    }

    void OnEnable()
    {
        productInfoPath = "Text/";
        json = JsonFile.ReadFromFile<JsonList<Product>>(productInfoPath, "productInfo");
        if (json == null)
            json = new JsonList<Product>();
        if (json.list.Count == 0)
        {
            json.list.Add(new Product());
        }
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存文件", GUILayout.Width(100)))
        {
            SaveInfoToFile();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("     ID                       Gem                       Type", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < json.list.Count; i++)
        {
            DrawInfoItem(i);
        }
        GUILayout.Space(10);
        EditorGUILayout.EndScrollView();
        GUILayout.Space(10);
    }
    void DrawInfoItem(int index)
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        json.list[index].id = EditorGUILayout.TextField(json.list[index].id, GUILayout.Width(100));
        json.list[index].coin = EditorGUILayout.IntField(json.list[index].coin, GUILayout.Width(100));
        json.list[index].type = (ProductType)EditorGUILayout.EnumPopup(json.list[index].type,GUILayout.Width(100));
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            Product goods = new Product();
            json.list.Insert(index + 1, goods);
        }
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            json.list.RemoveAt(index);
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }

    void SaveInfoToFile()
    {
        JsonFile.SaveToFile(json, productInfoPath, "productInfo");
    }
    void OnDestroy()
    {
        SaveInfoToFile();
    }
}
