using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Unity;

namespace Tools.Code{
    public class CodeProgress
    {
        List<Action> codeList = new List<Action>();
        public Action<float> onProgress;

        public CodeProgress(params Action[] actions){
            codeList = new List<Action>(actions); 
        }

        public Coroutine Excute(){
            return CoroutineManager.StartCoroutine(_Excute());
        }

        IEnumerator _Excute(){
            for (int i = 0; i < codeList.Count;i++){
                codeList[i].Invoke();
                yield return 0;
                onProgress.Invoke(((float)(i + 1))/ (float)codeList.Count);
            }
        }
    }
}