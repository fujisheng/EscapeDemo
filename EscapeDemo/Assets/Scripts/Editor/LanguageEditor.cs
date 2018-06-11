using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tools.Json;

public class LanguageEditor : EditorWindow {
    [System.Serializable]
    class LanguageInfo{
        public string Key;
        public string Value;

        public LanguageInfo(string key,string value){
            Key = key;
            Value = value;
        }
    }
    //[System.Serializable]
    class LanguageDic{
        public List<LanguageInfo> dic = new List<LanguageInfo>(); 
    }

    LanguageDic languageDic = new LanguageDic();
    LanguageDic engnishDic = new LanguageDic();
    SystemLanguage languageType;
    bool showMainMenu = true;
    string dataPath;

    [MenuItem("MyEditor/Language Editor")]
    static void Init()
    {
        LanguageEditor languageEditor = (LanguageEditor)EditorWindow.GetWindow(typeof(LanguageEditor), false, "LanguageEditor", false);
        languageEditor.Show();
    }

    private void OnEnable()
    {
        dataPath = "Text/Language/";
        languageType = SystemLanguage.English;
        engnishDic = JsonFile.ReadFromFile<LanguageDic>(dataPath, SystemLanguage.English.ToString());
        if (engnishDic == null)
        {
            engnishDic = new LanguageDic();
        }

    }
    private void OnGUI()
    {
        if (showMainMenu == true)
            ShowMainMenu();
        else 
            ShowLanguageEditor();
    }

    void ShowMainMenu(){
        if (GUILayout.Button("English", GUILayout.Width(200),GUILayout.Height(50))){
            languageType = SystemLanguage.English;
            LoadLanguageInfo();
            showMainMenu = false;
        }
        if (GUILayout.Button("Chinese", GUILayout.Width(200), GUILayout.Height(50)))
        {
            languageType = SystemLanguage.Chinese;
            LoadLanguageInfo();
            showMainMenu = false;
        }
        if (GUILayout.Button("Japanese", GUILayout.Width(200), GUILayout.Height(50)))
        {
            languageType = SystemLanguage.Japanese;
            LoadLanguageInfo();
            showMainMenu = false;
        }
        if (GUILayout.Button("Russian", GUILayout.Width(200), GUILayout.Height(50)))
        {
            languageType = SystemLanguage.Russian;
            LoadLanguageInfo();
            showMainMenu = false;
        }
        if (GUILayout.Button("French", GUILayout.Width(200), GUILayout.Height(50)))
        {
            languageType = SystemLanguage.French;
            LoadLanguageInfo();
            showMainMenu = false;
        }
        if (GUILayout.Button("Korean", GUILayout.Width(200), GUILayout.Height(50)))
        {
            languageType = SystemLanguage.Korean;
            LoadLanguageInfo();
            showMainMenu = false;
        }
        if (GUILayout.Button("German", GUILayout.Width(200), GUILayout.Height(50)))
        {
            languageType = SystemLanguage.German;
            LoadLanguageInfo();
            showMainMenu = false;
        }
    }
    void LoadLanguageInfo(){
        
        languageDic = JsonFile.ReadFromFile<LanguageDic>(dataPath, languageType.ToString());
        if(languageDic ==null)
        {
            languageDic = new LanguageDic(); 
        }
    }
    void ShowLanguageEditor(){
        if (languageDic.dic.Count == 0)
            languageDic.dic.Add(new LanguageInfo(string.Empty, string.Empty));
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("<<<",GUILayout.Width(50))){
            showMainMenu = true;
            JsonFile.SaveToFile(languageDic, dataPath, languageType.ToString());
        }
        if (GUILayout.Button("Save", GUILayout.Width(200)))
        {
            JsonFile.SaveToFile(languageDic,dataPath,languageType.ToString());
        }
        EditorGUILayout.LabelField(languageType.ToString(),EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("key                                                value", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < languageDic.dic.Count;i++){
            EditorGUILayout.BeginHorizontal();
            if(languageType==SystemLanguage.English)
                languageDic.dic[i].Key = EditorGUILayout.TextField(languageDic.dic[i].Key, GUILayout.Width(200));
            else
                languageDic.dic[i].Key = EditorGUILayout.TextField(engnishDic.dic[i].Key,GUILayout.Width(200));
            languageDic.dic[i].Value = EditorGUILayout.TextField(languageDic.dic[i].Value, GUILayout.Width(500));
            if (i == languageDic.dic.Count - 1)
            {
                if (GUILayout.Button("+",GUILayout.Width(20)))
                {
                    if(languageType==SystemLanguage.English)
                        languageDic.dic.Add(new LanguageInfo(string.Empty, string.Empty));
                    else if(languageDic.dic.Count + 1 <= engnishDic.dic.Count)
                        languageDic.dic.Add(new LanguageInfo(string.Empty, string.Empty));
                }
            }
            if(GUILayout.Button("-",GUILayout.Width(20))){
                languageDic.dic.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
