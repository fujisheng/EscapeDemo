using System.Collections;
using UnityEngine;
namespace Tools.Unity
{
    public class Mono : MonoBehaviour
    {
    }
    public class MonoManager
    {
        public static Mono mono;
        static MonoManager()
        {
            Init();
        }
        static void Init()
        {
            if (GameObject.Find("[MonoBehaviour]") != null)
                return;
            var go = new GameObject();
            go.name = "[MonoBehaviour]";
            mono = go.AddComponent<Mono>();
            Object.DontDestroyOnLoad(go);
        }
    }
    public class CoroutineManager
    {
        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return MonoManager.mono.StartCoroutine(routine);
        }
        public static void StopCoroutine(IEnumerator routine){
            MonoManager.mono.StopCoroutine(routine);
        }
    }

}
