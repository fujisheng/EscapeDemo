using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Tools.Json
{
    [System.Serializable]
    public class JsonKeyValuePair<T1,T2>{
        public T1 key;
        public T2 valuem;

        public JsonKeyValuePair(T1 t1,T2 t2){
            key = t1;
            valuem = t2;
        }
    }

    [System.Serializable]
    public class JsonDic<T1,T2>{
        public List<JsonKeyValuePair<T1, T2>> dic = new List<JsonKeyValuePair<T1, T2>>();

        public void FromDictionary(Dictionary<T1,T2> dictionary){
            foreach(var pair in dictionary){
                dic.Add(new JsonKeyValuePair<T1, T2>(pair.Key, pair.Value));
            }
        }
        public Dictionary<T1,T2> ToDictionary(){
            Dictionary<T1, T2> _dic = new Dictionary<T1, T2>();
            foreach(var list in dic){
                _dic.Add(list.key, list.valuem);
            }
            return _dic;
        }
    }

    [System.Serializable]
    public class JsonList<T>{
        public List<T> list = new List<T>();
        public List<T> ToList(){
            return list;
        }
    }


    public class JsonFile
    {
        public static void SaveToFile(object jsonData, string filePath, string fileName)
        {
            string json = JsonUtility.ToJson(jsonData);
            string savePath = Application.dataPath + "/Resources/"+filePath + fileName + ".json";
            StreamWriter sw = new StreamWriter(savePath);
            FileInfo fileInfo = new FileInfo(savePath);
            if (!fileInfo.Exists)
                sw = fileInfo.CreateText();
            else
                sw.Write(json);
            sw.Close();
            sw.Dispose();
            //File.WriteAllText(savePath, json, Encoding.UTF8);
        }

        public static T ReadFromFile<T>(string filePath, string fileName)
        {
            TextAsset json = Resources.Load<TextAsset>(filePath + fileName);
            if (json == null)
                return default(T);
            string str = json.ToString();
            return (T)JsonUtility.FromJson<T>(str);
        }
	}
}
