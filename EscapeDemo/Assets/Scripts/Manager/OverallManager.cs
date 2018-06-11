using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverallManager : MonoBehaviour,IListener,IValueSender {

    Dictionary<int, GameObject> overallDic = new Dictionary<int, GameObject>();

    int nowOver = 0;

    private void Awake()
    {
        foreach(Transform child in transform){
            overallDic.Add(int.Parse(child.name.Split('_')[0]), child.gameObject);
        }

        Mediator.AddListener(this, "openOverall");
        Mediator.AddValue(this, "nowOverall");
    }

    private void OnDestroy()
    {
        Mediator.RemoveListener(this);
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "openOverall":
                Open((int)args);
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "nowOverall":
                return nowOver;
            default:
                return null;
        }
    }

    void Open(int id){
        if(!overallDic.ContainsKey(id)){
            Debug.Log("dont have this overall " + id);
            return;
        }
        overallDic[id].transform.DOScale(Vector3.one, 0f);
        overallDic[id].transform.SetAsLastSibling();
        nowOver = id;
    }
}
