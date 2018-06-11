using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;

public class LanguageManager{

    [System.Serializable]
    class LanguageInfo
    {
        public string Key;
        public string Value;

        public LanguageInfo(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    [System.Serializable]
    class LanguageDic
    {
        public List<LanguageInfo> dic = new List<LanguageInfo>();
    }

    static LanguageManager instance;
    LanguageDic languageDic = new LanguageDic();
    LanguageDic EnglishDic = new LanguageDic();
    SystemLanguage languageType = SystemLanguage.English;
    string dataPath = "Text/Language/";

    public void Init(){
        SetLanguageType();
        EnglishDic = JsonFile.ReadFromFile<LanguageDic>(dataPath, "English");
        if(languageType==SystemLanguage.English)
            languageDic = EnglishDic;
        else if (JsonFile.ReadFromFile<LanguageDic>(dataPath, languageType.ToString()) == null)
            languageDic = EnglishDic;
        else
            languageDic = JsonFile.ReadFromFile<LanguageDic>(dataPath, languageType.ToString());
    }

    void SetLanguageType(){
        switch(Application.systemLanguage){
            case SystemLanguage.English:
                languageType = SystemLanguage.English;
                break;
            case SystemLanguage.ChineseSimplified:
                languageType = SystemLanguage.Chinese;
                break;
            case SystemLanguage.ChineseTraditional:
                languageType = SystemLanguage.Chinese;
                break;
            case SystemLanguage.Japanese:
                languageType = SystemLanguage.Japanese;
                break;
            case SystemLanguage.Russian:
                languageType = SystemLanguage.Russian;
                break;
            case SystemLanguage.French:
                languageType = SystemLanguage.French;
                break;
            case SystemLanguage.Korean:
                languageType = SystemLanguage.Korean;
                break;
            case SystemLanguage.German:
                languageType = SystemLanguage.German;
                break;
            default:
                languageType = SystemLanguage.English;
                break;
        }
    }

    public static LanguageManager GetInstance(){
        if (instance == null)
            instance = new LanguageManager();
        return instance;
    }

    public string GetString(string key){
        if (string.IsNullOrEmpty(GetString(key, EnglishDic)))
        {
            Debug.LogWarning("没有添加 <" + key + "> 的多语言");
            return string.Empty;
        }
        else if (string.IsNullOrEmpty(GetString(key, languageDic)))
            return GetString(key, EnglishDic);
        else
            return GetString(key, languageDic);
    }

    string GetString(string key,LanguageDic dic){
        foreach (var str in dic.dic)
        {
            if (str.Key == key)
            {
                if (!string.IsNullOrEmpty(str.Value))
                    return str.Value;
                else
                    return string.Empty;
            }
        }
        return string.Empty;
    }
}
