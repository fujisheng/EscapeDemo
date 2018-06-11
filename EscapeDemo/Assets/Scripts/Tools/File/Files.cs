using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

namespace Tools.Files{
    public class Files
    {
        static string path = Application.persistentDataPath;

        public static void SaveFile<T>(string name, T file)
        {
            StreamWriter sw = new StreamWriter(path + "//" + name);
            FileInfo fileInfo = new FileInfo(path + "//" + name);
            string json = JsonUtility.ToJson(file);
            if (!fileInfo.Exists)
                sw = fileInfo.CreateText();
            else
                sw.Write(json);
            sw.Close();
            sw.Dispose();
        }

        public static T LoadFile<T>(string name)
        {
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                return default(T);
            }
            string allStr;
            allStr = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            T t = JsonUtility.FromJson<T>(allStr);
            return t;
        }

        public static void DeleteFile(string name)
        {
            File.Delete(path + "//" + name);
        }
    }
}