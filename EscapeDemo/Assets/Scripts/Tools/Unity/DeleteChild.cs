using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Unity{
    public class DeleteChild
    {

        public static void Delete(Transform root)
        {
            foreach(Transform child in root.transform){
                Object.DestroyObject(child.gameObject);
            }
        }
        public static void Delete(List<Transform> rootList){
            foreach(var root in rootList){
                Delete(root);
            }
        }
    }
}