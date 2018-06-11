using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PartManager : MonoBehaviour,IListener,IValueSender {

    Dictionary<int, GameObject> partDic = new Dictionary<int, GameObject>();
    List<GameObject> openedPart = new List<GameObject>();
    int nowPart;

    private void Awake()
    {
        foreach(Transform child in transform){
            partDic.Add(int.Parse(child.name.Split('_')[0]), child.gameObject);
        }

        Mediator.AddListener(this, "openPart","closePart");
        Mediator.AddValue(this, "nowPart");
    }

    private void OnDestroy()
    {
        Mediator.RemoveListener(this);
    }

    public void OnNotify(string notify,object args){
        switch (notify){
            case "openPart":
                Open((int)args);
                break;
            case "closePart":
                Close();
                break;
        }
    }

    public object OnGetValue(string valueType){
        switch(valueType){
            case "nowPart":
                return nowPart;
            default:
                return null;
        }
    }

    void Open(int id){
        if (!partDic.ContainsKey(id))
        {
            Debug.Log("dont have this part " + id);
            return;
        }
        transform.DOScale(Vector3.one, 0f);
        partDic[id].transform.SetAsLastSibling();
        partDic[id].transform.DOScale(Vector3.one, 0.2f).OnComplete(()=>{
            openedPart.ForEach((part) => part.transform.DOScale(Vector3.zero, 0f));
            openedPart.Add(partDic[id]);

            nowPart = id;
            Mediator.SendMassage("openView", "partView");
        });
    }

    public void Close(){
        if (openedPart.Count == 0)
            return;
        openedPart.Last().transform.DOScale(Vector3.zero, 0f);
        GameObject obj = openedPart.Last();
        openedPart.Remove(obj);
        if (openedPart.Count == 0){
            nowPart = 0;
            transform.DOScale(Vector3.zero, 0f);
            Mediator.SendMassage("closeView", "partView");
            return;
        }
        openedPart.Last().transform.SetAsLastSibling();
        openedPart.Last().transform.DOScale(Vector3.one, 0f);
        nowPart = int.Parse(openedPart.Last().name.Split('_')[0]);
    }
}
